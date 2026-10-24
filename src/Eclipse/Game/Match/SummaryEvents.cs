using System.Collections.Generic;
using Eclipse.Core;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.LocalSettings;
using MiraAPI.Utilities;
using Reactor.Utilities;
using MiraAPI.Translation;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Eclipse.Game;

internal static class SummaryEvents
{
    private const int MaxLines = 15;

    private static readonly DeathBook Deaths = new();

    public static string LastText { get; private set; } = string.Empty;

    [RegisterEvent(-1)]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        if (roundStart.TriggeredByIntro)
        {
            Deaths.Clear();
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

    public static void Capture()
    {
        LastText = string.Empty;
        if (!GameData.Instance)
        {
            Logger<EclipsePlugin>.Info("Match summary: no game data when the match ended.");
            return;
        }

        LastText = SummaryFormatter.Build(Collect(), Labels(), MaxLines);
        Logger<EclipsePlugin>.Info($"Match summary: captured, {LastText.Length} characters.");
    }
    public static void Show(EndGameManager manager)
    {
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
            label.text = "<line-height=1em>" + LastText;
            Logger<EclipsePlugin>.Info("Match summary: drawn.");
        }
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
        var role = info.IsDead && info.RoleWhenAlive.HasValue ? RoleManager.Instance.GetRole(info.RoleWhenAlive.Value) : info.Role;
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
            ColorUtility.ToHtmlStringRGB(Palette.PlayerColors[info.DefaultOutfit.ColorId]),
            role.GetRoleName(),
            ColorUtility.ToHtmlStringRGB(RoleLabels.ColorOf(role)),
            fate,
            killerName == null ? null : NameSanitizer.Clean(killerName),
            note?.Round ?? 0,
            done,
            info.Tasks.Count,
            winner);
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
