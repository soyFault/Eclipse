using System.Text;
using Eclipse.Api.Alignments;
using Eclipse.Api.Roles;
using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Options.Roles.Crewmate;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Translation;
using UnityEngine;

namespace Eclipse.Roles.Crewmate;

internal sealed class OfficerRole : CrewmateRole, IEclipseRole, IPreventsGameEnd, IDoomable
{

    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Officer";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Fearmonger;

    public Color RoleColor => EclipseColors.Officer;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewKilling;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };

    public int BulletsLeft { get; set; }

    public int LoadedBullets { get; set; }

    public int RoundsBeforeReset { get; set; }

    public float LastLoadTime { get; set; } = float.NegativeInfinity;

    public bool GunConfiscated => RoundsBeforeReset > 0;
    public bool PreventsGameEnd => OfficerRules.PreventsGameEnd(BulletsLeft, LoadedBullets);

    public override bool IsAffectedByComms => false;
    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        BulletsLeft = (int)OptionGroupSingleton<OfficerOptions>.Instance.MaxBulletsTotal;
        LoadedBullets = 0;
        RoundsBeforeReset = 0;
        LastLoadTime = float.NegativeInfinity;
    }
    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var options = OptionGroupSingleton<OfficerOptions>.Instance;
        var text = CustomRoleUtils.CreateForRole(this);
        var hint = OfficerRules.TabHint(options.CanOnlyShootActiveKillers, options.NonKillingNeutralsAreInnocent) switch
        {
            OfficerHint.KillBased => "Eclipse.Role.Officer.TabKillBased",
            OfficerHint.MajorityInnocent => "Eclipse.Role.Officer.TabMajorityInnocent",
            _ => "Eclipse.Role.Officer.TabCrewmatesInnocent",
        };
        text.AppendLine($"<b>{MiraLocaleManager.Get(hint)}</b>");
        if (RoundsBeforeReset > 0)
        {
            var wait = RoundsBeforeReset == 1
                ? MiraLocaleManager.Get("Eclipse.Role.Officer.TabWaitNext")
                : MiraLocaleManager.Get("Eclipse.Role.Officer.TabWaitCount").Replace("<count>", RoundsBeforeReset.ToString(System.Globalization.CultureInfo.InvariantCulture));
            text.AppendLine($"\n<b>{wait}</b>");
        }

        return text;
    }
}
