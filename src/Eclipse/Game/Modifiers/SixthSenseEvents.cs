using Eclipse.Modifiers;
using Eclipse.Networking;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using UnityEngine;

namespace Eclipse.Game;

internal static class SixthSenseEvents
{
    private static readonly Color Flash = new Color32(0, 255, 153, 255);

    [RegisterEvent]
    public static void OnButtonClick(MiraButtonClickEvent click)
    {
        if (click.IsCancelled || click.Button is not CustomActionButton<PlayerControl> button)
        {
            return;
        }

        var target = button.Target;
        if (target != null && button.CanClick() && target.HasModifier<SixthSenseModifier>() && !MeetingHud.Instance && !ExileController.Instance)
        {
            SixthSenseRpcs.RpcWarn(PlayerControl.LocalPlayer, target);
        }
    }

    // Runs on every client; only the target flashes.
    public static void Warn(PlayerControl target)
    {
        if (target.AmOwner && target.HasModifier<SixthSenseModifier>())
        {
            ScreenFlash.Flash(Flash);
        }
    }
}
