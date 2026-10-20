using MiraAPI.Colors;
using MiraAPI.Colors.ColorSpaces;
using MiraAPI.Utilities;
using UnityEngine;

[RegisterCustomColors]
public static class EclipsePlayerColors
{
    public static CustomColor Cerulean { get; } = new("Cerulean", new Color(0.0f, 0.48f, 0.65f))
    {
        ColorBrightness = CustomColorBrightness.Lighter,
    };

    public static CustomColor Rose { get; } = new("Rose", new Color(0.98f, 0.26f, 0.62f));

    public static CustomColor Gold { get; } = new("Gold", new Color(1.0f, 0.84f, 0.0f));

    public static VariableColor Rainbow { get; } = new("Rainbow", t => new HsbColor(MathUtilities.PingPong(t, 1f, 0.3f), 1f, 1f))
    {
        ColorBrightness = CustomColorBrightness.Lighter,
    };
}
