using Eclipse.Game;
using Eclipse.Roles;
using Eclipse.Roles.Crewmate.Investigative;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace Eclipse.Networking;

internal static class AurialRpcs
{

    [MethodRpc((uint)EclipseRpc.AurialSense, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcAurialSense(PlayerControl aurial, PlayerControl source)
    {
        if (LobbyBehaviour.Instance || aurial == null || source == null || aurial.Data?.Role is not AurialRole)
        {
            return;
        }

        AurialEvents.Show(aurial, source); // lo reciben todos; Show solo actúa en el PC del Aurial
    }
}
