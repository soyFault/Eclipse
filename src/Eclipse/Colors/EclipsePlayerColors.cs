using MiraAPI.Colors;
using MiraAPI.Colors.ColorSpaces;
using MiraAPI.Utilities;
using UnityEngine;

[RegisterCustomColors]
public static class EclipsePlayerColors
{
    // Cerúleo
    public static CustomColor Cerulean { get; } = new("Cerulean", new Color(0.0f, 0.48f, 0.65f))
    {
        ColorBrightness = CustomColorBrightness.Lighter,
    };

    // Rosa
    public static CustomColor Rose { get; } = new("Rose", new Color(0.98f, 0.26f, 0.62f));

    // Aúreo
    public static CustomColor Gold { get; } = new("Gold", new Color(1.0f, 0.84f, 0.0f));

    // Arcoiris
    public static VariableColor Rainbow { get; } = new("Rainbow", t => new HsbColor(MathUtilities.PingPong(t, 1f, 0.3f), 1f, 1f))
    {
        ColorBrightness = CustomColorBrightness.Lighter,
    };
    //
    public static CustomColor Watermelon { get; } = new("Perla",
            new Color32(239, 239, 239, byte.MaxValue),   // base
            new Color32(105, 105, 105, byte.MaxValue))   // sombra
        {
            ColorBrightness = CustomColorBrightness.Lighter
        };

    // Fault
    private static readonly Color Pink = new Color32(255, 60, 217, byte.MaxValue);
    private static readonly Color Orange = new Color32(255, 142, 6, byte.MaxValue);

    private static Color PinkToOrange(float p) => new(
        MathUtilities.Lerp(Pink.r, Orange.r, p),
        MathUtilities.Lerp(Pink.g, Orange.g, p),
        MathUtilities.Lerp(Pink.b, Orange.b, p));

    public static VariableColor PinkOrange { get; } = new("PinkOrange",
        t => PinkToOrange(MathUtilities.PingPong(t, 1f, 0.9f)))
    {
        ColorBrightness = CustomColorBrightness.Lighter,
    };

    // otrocolor
}
