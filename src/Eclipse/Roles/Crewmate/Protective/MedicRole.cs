using System.Text;
using Eclipse.Api.Alignments;
using Eclipse.Api.Roles;
using Eclipse.Assets;
using Eclipse.Core;
using Eclipse.Colors;
using Eclipse.Modifiers;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Translation;
using UnityEngine;

namespace Eclipse.Roles.Crewmate.Protective;

internal sealed class MedicRole : CrewmateRole, IEclipseRole, IDoomable
{
    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Medic";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Protective;

    public Color RoleColor => EclipseColors.Medic;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewProtective;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };

    [HideFromIl2Cpp]
    public PlayerControl? Shielded { get; set; }

    public bool CanChangeTarget { get; set; } = true;

    [HideFromIl2Cpp]
    public void SetShieldedPlayer(PlayerControl? target)
    {
        if (Shielded != null && Shielded.TryGetModifier<MedicShieldModifier>(out var old))
        {
            old.RemoveMedic(Player);
        }

        Shielded = target;
        if (target == null)
        {
            return;
        }

        if (target.TryGetModifier<MedicShieldModifier>(out var shield))
        {
            shield.AddMedic(Player);
        }
        else
        {
            target.AddModifier<MedicShieldModifier>(Player);
        }
    }

    public void Clear()
    {
        SetShieldedPlayer(null);
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
    public StringBuilder SetTabText()
    {
        var text = CustomRoleUtils.CreateForRole(this);
        if (Shielded != null && Shielded.Data != null)
        {
            var line = MiraLocaleManager.Get("Eclipse.Role.Medic.TabProtecting").Replace("<player>", Shielded.Data.PlayerName);
            text.AppendLine($"\n<b>{line}</b>");
        }

        return text;
    }

    public override bool IsAffectedByComms => false;
}
