using Eclipse.Game;
using HarmonyLib;

namespace Eclipse.Patches;

[HarmonyPatch]
internal static class AutoRejoinPatches
{
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    [HarmonyPostfix]
    public static void EndGameStartPostfix(EndGameManager __instance)
    {
        FaultGuard.Run("AutoRejoin begin", () => AutoRejoin.Begin(__instance));
    }

    [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
    [HarmonyPostfix]
    public static void LobbyStartPostfix()
    {
        AutoRejoin.Cancel();
    }

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.ExitGame))]
    [HarmonyPrefix]
    public static void ExitGamePrefix()
    {
        AutoRejoin.Cancel();
    }
}
