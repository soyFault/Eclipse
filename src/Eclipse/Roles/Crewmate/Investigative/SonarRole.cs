using System.Collections.Generic;
using System.Text;
using Eclipse.Api.Alignments;
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
using UnityEngine;

namespace Eclipse.Roles.Crewmate.Investigative;

internal sealed class SonarRole : CrewmateRole, IEclipseRole, IDoomable
{
    private readonly List<Track> _tracks = [];

    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Sonar";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Hunter;

    public Color RoleColor => EclipseColors.Sonar;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewInvestigative;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };

    public override bool IsAffectedByComms => false;

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        Clear();
    }

    public override void OnDeath(DeathReason reason)
    {
        RoleBehaviourStubs.OnDeath(this, reason);
        Clear();
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        Clear();
    }

    [HideFromIl2Cpp]
    public bool IsTracking(PlayerControl player)
    {
        return _tracks.Exists(t => t.Player == player);
    }

    [HideFromIl2Cpp]
    public void StartTracking(PlayerControl player)
    {
        if (IsTracking(player))
        {
            return;
        }

        var color = Palette.PlayerColors[player.Data.DefaultOutfit.ColorId];
        var arrow = TargetArrow.Create(Player.transform, color);
        arrow.PointAt(player.transform.position);
        _tracks.Add(new Track(player, arrow));
    }

    [HideFromIl2Cpp]
    public void Tick(float deltaTime)
    {
        var interval = OptionGroupSingleton<SonarOptions>.Instance.UpdateInterval;
        for (var i = _tracks.Count - 1; i >= 0; i--)
        {
            var track = _tracks[i];
            var player = track.Player;
            if (TrackRules.ShouldDrop(player != null, player != null && player.Data.IsDead, player != null && player.Data.Disconnected))
            {
                track.Arrow.Destroy();
                _tracks.RemoveAt(i);
                continue;
            }

            track.SinceUpdate += deltaTime;
            if (TrackRules.UpdateDue(track.SinceUpdate, interval))
            {
                track.SinceUpdate = 0f;
                track.Arrow.PointAt(player!.transform.position);
            }
        }
    }

    [HideFromIl2Cpp]
    public void Clear()
    {
        foreach (var track in _tracks)
        {
            track.Arrow.Destroy();
        }

        _tracks.Clear();
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var text = CustomRoleUtils.CreateForRole(this);
        if (_tracks.Count > 0)
        {
            text.AppendLine($"<b>{MiraLocaleManager.Get("Eclipse.Role.Sonar.TabTracked")}</b>");
            foreach (var track in _tracks)
            {
                text.AppendLine(track.Player.Data.PlayerName);
            }
        }

        return text;
    }

    private sealed class Track(PlayerControl player, TargetArrow arrow)
    {
        public PlayerControl Player { get; } = player;

        public TargetArrow Arrow { get; } = arrow;

        public float SinceUpdate { get; set; }
    }
}
