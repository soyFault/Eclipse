using Eclipse.Core;
using MiraAPI.Colors;

namespace Eclipse.Game;

internal static class PlayerColorTone
{
    public static ColorTone Of(int colorId)
    {
        if (ColorTones.OfVanillaColor(colorId) is { } known)
        {
            return known;
        }

        var custom = PaletteManager.RegisteredColors;
        var firstCustomId = Palette.PlayerColors.Length - custom.Length;
        var index = colorId - firstCustomId;
        if (index >= 0 && index < custom.Length)
        {
            return custom[index].ColorBrightness == CustomColorBrightness.Lighter ? ColorTone.Lighter : ColorTone.Darker;
        }

        if (colorId >= 0 && colorId < Palette.PlayerColors.Length)
        {
            var color = Palette.PlayerColors[colorId];
            return ColorTones.FromBrightness(color.r / 255f, color.g / 255f, color.b / 255f);
        }

        return ColorTone.Darker;
    }
}
