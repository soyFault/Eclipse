using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace Eclipse.Networking;

internal static class ClericRpcs
{
    [MethodRpc((uint)EclipseRpc.ClericBarrier, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcClericBarrier(PlayerControl cleric, PlayerControl target)
    {
        if (LobbyBehaviour.Instance || cleric == null || target == null || cleric.Data?.Role is not ClericRole)
        {
            return;
        }

        var expiresAt = Protections.Now + OptionGroupSingleton<ClericOptions>.Instance.BarrierDuration;
        Protections.Ledger.Add(ProtectionKind.Barrier, cleric.PlayerId, target.PlayerId, expiresAt);
    }

    [MethodRpc((uint)EclipseRpc.ClericCleanse, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcClericCleanse(PlayerControl cleric, PlayerControl target)
    {
        if (LobbyBehaviour.Instance || cleric == null || target == null || cleric.Data?.Role is not ClericRole)
        {
            return;
        }

        CleanseReports.Apply(cleric, target);
    }
}
