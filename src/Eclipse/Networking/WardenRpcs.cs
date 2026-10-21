using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Roles.Crewmate.Protective;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace Eclipse.Networking;

internal static class WardenRpcs
{
    [MethodRpc((uint)EclipseRpc.WardenFortify, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcWardenFortify(PlayerControl warden, PlayerControl target)
    {
        if (LobbyBehaviour.Instance || warden == null || target == null || warden.Data?.Role is not WardenRole)
        {
            return;
        }

        Protections.Ledger.Set(ProtectionKind.Fortify, warden.PlayerId, target.PlayerId);
    }

    [MethodRpc((uint)EclipseRpc.InteractionBlocked, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcInteractionBlocked(PlayerControl source, byte kind, PlayerControl protector, PlayerControl target, bool wasAttack)
    {
        if (LobbyBehaviour.Instance || source == null || protector == null || target == null || kind > (byte)ProtectionKind.Barrier)
        {
            return;
        }

        Protections.OnBlocked((ProtectionKind)kind, source, protector, wasAttack);
    }
}
