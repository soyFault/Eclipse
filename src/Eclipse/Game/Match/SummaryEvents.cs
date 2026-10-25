using System.Collections.Generic;
using Eclipse.Core;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.LocalSettings;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using MiraAPI.Translation;
using AmongUs.GameOptions;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Eclipse.Game;

internal static class SummaryEvents
{
    private const int MaxLines = 15;

    private static readonly DeathBook Deaths = new();

    // The role each player had while alive, as last seen (the dead have a ghost role, and the game's own "role when alive" has been wrong in tests).
    private static readonly Dictionary<byte, RoleTypes> LivingRoles = [];

    public static string LastText { get; private set; } = string.Empty;

    private static List<SummaryLine> _lines = [];

    // A new match starts with an empty book.
    [RegisterEvent(-1)]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        if (roundStart.TriggeredByIntro)
        {
            Deaths.Clear();
            LivingRoles.Clear();
            RememberAllLivingRoles();
        }
    }

    // Exiles happen after a meeting starts, so the roles are written down then.
    [RegisterEvent(-1)]
    public static void OnStartMeeting(StartMeetingEvent meeting)
    {
        RememberAllLivingRoles();
    }

    // The victim of a murder is still alive here, with the role that matters.
    [RegisterEvent(-1)]
    public static void OnBeforeMurder(BeforeMurderEvent murder)
    {
        RememberLivingRole(murder.Target?.Data);
    }

    private static void RememberAllLivingRoles()
    {
        if (!GameData.Instance)
        {
            return;
        }

        var players = GameData.Instance.AllPlayers;
        for (var i = 0; i < players.Count; i++)
        {
            RememberLivingRole(players[i]);
        }
    }

    private static void RememberLivingRole(NetworkedPlayerInfo? info)
    {
        if (info != null && info.Role != null && !info.IsDead && !info.Role.IsDead)
        {
            LivingRoles[info.PlayerId] = info.Role.Role;
        }
    }

    [RegisterEvent]
    public static void OnPlayerDeath(PlayerDeathEvent death)
    {
        var victim = death.Player;
        if (victim == null)
        {
            return;
        }

        var fate = death.DeathReason switch
        {
            DeathReason.Kill => PlayerFate.Killed,
            DeathReason.Exile => PlayerFate.Exiled,
            _ => PlayerFate.Died,
        };

        var killer = fate == PlayerFate.Killed && KillLogEvents.Log.LatestOf(victim.PlayerId) is { } record ? record.KillerId : DeathBook.NoKiller;
        Deaths.Record(victim.PlayerId, fate, killer, MatchRounds.Current);
    }

    [RegisterEvent]
    public static void OnPlayerLeave(PlayerLeaveEvent leave)
    {
        if (leave.ClientData.Character is { } leaver)
        {
            Deaths.Record(leaver.PlayerId, PlayerFate.Disconnected, DeathBook.NoKiller, MatchRounds.Current);
        }
    }

    // Called when the game ends, while the players' data still exists (it is gone by the time the end screen starts).
    public static void Capture()
    {
        LastText = string.Empty;
        if (!GameData.Instance)
        {
            Logger<EclipsePlugin>.Info("Match summary: no game data when the match ended.");
            return;
        }

        _lines = Collect();
        LastText = SummaryFormatter.Build(_lines, Labels(), MaxLines);
        Logger<EclipsePlugin>.Info($"Match summary: captured, {LastText.Length} characters.");
    }

    // Called from the end screen. Every reason for not drawing the summary leaves one line in the log.
    public static void Show(EndGameManager manager)
    {
        Refresh();
        var mode = LocalSettingsTabSingleton<EclipseLocalTabPreferences>.Instance.EndSummary.Value;
        if (manager == null || mode == EndSummaryMode.Hidden || LastText.Length == 0)
        {
            Logger<EclipsePlugin>.Info($"Match summary: not drawn (setting {mode}, {LastText.Length} characters).");
            return;
        }

        var lines = LastText.Split('\n').Length;
        var label = EndScreenText.Create(manager, "EclipseMatchSummary", SummaryFormatter.FontSize(lines), TextAlignmentOptions.TopLeft, 0.02f, 0.95f);
        if (label != null)
        {
            // Every line gets the same height: a line with a glyph from the fallback font (the star, some accents) is taller on its own.
            label.text = "<line-height=1em>" + LastText;
            Logger<EclipsePlugin>.Info("Match summary: drawn.");
        }
    }

    // The winners of a neutral ending (a Jester who was voted out) are only known on the end screen, so the marks are set again from the final list.
    private static void Refresh()
    {
        if (_lines.Count == 0)
        {
            return;
        }

        var winners = new HashSet<string>();
        foreach (var winner in EndGameResult.CachedWinners)
        {
            winners.Add(NameSanitizer.Clean(winner.PlayerName));
        }

        var lines = _lines.ConvertAll(line => line with { Winner = winners.Contains(line.Name) });
        lines.Sort(SummaryFormatter.Compare);
        _lines = lines;
        LastText = SummaryFormatter.Build(_lines, Labels(), MaxLines);
    }

    private static string IconOf(RoleBehaviour role)
    {
        var icon = role is ICustomRole custom ? custom.Configuration.IconTmp : null;
        return icon == null ? string.Empty : $"<sprite name=\"{icon.name}\">";
    }

    private static List<SummaryLine> Collect()
    {
        var winners = new HashSet<string>();
        foreach (var winner in EndGameResult.CachedWinners)
        {
            winners.Add(winner.PlayerName);
        }

        var lines = new List<SummaryLine>();
        var players = GameData.Instance.AllPlayers;
        for (var i = 0; i < players.Count; i++)
        {
            var info = players[i];
            if (info == null || info.Role == null)
            {
                continue;
            }

            lines.Add(LineOf(info, winners.Contains(info.PlayerName)));
        }

        lines.Sort(SummaryFormatter.Compare);
        return lines;
    }

    private static SummaryLine LineOf(NetworkedPlayerInfo info, bool winner)
    {
        var role = LivingRoleOf(info);
        Logger<EclipsePlugin>.Info($"Match summary row: '{info.PlayerName}' color {info.DefaultOutfit.ColorId}, role {info.Role.Role}, when alive {(info.RoleWhenAlive.HasValue ? info.RoleWhenAlive.Value.ToString() : "-")}, remembered {(LivingRoles.TryGetValue(info.PlayerId, out var seen) ? seen.ToString() : "-")}, shown {role.Role}.");
        var note = Deaths.Get(info.PlayerId);
        var fate = info.Disconnected ? PlayerFate.Disconnected : info.IsDead ? note?.Fate ?? PlayerFate.Died : PlayerFate.Alive;
        var killerName = note is { KillerId: not DeathBook.NoKiller } ? GameData.Instance.GetPlayerById(note.KillerId)?.PlayerName : null;

        var done = 0;
        for (var i = 0; i < info.Tasks.Count; i++)
        {
            if (info.Tasks[i].Complete)
            {
                done++;
            }
        }

        return new SummaryLine(
            NameSanitizer.Clean(info.PlayerName),
            ColorUtility.ToHtmlStringRGB(PlayerColorOf(info)),
            role.GetRoleName(),
            ColorUtility.ToHtmlStringRGB(RoleLabels.ColorOf(role)),
            fate,
            killerName == null ? null : NameSanitizer.Clean(killerName),
            note?.Round ?? 0,
            done,
            info.Tasks.Count,
            winner,
            IconOf(role));
    }

    private static RoleBehaviour LivingRoleOf(NetworkedPlayerInfo info)
    {
        var current = info.IsDead ? null : info.Role;
        var remembered = LivingRoles.TryGetValue(info.PlayerId, out var seen) ? RoleManager.Instance.GetRole(seen) : null;
        var whenAlive = info.RoleWhenAlive.HasValue ? RoleManager.Instance.GetRole(info.RoleWhenAlive.Value) : null;

        // A plain crewmate or impostor is what the game falls back to, so a more specific role from any other source wins over it.
        foreach (var candidate in new[] { current, remembered, whenAlive })
        {
            if (candidate != null && !IsPlain(candidate.Role))
            {
                return candidate;
            }
        }

        return current ?? remembered ?? whenAlive ?? info.Role;
    }

    private static bool IsPlain(RoleTypes role)
    {
        return role is RoleTypes.Crewmate or RoleTypes.Impostor or RoleTypes.CrewmateGhost or RoleTypes.ImpostorGhost;
    }

    // The color the player's body is drawn with (that is what everybody recognises), or the palette's if the body is not there.
    private static Color PlayerColorOf(NetworkedPlayerInfo info)
    {
        var player = info.Object;
        var body = player != null && player.cosmetics != null && player.cosmetics.currentBodySprite != null ? player.cosmetics.currentBodySprite.BodySprite.material : null;
        return body != null ? body.GetColor(ShaderID.BodyColor) : Palette.PlayerColors[info.DefaultOutfit.ColorId];
    }

    private static SummaryLabels Labels()
    {
        return new SummaryLabels(
            MiraLocaleManager.Get("Eclipse.Summary.Alive"),
            MiraLocaleManager.Get("Eclipse.Summary.Killed"),
            MiraLocaleManager.Get("Eclipse.Summary.Exiled"),
            MiraLocaleManager.Get("Eclipse.Summary.Disconnected"),
            MiraLocaleManager.Get("Eclipse.Summary.Died"),
            MiraLocaleManager.Get("Eclipse.Summary.Winner"));
    }
}
