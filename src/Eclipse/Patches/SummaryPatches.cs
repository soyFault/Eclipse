using Eclipse.Game;
using HarmonyLib;

namespace Eclipse.Patches;

[HarmonyPatch]
internal static class SummaryPatches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    [HarmonyPostfix]
    public static void GameEndPostfix()
    {
        FaultGuard.Run("Match summary capture", SummaryEvents.Capture);
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    [HarmonyPostfix]
    public static void EndScreenPostfix(EndGameManager __instance)
    {
        FaultGuard.Run("Match summary", () => SummaryEvents.Show(__instance));
    }
}
