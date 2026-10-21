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
using MiraAPI.Utilities;
using UnityEngine;

namespace Eclipse.Roles.Crewmate.Killing;

internal sealed class VeteranRole : CrewmateRole, IEclipseRole, IPreventsGameEnd, IDoomable
{
    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "Veteran";

    [HideFromIl2Cpp]
    public DoomableType DoomHintType => DoomableType.Trickster;

    public Color RoleColor => EclipseColors.Veteran;

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.CrewKilling;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = EclipseAssets.RoleIcon(IdPart),
        IconTmp = EclipseAssets.RoleIconTmp(IdPart)!,
    };

    // How many alerts are left (one in progress still counts until it ends). Every game keeps the count: the match rules read it everywhere.
    public int Alerts { get; set; }

    public bool AttackedRecently { get; set; }

    public bool PreventsGameEnd => VeteranAlertRules.PreventsGameEnd(Alerts);

    public override bool IsAffectedByComms => false;

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        Alerts = (int)OptionGroupSingleton<VeteranOptions>.Instance.MaxAlerts;
        AttackedRecently = false;
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        return CustomRoleUtils.CreateForRole(this);
    }

    public override void OnMeetingStart()
    {
        RoleBehaviourStubs.OnMeetingStart(this);

        if (!AttackedRecently)
        {
            return;
        }

        AttackedRecently = false;
        var options = OptionGroupSingleton<VeteranOptions>.Instance;
        if (!Player.AmOwner || options.KilledOnAlert || !options.KnowWhenAttacked.Value)
        {
            return;
        }

        var message = MiraLocaleManager.Get("Eclipse.Role.Veteran.AttackedMessage");
        Helpers.CreateAndShowNotification(message, EclipseColors.Veteran);
        ChatReports.Add(Player, MiraLocaleManager.Get("Eclipse.Role.Veteran.WarningTitle"), EclipseColors.Veteran, message);
    }
}
