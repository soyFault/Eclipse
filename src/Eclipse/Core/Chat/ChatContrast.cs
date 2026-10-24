using System;
using System.Text.RegularExpressions;

namespace Eclipse.Core;

internal static class ChatContrast
{
    private const float MinLuminance = 0.45f;

    private static readonly Regex ColorTag = new("<color=#([0-9A-Fa-f]{6})([0-9A-Fa-f]{2})?>", RegexOptions.Compiled);

    public static float Luminance(float r, float g, float b)
    {
        return (0.2126f * r) + (0.7152f * g) + (0.0722f * b);
    }

    public static (float R, float G, float B) Lift(float r, float g, float b)
    {
        var luminance = Luminance(r, g, b);
        if (luminance >= MinLuminance)
        {
            return (r, g, b);
        }

        var amount = (MinLuminance - luminance) / (1f - luminance);
        return (r + ((1f - r) * amount), g + ((1f - g) * amount), b + ((1f - b) * amount));
    }

    public static string LiftColors(string text)
    {
        return ColorTag.Replace(text, match =>
        {
            var hex = match.Groups[1].Value;
            var r = Convert.ToInt32(hex.Substring(0, 2), 16) / 255f;
            var g = Convert.ToInt32(hex.Substring(2, 2), 16) / 255f;
            var b = Convert.ToInt32(hex.Substring(4, 2), 16) / 255f;
            var lifted = Lift(r, g, b);
            return $"<color=#{ToByte(lifted.R):X2}{ToByte(lifted.G):X2}{ToByte(lifted.B):X2}{match.Groups[2].Value}>";
        });
    }

    private static int ToByte(float value)
    {
        return (int)Math.Round(Math.Clamp(value, 0f, 1f) * 255f);
    }
}
