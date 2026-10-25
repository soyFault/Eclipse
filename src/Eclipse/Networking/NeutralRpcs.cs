using Eclipse.Core;
using Eclipse.Game;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace Eclipse.Networking;

internal static class NeutralRpcs
{
    // The host tells every game who the target of a holder is.
    [MethodRpc((uint)EclipseRpc.NeutralTarget, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcSetTarget(PlayerControl holder, byte kind, PlayerControl target)
    {
        if (LobbyBehaviour.Instance || holder == null || target == null || kind > (byte)TargetKind.Fairy)
        {
            return;
        }

        Targets.Ledger.Set((TargetKind)kind, holder.PlayerId, target.PlayerId);
    }

    // The host tells every game that a holder has nobody to have as a target: they turn into what the host's setting says.
    [MethodRpc((uint)EclipseRpc.NeutralBecome, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcBecome(PlayerControl holder, byte kind, byte option)
    {
        if (LobbyBehaviour.Instance || holder == null || kind > (byte)TargetKind.Fairy || option > (byte)BecomeOption.Jester)
        {
            return;
        }

        if (holder.Data == null || holder.Data.IsDead)
        {
            return;
        }

        Targets.Ledger.Clear((TargetKind)kind, holder.PlayerId);
        BecomeRoles.Become(holder, (BecomeOption)option);
    }
}
