using Eclipse.Game;
using HarmonyLib;

namespace Eclipse.Patches;

// The lobby screen loads every time the players come back from a match (or the first time they open the room): a good moment to forget the old match.
[HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
internal static class MatchResetPatch
{
    [HarmonyPostfix]
    public static void Postfix()
    {
        MatchReset.Run();
    }
}
