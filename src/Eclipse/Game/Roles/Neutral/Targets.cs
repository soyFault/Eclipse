using Eclipse.Core;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Player;

namespace Eclipse.Game;

internal static class Targets
{
    public static TargetLedger Ledger { get; } = new();

    public static PlayerControl? TargetOf(TargetKind kind, byte holderId)
    {
        return Ledger.TargetOf(kind, holderId) is { } id && GameData.Instance ? GameData.Instance.GetPlayerById(id)?.Object : null;
    }

    public static PlayerControl? TargetOf(TargetKind kind, PlayerControl holder)
    {
        return TargetOf(kind, holder.PlayerId);
    }

    public static void Reset()
    {
        Ledger.Clear();
    }

    [RegisterEvent]
    public static void OnPlayerLeave(PlayerLeaveEvent leave)
    {
        if (leave.ClientData.Character is { } leaver)
        {
            Ledger.ClearHolder(leaver.PlayerId);
        }
    }
}
