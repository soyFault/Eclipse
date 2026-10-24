using Eclipse.Game;
using HarmonyLib;

namespace Eclipse.Patches;

[HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
internal static class ChatCommandPatch
{
    [HarmonyPrefix]
    public static bool Prefix(ChatController __instance)
    {
        if (!ChatCommands.TryRun(__instance.freeChatField.Text))
        {
            return true;
        }

        __instance.freeChatField.Clear();
        __instance.quickChatMenu.Clear();
        __instance.quickChatField.Clear();
        __instance.UpdateChatMode();
        return false;
    }
}
