using Eclipse.Core;
using Eclipse.Options.Host;
using HarmonyLib;
using MiraAPI.GameOptions;
using UnityEngine;

namespace Eclipse.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
internal static class KillCooldownPatch
{
    [HarmonyPrefix]
    public static void Prefix(PlayerControl __instance)
    {
        if (!__instance.AmOwner || __instance.Data?.Role == null || !__instance.Data.Role.CanUseKillButton)
        {
            return;
        }

        if (KillCooldownRules.Pauses(OptionGroupSingleton<VanillaTweakOptions>.Instance.TickCooldownsInMinigame, Minigame.Instance != null))
        {
            __instance.killTimer += Time.fixedDeltaTime;
        }
    }
}
