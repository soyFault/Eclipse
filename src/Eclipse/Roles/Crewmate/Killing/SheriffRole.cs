// Portions derived from earlier GPLv3 work; see ATTRIBUTIONS.md.
using Eclipse.Api.Alignments;
using System.Text;
using Eclipse.Api.Roles;
using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Options.Roles.Crewmate;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Translation;
using UnityEngine;

namespace Eclipse.Roles.Crewmate.Killing;

internal sealed class SheriffRole : CrewmateRole, IEclipseRole, IPreventsGameEnd, IDoomable
{

    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Sheriff";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Relentless;

    public Color RoleColor => EclipseColors.Sheriff;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewKilling;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };

    public bool HasMisfired { get; set; }

    public bool PreventsGameEnd => !HasMisfired;

    public override bool IsAffectedByComms => false;

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var text = CustomRoleUtils.CreateForRole(this);
        var hint = HasMisfired
            ? "Eclipse.Role.Sheriff.TabMisfired"
            : $"Eclipse.Role.Sheriff.TabMisfireKills{OptionGroupSingleton<SheriffOptions>.Instance.Misfire}";
        text.AppendLine($"<b>{MiraLocaleManager.Get(hint)}</b>");
        return text;
    }
}
