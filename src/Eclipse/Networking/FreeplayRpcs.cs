using AmongUs.GameOptions;
using Eclipse.Core;
using Eclipse.Game;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using UnityEngine;

namespace Eclipse.Networking;

internal static class FreeplayRpcs
{
    [MethodRpc((uint)EclipseRpc.FreeplayRequest, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcFreeplayRequest(PlayerControl sender, byte action, byte target, byte other, uint data)
    {
        if (sender == null || !MultiplayerFreeplayRules.IsKnownAction(action))
        {
            return;
        }

        MultiplayerFreeplay.Handle(sender, (FreeplayAction)action, target, other, data);
    }

    [MethodRpc((uint)EclipseRpc.FreeplayRole, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcApplyRole(PlayerControl target, ushort roleType)
    {
        if (target == null || target.Data == null || !MultiplayerFreeplay.Enabled)
        {
            return;
        }

        RoleChanger.ChangeRole(target, (RoleTypes)roleType);
    }

    [MethodRpc((uint)EclipseRpc.FreeplayRevive, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcRevive(PlayerControl target)
    {
        if (target == null || target.Data == null || !MultiplayerFreeplay.Enabled)
        {
            return;
        }

        FreeplayPlayers.Revive(target);
    }

    [MethodRpc((uint)EclipseRpc.FreeplayResetState, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcResetState(PlayerControl sender)
    {
        if (sender == null || !MultiplayerFreeplay.Enabled)
        {
            return;
        }

        FreeplayPlayers.ForgetRound();
    }

    [MethodRpc((uint)EclipseRpc.FreeplayRestore, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcRestorePlayer(PlayerControl target, ushort roleType, float x, float y)
    {
        if (target == null || target.Data == null || !MultiplayerFreeplay.Enabled)
        {
            return;
        }

        FreeplayPlayers.Revive(target);
        RoleChanger.ChangeRole(target, (RoleTypes)roleType);
        var position = new Vector2(x, y);
        target.transform.position = position;
        target.NetTransform.SnapTo(position);
    }
}
