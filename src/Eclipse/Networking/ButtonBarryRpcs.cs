using Reactor.Networking.Attributes;

namespace Eclipse.Networking;

internal static class ButtonBarryRpcs
{
    [MethodRpc((uint)EclipseRpc.ButtonBarry)]
    public static void RpcCallMeeting(PlayerControl caller)
    {
        if (!AmongUsClient.Instance.AmHost || caller == null || caller.Data == null || caller.Data.IsDead || MeetingHud.Instance != null)
        {
            return;
        }

        MeetingRoomManager.Instance.AssignSelf(caller, null);
        if (GameManager.Instance.CheckTaskCompletion())
        {
            return;
        }

        HudManager.Instance.OpenMeetingRoom(caller);
        caller.RpcStartMeeting(null);
    }
}
