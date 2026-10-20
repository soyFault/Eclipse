namespace Eclipse.Core;

internal enum AuraSense
{
    None = 0,
    Place = 1,
    PlayerColor = 2,
}

internal static class AurialRules
{
    public static AuraSense Sense(float distance, float innerRadius, float outerRadius, float lightRadius)
    {
        if (distance > outerRadius * lightRadius)
        {
            return AuraSense.None;
        }
        return distance <= innerRadius * lightRadius ? AuraSense.PlayerColor : AuraSense.Place;
    }
}
