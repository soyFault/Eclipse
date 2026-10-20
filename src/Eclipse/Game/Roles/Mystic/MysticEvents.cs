using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;

namespace Eclipse.Game;

internal static class MysticEvents
{
    [RegisterEvent]
    public static void OnAfterMurder(AfterMurderEvent murder)
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null || local.Data == null)
        {
            return;
        }

        var alerted = MysticRules.IsAlerted(
            MeetingHud.Instance != null,
            local.Data.Role is MysticRole,
            local.Data.IsDead,
            murder.Target.AmOwner);
        if (!alerted)
        {
            return;
        }

        ScreenFlash.Flash(Palette.CrewmateRoleBlue);

        var duration = OptionGroupSingleton<MysticOptions>.Instance.ArrowDuration;
        if (duration > 0f)
        {
            TargetArrow.Blip(local.transform, EclipseColors.Mystic, murder.Target.GetTruePosition(), duration);
        }
    }
}
