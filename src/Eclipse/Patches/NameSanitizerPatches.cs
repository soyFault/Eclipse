using Eclipse.Core;
using HarmonyLib;

namespace Eclipse.Patches;

[HarmonyPatch]
internal static class NameSanitizerPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetName))]
    [HarmonyPrefix]
    public static void SetNamePrefix(ref string playerName)
    {
        playerName = NameSanitizer.Clean(playerName);
    }

    [HarmonyPatch(typeof(NetworkedPlayerInfo.PlayerOutfit), nameof(NetworkedPlayerInfo.PlayerOutfit.Deserialize))]
    [HarmonyPostfix]
    public static void OutfitDeserializePostfix(NetworkedPlayerInfo.PlayerOutfit __instance)
    {
        __instance.PlayerName = NameSanitizer.Clean(__instance.PlayerName);
    }
}
