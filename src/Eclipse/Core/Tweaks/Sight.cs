using UnityEngine;

namespace Eclipse.Game;

internal static class Sight
{
    public static bool InSight(PlayerControl observer, PlayerControl other)
    {
        var from = observer.GetTruePosition();
        var vector = other.GetTruePosition() - from;
        var distance = vector.magnitude;
        return distance < PlayerControl.LocalPlayer.lightSource.viewDistance
               && !PhysicsHelpers.AnyNonTriggersBetween(from, vector.normalized, distance, Constants.ShipAndObjectsMask);
    }

    public static bool InSameRoom(Vector2 a, Vector2 b)
    {
        var ship = ShipStatus.Instance;
        if (ship == null)
        {
            return false;
        }

        foreach (var room in ship.FastRooms.Values)
        {
            if (room.roomArea != null && room.roomArea.OverlapPoint(a) && room.roomArea.OverlapPoint(b))
            {
                return true;
            }
        }

        return false;
    }
}
