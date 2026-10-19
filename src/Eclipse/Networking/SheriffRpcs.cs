
using Eclipse.Roles.Crewmate.Killing;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;

namespace Eclipse.Networking;

internal static class SheriffRpcs
{

    [MethodRpc((uint)EclipseRpc.SheriffMisfired, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcSheriffMisfired(PlayerControl sheriff)
    {
        if (sheriff != null && sheriff.Data?.Role is SheriffRole role)
        {
            role.HasMisfired = true;
            Logger<EclipsePlugin>.Info($"Sheriff misfire synced for player {sheriff.PlayerId}.");
        }
    }
}
