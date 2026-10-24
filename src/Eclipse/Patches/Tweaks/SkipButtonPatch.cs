using Eclipse.Core;
using Eclipse.Options.Host;
using HarmonyLib;
using MiraAPI.GameOptions;

namespace Eclipse.Patches;

[HarmonyPatch]
internal static class SkipButtonPatch
{
    private static bool _emergencyMeeting;

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.StartMeeting))]
    [HarmonyPrefix]
    public static void StartMeetingPrefix(NetworkedPlayerInfo target)
    {
        _emergencyMeeting = target == null;
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix(MeetingHud __instance)
    {
        var state = OptionGroupSingleton<VanillaTweakOptions>.Instance.SkipButtonDisable;
        if (state != SkipState.No && SkipButtonRules.Hides(state, _emergencyMeeting) && __instance.SkipVoteButton.gameObject.activeSelf)
        {
            __instance.SkipVoteButton.gameObject.SetActive(false);
        }
    }
}
