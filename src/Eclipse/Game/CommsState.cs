namespace Eclipse.Game;

internal static class CommsState
{
    private static ShipStatus? _ship;
    private static HudOverrideSystemType? _hudSystem;
    private static HqHudSystemType? _hqSystem;

    public static bool IsActive
    {
        get
        {
            var ship = ShipStatus.Instance;
            if (!ship)
            {
                return false;
            }
            if (_ship != ship)
            {
                _ship = ship;
                _hudSystem = null;
                _hqSystem = null;
                if (ship.Systems.TryGetValue(SystemTypes.Comms, out var comms))
                {
                    _hqSystem = comms.TryCast<HqHudSystemType>();
                    _hudSystem = comms.TryCast<HudOverrideSystemType>();
                }
            }

            return (_hudSystem != null && _hudSystem.IsActive) || (_hqSystem != null && _hqSystem.IsActive);
        }
    }
}
