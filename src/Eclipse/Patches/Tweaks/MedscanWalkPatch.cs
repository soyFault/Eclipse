using System.Reflection;
using Eclipse.Options.Host;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;

namespace Eclipse.Patches;

internal static class MedscanWalkPatch
{
    [HarmonyPatch]
    internal static class WalkToPad
    {
        public static MethodBase TargetMethod()
        {
            return Helpers.GetStateMachineMoveNext<MedScanMinigame>(nameof(MedScanMinigame.WalkToPad))!;
        }

        public static bool Prefix(Il2CppObjectBase __instance)
        {
            return SkipWalking(__instance);
        }
    }

    [HarmonyPatch]
    internal static class WalkToOffset
    {
        public static MethodBase TargetMethod()
        {
            return Helpers.GetStateMachineMoveNext<MedScanMinigame>(nameof(MedScanMinigame.WalkToOffset))!;
        }

        public static bool Prefix(Il2CppObjectBase __instance)
        {
            return SkipWalking(__instance);
        }
    }

    private static bool SkipWalking(Il2CppObjectBase machine)
    {
        if (OptionGroupSingleton<VanillaTweakOptions>.Instance.MedscanWalk)
        {
            return true;
        }

        var minigame = new StateMachineWrapper<MedScanMinigame>(machine).Instance;
        switch (minigame.state)
        {
            case MedScanMinigame.PositionState.None:
                minigame.state = MedScanMinigame.PositionState.WalkingToPad;
                break;
            case MedScanMinigame.PositionState.WalkingToPad:
                minigame.state = MedScanMinigame.PositionState.WalkingToOffset;
                break;
            case MedScanMinigame.PositionState.WalkingToOffset:
                minigame.state = (MedScanMinigame.PositionState)(-1);
                minigame.walking = null;
                break;
        }

        return false;
    }
}
