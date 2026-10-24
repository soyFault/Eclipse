using Eclipse.Options.Host;
using HarmonyLib;
using MiraAPI.GameOptions;

namespace Eclipse.Patches;

[HarmonyPatch(typeof(MedScanMinigame), nameof(MedScanMinigame.FixedUpdate))]
internal static class ParallelMedbayPatch
{
    [HarmonyPrefix]
    public static void Prefix(MedScanMinigame __instance)
    {
        if (OptionGroupSingleton<VanillaTweakOptions>.Instance.ParallelMedbay)
        {
            __instance.medscan.CurrentUser = PlayerControl.LocalPlayer.PlayerId;
            __instance.medscan.UsersList.Clear();
        }
    }
}
