using Eclipse.Game;
using HarmonyLib;
using UnityEngine;

namespace Eclipse.Patches;

[HarmonyPatch]
internal static class NameDecorationPatch
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    [HarmonyPostfix]
    public static void WorldPostfix(PlayerControl __instance)
    {
        FaultGuard.Run("Name decorations (world)", () => NameDecorations.UpdateInWorld(__instance));
        FaultGuard.Run("Name colors (world)", () => NameColors.UpdateInWorld(__instance));

        if (__instance.AmOwner)
        {
            LocalTick.Run(Time.fixedDeltaTime);
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    [HarmonyPostfix]
    public static void MeetingPostfix(MeetingHud __instance)
    {
        FaultGuard.Run("Meeting hint", () => MeetingHints.Update(__instance));

        if (__instance.playerStates == null || !GameData.Instance)
        {
            return;
        }

        foreach (var area in __instance.playerStates)
        {
            var player = GameData.Instance.GetPlayerById(area.PlayerId)?.Object;
            if (player != null)
            {
                FaultGuard.Run("Name decorations (meeting)", () => NameDecorations.UpdateInMeeting(player, area));
                FaultGuard.Run("Name colors (meeting)", () => NameColors.UpdateInMeeting(player, area.NameText));
            }
        }
    }
}
