namespace Eclipse.Core;

internal enum ColorTone
{
    Darker,
    Lighter,
}

internal static class ColorTones
{
    public const int VanillaTableSize = 18;

    private static readonly ColorTone[] Vanilla =
    [
        ColorTone.Darker, ColorTone.Darker, ColorTone.Darker, ColorTone.Lighter, ColorTone.Lighter, ColorTone.Lighter,
        ColorTone.Darker, ColorTone.Lighter, ColorTone.Darker, ColorTone.Darker, ColorTone.Lighter, ColorTone.Lighter,
        ColorTone.Darker, ColorTone.Lighter, ColorTone.Lighter, ColorTone.Darker, ColorTone.Darker, ColorTone.Lighter,
    ];

    public static ColorTone? OfVanillaColor(int colorId)
    {
        return colorId >= 0 && colorId < Vanilla.Length ? Vanilla[colorId] : null;
    }

    public static ColorTone FromBrightness(float red, float green, float blue)
    {
        var luma = (0.299f * red) + (0.587f * green) + (0.114f * blue);
        return luma >= 0.5f ? ColorTone.Lighter : ColorTone.Darker;
    }
}
