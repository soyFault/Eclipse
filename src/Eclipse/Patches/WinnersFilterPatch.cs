using Eclipse.Game;
using Eclipse.Roles;
using HarmonyLib;

namespace Eclipse.Patches;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
internal static class WinnersFilterPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(Priority.Last)]
    public static void Postfix()
    {
        var winners = EndGameResult.CachedWinners;
        if (winners == null || RoleManager.Instance == null)
        {
            return;
        }
        for (var i = winners.Count - 1; i >= 0; i--)
        {
            var role = RoleManager.Instance.GetRole(winners[i].RoleWhenAlive);
            if (role is IWinsAlone)
            {
                winners.RemoveAt(i);
            }
        }

        NeutralWinners.AddEarlyWinnersTo(winners);
    }
}
