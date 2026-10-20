using Eclipse.Roles.Crewmate;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using UnityEngine;

namespace Eclipse.Networking;

internal static class OfficerRpcs
{

    [MethodRpc((uint)EclipseRpc.OfficerSync, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcOfficerSync(PlayerControl officer, int roundsBeforeReset, int bulletsLeft, int loadedBullets)
    {
        if (LobbyBehaviour.Instance || officer == null || officer.Data?.Role is not OfficerRole role)
        {
            return;
        }

        role.RoundsBeforeReset = Mathf.Max(0, roundsBeforeReset);
        role.BulletsLeft = Mathf.Max(0, bulletsLeft);
        role.LoadedBullets = Mathf.Max(0, loadedBullets);
    }
}
