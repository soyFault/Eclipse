using Eclipse.Game;

namespace Eclipse.Freeplay;

internal static class FreeplayState
{
    public static bool IsActive => PlayerControl.LocalPlayer != null && (TutorialManager.InstanceExists || MultiplayerFreeplay.LocalMayUse);

    public static bool IsMultiplayer => !TutorialManager.InstanceExists && MultiplayerFreeplay.Enabled;
}
