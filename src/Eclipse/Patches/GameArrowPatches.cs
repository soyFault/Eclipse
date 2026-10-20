using HarmonyLib;

namespace Eclipse.Patches;

[HarmonyPatch(typeof(ArrowBehaviour), nameof(ArrowBehaviour.Awake))]
internal static class ArrowBehaviourPatch
{
    [HarmonyPostfix]
    public static void Postfix(ArrowBehaviour __instance)
    {
        Game.GameArrows.Swap(__instance.image, "arrow", "Arrow");
    }
}

[HarmonyPatch(typeof(Vent), nameof(Vent.SetButtons))]
internal static class VentArrowsPatch
{
    [HarmonyPostfix]
    public static void Postfix(Vent __instance)
    {
        foreach (var button in __instance.Buttons)
        {
            if (button != null)
            {
                Game.GameArrows.Swap(button.GetComponent<UnityEngine.SpriteRenderer>(), "vent", "VentArrow");
            }
        }
    }
}
