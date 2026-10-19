namespace Eclipse.Freeplay;

internal static class FreeplayState
{
    public static bool IsActive => TutorialManager.InstanceExists && PlayerControl.LocalPlayer != null;
}
