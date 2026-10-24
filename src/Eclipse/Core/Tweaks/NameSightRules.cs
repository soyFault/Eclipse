namespace Eclipse.Core;

internal enum NameSight
{
    Basic = 0,

    RoomBased = 1,

    Harsh = 2,
}

internal static class NameSightRules
{
    public static bool NameVisible(NameSight mode, bool localDead, bool isSelf, bool inSight, bool sameRoom)
    {
        if (mode == NameSight.Basic || localDead || isSelf)
        {
            return true;
        }

        return inSight || (mode == NameSight.RoomBased && sameRoom);
    }
}
