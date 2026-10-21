using Eclipse.Buttons;
using Eclipse.Buttons.Crewmate;
using Eclipse.Core;
using Eclipse.Modifiers;
using Eclipse.Networking;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Killing;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Networking;

namespace Eclipse.Game;

internal static class VeteranEvents
{
    // Finishing a task gives the Veteran one more alert. Every game keeps the count; only the Veteran's own game updates the button.
    [RegisterEvent]
    public static void OnCompleteTask(CompleteTaskEvent task)
    {
        if (task.Player.Data?.Role is not VeteranRole veteran || !OptionGroupSingleton<VeteranOptions>.Instance.TaskUses)
        {
            return;
        }

        veteran.Alerts++;
        if (task.Player.AmOwner)
        {
            var button = CustomButtonSingleton<VeteranAlertButton>.Instance;
            button.SetUses(button.UsesLeft + 1);
        }
    }

    // A click on a button that targets the alerting Veteran: stopped, and whoever clicked dies. It is decided on the clicker's own game. Priority 1:
    // after the Medic's shield (-900) and the protections (-800), so an interaction they already stopped is not answered a second time.
    [RegisterEvent(1)]
    public static void OnButtonClick(MiraButtonClickEvent click)
    {
        if (click.IsCancelled || click.Button is not CustomActionButton<PlayerControl> button)
        {
            return;
        }

        var target = button.Target;
        if (target == null || !button.CanClick())
        {
            return;
        }

        Respond(click, PlayerControl.LocalPlayer, target, button is IKillButton, false, button is IIndirectKillButton, true, button);
    }

    // Any other attack on the alerting Veteran (a vanilla Impostor kill, a custom murder from any mod): stopped, and the attacker dies. Only the
    // HOST answers, so it happens once; for a vanilla kill the host is the only game that sees it at all.
    [RegisterEvent(1)]
    public static void OnBeforeMurder(BeforeMurderEvent murder)
    {
        if (murder.IsCancelled)
        {
            return;
        }

        Respond(murder, murder.Source, murder.Target, true, murder.IgnoreDefense, murder.IsIndirectAttack, AmongUsClient.Instance.AmHost, null);
    }

    private static void Respond(
        MiraCancelableEvent miraEvent,
        PlayerControl source,
        PlayerControl target,
        bool isAttack,
        bool ignoresDefense,
        bool isIndirect,
        bool answersHere,
        CustomActionButton? clickedButton)
    {
        var inMeeting = MeetingHud.Instance || ExileController.Instance;
        var response = VeteranAlertRules.Decide(
            target.HasModifier<VeteranAlertModifier>(),
            source.PlayerId == target.PlayerId,
            inMeeting,
            source.HasModifier<VeteranAlertModifier>(),
            OptionGroupSingleton<VeteranOptions>.Instance.KilledOnAlert,
            ignoresDefense,
            isIndirect,
            isAttack);

        if (response.CancelsInteraction)
        {
            miraEvent.Cancel();
        }

        if (!answersHere)
        {
            return;
        }

        // A clicked button gets the short cooldown here (it also stops a very fast second click); a stopped attack is reset by the message below.
        if (response.CancelsInteraction)
        {
            clickedButton?.SetTimer(ShieldGuard.BlockedCooldown);
        }

        if (response.CancelsInteraction || response.CountsAsAttack)
        {
            VeteranRpcs.RpcVeteranAlert(source, target, response.CancelsInteraction && clickedButton == null, response.CountsAsAttack && target.Data?.Role is VeteranRole);
        }

        if (response.KillsInteractor)
        {
            target.RpcCustomMurder(source, MeetingCheck.OutsideMeeting);
        }
    }
}
