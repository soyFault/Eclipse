using System.Collections.Generic;
using System.Text;
using Eclipse.Api.Alignments;
using Eclipse.Api.Factions;
using Eclipse.Api.Roles;
using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Options.Roles.Crewmate;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using UnityEngine;

namespace Eclipse.Roles.Crewmate.Investigative;

internal sealed class SnitchRole : CrewmateRole, IEclipseRole, IDoomable
{
    private readonly Dictionary<byte, TargetArrow> _impostorArrows = [];
    private TargetArrow? _snitchArrow;
    private float _sinceRecount;

    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Snitch";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Insight;

    public Color RoleColor => EclipseColors.Snitch;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewInvestigative;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };

    public override bool IsAffectedByComms => false;

    [HideFromIl2Cpp]
    public SnitchStage Stage { get; private set; }

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        Stage = SnitchStage.Unrevealed;
        ClearArrows();

        // A player can become a Snitch with tasks already done (a role change): count them without announcing anything.
        CheckTasks(silent: true);
    }

    public override void OnDeath(DeathReason reason)
    {
        RoleBehaviourStubs.OnDeath(this, reason);
        ClearArrows();
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        ClearArrows();
    }

    public void CheckTasks(bool silent = false)
    {
        if (Player == null || Player.Data == null)
        {
            return;
        }

        var completed = 0;
        var total = 0;
        foreach (var task in Player.Data.Tasks)
        {
            total++;
            if (task.Complete)
            {
                completed++;
            }
        }

        var remaining = (int)OptionGroupSingleton<SnitchOptions>.Instance.TasksRemainingWhenRevealed;
        var next = SnitchRules.Advance(Stage, completed, total, remaining);
        if (next == Stage)
        {
            return;
        }

        Stage = next;
        if (!silent)
        {
            Announce(next);
        }
    }

    public void Tick(float deltaTime)
    {
        // The tasks are looked at again once a second: finishing one raises an event, but this does not depend on it.
        _sinceRecount += deltaTime;
        if (_sinceRecount >= 1f)
        {
            _sinceRecount = 0f;
            CheckTasks();
        }

        var local = PlayerControl.LocalPlayer;
        if (local == null || local.Data == null || Player == null || Player.Data == null)
        {
            return;
        }

        var snitchAlive = !Player.Data.IsDead;
        if (Player.AmOwner)
        {
            ShowImpostors(local, snitchAlive);
        }
        else
        {
            ShowSnitch(local, snitchAlive);
        }
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var text = CustomRoleUtils.CreateForRole(this);
        if (Stage == SnitchStage.CompletedTasks)
        {
            text.AppendLine($"<b>{MiraLocaleManager.Get("Eclipse.Role.Snitch.TabCompleted")}</b>");
        }

        return text;
    }

    private void ShowImpostors(PlayerControl local, bool snitchAlive)
    {
        var wanted = new HashSet<byte>();
        if (snitchAlive && Stage == SnitchStage.CompletedTasks)
        {
            foreach (var other in PlayerControl.AllPlayerControls)
            {
                if (other != null && other != Player && other.Data != null && !other.Data.IsDead && !other.Data.Disconnected && IsImpostorPlayer(other))
                {
                    wanted.Add(other.PlayerId);
                    if (!_impostorArrows.TryGetValue(other.PlayerId, out var arrow))
                    {
                        arrow = TargetArrow.Create(local.transform, Palette.ImpostorRed);
                        _impostorArrows[other.PlayerId] = arrow;
                    }

                    arrow.PointAt(other.transform.position);
                }
            }
        }

        // Whoever died, left or stopped being an impostor loses their arrow.
        var gone = new List<byte>();
        foreach (var id in _impostorArrows.Keys)
        {
            if (!wanted.Contains(id))
            {
                gone.Add(id);
            }
        }

        foreach (var id in gone)
        {
            _impostorArrows[id].Destroy();
            _impostorArrows.Remove(id);
        }
    }

    private void ShowSnitch(PlayerControl local, bool snitchAlive)
    {
        var shown = SnitchRules.RevealedTo(Stage, snitchAlive, IsImpostorPlayer(local), !local.Data.IsDead);
        if (!shown)
        {
            _snitchArrow?.Destroy();
            _snitchArrow = null;
            return;
        }

        _snitchArrow ??= TargetArrow.Create(local.transform, EclipseColors.Snitch);
        _snitchArrow.PointAt(Player.transform.position);
    }

    [HideFromIl2Cpp]
    private void Announce(SnitchStage stage)
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null)
        {
            return;
        }

        string? key = null;
        if (Player.AmOwner)
        {
            key = stage == SnitchStage.CompletedTasks ? "Eclipse.Role.Snitch.SelfCompleted" : "Eclipse.Role.Snitch.SelfRevealed";
        }
        else if (IsImpostorPlayer(local) && !local.Data.IsDead)
        {
            key = stage == SnitchStage.CompletedTasks ? "Eclipse.Role.Snitch.ImpostorCompleted" : "Eclipse.Role.Snitch.ImpostorRevealed";
        }

        if (key == null)
        {
            return;
        }

        ScreenFlash.Flash(EclipseColors.Snitch, 1f, 0.5f);
        Helpers.CreateAndShowNotification($"<b>{MiraLocaleManager.Get(key)}</b>", EclipseColors.Snitch);
    }

    private void ClearArrows()
    {
        foreach (var arrow in _impostorArrows.Values)
        {
            arrow.Destroy();
        }

        _impostorArrows.Clear();
        _snitchArrow?.Destroy();
        _snitchArrow = null;
    }

    private static bool IsImpostorPlayer(PlayerControl player)
    {
        return player.Data?.Role != null && RoleClassification.Classify(player.Data.Role).Faction == Faction.Impostor;
    }
}
