using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Options.Host;
using HarmonyLib;
using MiraAPI.GameOptions;

namespace Eclipse.Patches;

[HarmonyPatch]
internal static class VentAnimationPatch
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.EnterVent))]
    [HarmonyPrefix]
    public static bool EnterVentPrefix(Vent __instance, PlayerControl pc)
    {
        return __instance.EnterVentAnim == null || Plays(pc);
    }

    [HarmonyPatch(typeof(Vent), nameof(Vent.ExitVent))]
    [HarmonyPrefix]
    public static bool ExitVentPrefix(Vent __instance, PlayerControl pc)
    {
        return __instance.ExitVentAnim == null || Plays(pc);
    }

    private static bool Plays(PlayerControl pc)
    {
        var hide = OptionGroupSingleton<VanillaTweakOptions>.Instance.HideVentAnimationNotInVision;
        var local = PlayerControl.LocalPlayer;
        return VentAnimationRules.Plays(hide, !hide || local.Data.IsDead || Sight.InSight(local, pc));
    }
}
