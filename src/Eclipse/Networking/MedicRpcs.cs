using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Modifiers;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Networking;

internal static class MedicRpcs
{

    [MethodRpc((uint)EclipseRpc.MedicShield, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcMedicShield(PlayerControl medic, PlayerControl target)
    {
        if (medic != null && target != null && medic.Data?.Role is MedicRole role)
        {
            role.SetShieldedPlayer(target);
        }
    }
    [MethodRpc((uint)EclipseRpc.ShieldBlocked, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcShieldBlocked(PlayerControl attacker, PlayerControl medic, PlayerControl shielded)
    {
        if (attacker == null || medic == null || shielded == null || medic.Data?.Role is not MedicRole role)
        {
            return;
        }

        var options = OptionGroupSingleton<MedicOptions>.Instance;
        var local = PlayerControl.LocalPlayer;

        if (attacker.AmOwner)
        {
            ShieldGuard.ResetStoppedAttacker(attacker);
            Helpers.CreateAndShowNotification(MiraLocaleManager.Get("Eclipse.Role.Medic.AttackBlocked"), EclipseColors.Medic);
        }
        else if (ShieldRules.GetsAttackNotification(options.WhoGetsNotification, local.PlayerId == medic.PlayerId, local.PlayerId == shielded.PlayerId))
        {
            Helpers.CreateAndShowNotification(MiraLocaleManager.Get("Eclipse.Role.Medic.ShieldAbsorbed"), EclipseColors.Medic);
        }

        if (options.ShieldBreaks)
        {
            role.SetShieldedPlayer(null);
        }
        else if (shielded.TryGetModifier<MedicShieldModifier>(out var shield))
        {
            shield.ShiftNextMedic(medic);
        }
    }
}
