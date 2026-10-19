using UnityEngine;

namespace Eclipse.Api.Factions;

public static class FactionColors
{
    public static Color Crewmate { get; } = new Color32(0x68, 0xAC, 0xF4, 0xFF);

    public static Color Impostor { get; } = new Color32(0xD6, 0x3F, 0x42, 0xFF);

    public static Color Neutral { get; } = new Color32(0x8A, 0x8A, 0x8A, 0xFF);

    public static Color Of(Faction faction)
    {
        return faction switch
        {
            Faction.Crewmate => Crewmate,
            Faction.Impostor => Impostor,
            _ => Neutral,
        };
    }
}
