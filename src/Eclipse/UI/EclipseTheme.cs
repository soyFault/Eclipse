using UnityEngine;

namespace Eclipse.Ui;

internal static class EclipseTheme
{
    public static Color Accent { get; } = new Color32(0xE8, 0xA3, 0x3D, 0xFF);

    public static Color Dark { get; } = new Color32(0x1B, 0x12, 0x30, 0xFF);

    public const float ArtworkTint = 0.5f;


    public static Color TintArtwork(Color original)
    {
        return Color.Lerp(original, Accent, ArtworkTint);
    }
}
