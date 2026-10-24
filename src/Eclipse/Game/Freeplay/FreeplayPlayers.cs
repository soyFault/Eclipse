using Eclipse.Core;
using MiraAPI.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal static class FreeplayPlayers
{
    public static void Revive(PlayerControl player)
    {
        if (!player.Data.IsDead)
        {
            return;
        }

        var body = Helpers.GetBodyById(player.PlayerId);
        if (body != null)
        {
            Object.Destroy(body.gameObject);
        }

        KillLogEvents.Log.Clear();
        player.Revive();
    }

    public static void ForgetRound()
    {
        Protections.Reset();
        Mirrors.Reset();
        KillLogEvents.Log.Clear();
        foreach (var body in Object.FindObjectsOfType<DeadBody>())
        {
            Object.Destroy(body.gameObject);
        }
    }
}
