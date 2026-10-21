using Eclipse.Game;
using Eclipse.Roles.Crewmate.Killing;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace Eclipse.Networking;

internal static class VeteranRpcs
{
    [MethodRpc((uint)EclipseRpc.VeteranAlert, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcVeteranAlert(PlayerControl attacker, PlayerControl veteran, bool stopped, bool countsAsAttack)
    {
        if (LobbyBehaviour.Instance || attacker == null || veteran == null)
        {
            return;
        }

        if (countsAsAttack && veteran.Data?.Role is VeteranRole role)
        {
            role.AttackedRecently = true;
        }

        if (stopped && attacker.AmOwner)
        {
            ShieldGuard.ResetStoppedAttacker(attacker);
        }
    }
}
