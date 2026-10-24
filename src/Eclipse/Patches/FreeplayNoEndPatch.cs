using Eclipse.Game;
using HarmonyLib;

namespace Eclipse.Patches;

[HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
internal static class FreeplayNoEndPatch
{
    [HarmonyPrefix]
    public static bool Prefix()
    {
        return !MultiplayerFreeplay.PreventsGameEnd;
    }
}
