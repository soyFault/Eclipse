using Eclipse.Game;
using HarmonyLib;
using MiraAPI.GameModes;
using Reactor.Utilities;

namespace Eclipse.Patches;

[HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
internal static class NeutralWinPatch
{
    [HarmonyPrefix]
    public static bool Prefix()
    {
        if (!AmongUsClient.Instance.AmHost || !GameData.Instance || !NeutralWinners.Board.HasPending)
        {
            return true;
        }

        if (!TutorialManager.InstanceExists && !CustomGameModeManager.IsClassic())
        {
            return true;
        }

        if (!ExileController.Instance)
        {
            Logger<EclipsePlugin>.Info("Neutral win: ending the match now that the exile screen is closed.");
            NeutralWinners.EndMatch();
        }

        return false;
    }
}
