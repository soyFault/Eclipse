using Eclipse.Game;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace Eclipse.Networking;

internal static class FirstRoundRpcs
{
    [MethodRpc((uint)EclipseRpc.FirstRoundShield, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcFirstRoundShield(PlayerControl recipient)
    {
        if (recipient == null || recipient.Data == null)
        {
            return;
        }

        FirstRoundShield.Give(recipient);
    }
}
