using Eclipse.Game;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace Eclipse.Networking;

internal static class SixthSenseRpcs
{
    [MethodRpc((uint)EclipseRpc.SixthSense, LocalHandling = RpcLocalHandling.None)]
    public static void RpcWarn(PlayerControl attacker, PlayerControl target)
    {
        if (attacker == null || target == null || LobbyBehaviour.Instance != null)
        {
            return;
        }

        SixthSenseEvents.Warn(target);
    }
}
