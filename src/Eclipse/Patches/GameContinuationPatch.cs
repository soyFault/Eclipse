using Eclipse.Game;
using HarmonyLib;
using MiraAPI.GameModes;

namespace Eclipse.Patches;

[HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.IsGameOverDueToDeath))]
internal static class GameContinuationPatch
{
    [HarmonyPostfix]
    public static void Postfix(ref bool __result)
    {
        if (!__result || TutorialManager.InstanceExists || !CustomGameModeManager.IsClassic())
        {
            return;
        }

        var state = GameContinuationState.Evaluate();
        if (state.Held)
        {
            GameContinuationState.LogIfChanged(state);
            __result = false;
        }
    }
}
