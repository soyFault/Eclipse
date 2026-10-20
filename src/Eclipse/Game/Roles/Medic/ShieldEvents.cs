using Eclipse.Buttons;
using Eclipse.Buttons.Crewmate;
using Eclipse.Modifiers;
using Eclipse.Networking;
using Eclipse.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;

namespace Eclipse.Game;

internal static class ShieldEvents
{
    [RegisterEvent(-900)]
    public static void OnBeforeMurder(BeforeMurderEvent murder)
    {
        if (murder.IsCancelled || !ShieldGuard.Blocks(murder.Source, murder.Target, murder.IgnoreDefense))
        {
            return;
        }

        murder.Cancel();
        if (AmongUsClient.Instance.AmHost && ShieldGuard.MedicOf(murder.Target) is { } medic)
        {
            MedicRpcs.RpcShieldBlocked(murder.Source, medic, murder.Target);
        }
    }

    [RegisterEvent(-900)]
    public static void OnButtonClick(MiraButtonClickEvent click)
    {
        if (click.IsCancelled || click.Button is not IKillButton || click.Button is not CustomActionButton<PlayerControl> button)
        {
            return;
        }

        var attacker = PlayerControl.LocalPlayer;
        var target = button.Target;
        if (target == null || !button.CanClick() || !ShieldGuard.Blocks(attacker, target, false))
        {
            return;
        }

        click.Cancel();
        button.SetTimer(ShieldGuard.BlockedCooldown);
        if (ShieldGuard.MedicOf(target) is { } medic)
        {
            MedicRpcs.RpcShieldBlocked(attacker, medic, target);
        }
    }


    [RegisterEvent]
    public static void OnPlayerDeath(PlayerDeathEvent death)
    {
        foreach (var medic in CustomRoleUtils.GetActiveRolesOfType<MedicRole>())
        {
            if (medic.Shielded == death.Player)
            {
                medic.Clear();
                medic.CanChangeTarget = true;
            }
        }
    }

    [RegisterEvent]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        if (PlayerControl.LocalPlayer?.Data?.Role is MedicRole role)
        {
            role.CanChangeTarget = true;
        }
    }

    [RegisterEvent]
    public static void OnPlayerLeave(PlayerLeaveEvent leave)
    {
        var leaver = leave.ClientData.Character;
        if (leaver == null)
        {
            return;
        }

        foreach (var medic in CustomRoleUtils.GetActiveRolesOfType<MedicRole>())
        {
            if (medic.Player == leaver)
            {
                medic.Clear();
            }
            else if (medic.Shielded == leaver)
            {
                medic.Clear();
                medic.CanChangeTarget = true;
                if (medic.Player.AmOwner)
                {
                    var button = CustomButtonSingleton<MedicShieldButton>.Instance;
                    button.SetUses(button.UsesLeft + 1);
                    button.ResetCooldownAndOrEffect();
                }
            }
        }
    }
}
