using Eclipse.Modifiers;
using HarmonyLib;
using MiraAPI.Modifiers;

namespace Eclipse.Patches;

[HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
internal static class DrunkPatch
{
    [HarmonyPostfix]
    public static void Postfix(PlayerPhysics __instance)
    {
        var player = __instance.myPlayer;
        if (!__instance.AmOwner || player == null || !player.CanMove || player.Data == null || player.Data.IsDead)
        {
            return;
        }

        if (player.HasModifier<DrunkModifier>())
        {
            __instance.body.velocity *= -1f;
        }
    }
}
