using System.Collections.Generic;
using Eclipse.Core;

namespace Eclipse.Game;

internal static class MeetingHints
{
    private static readonly List<HintProvider> Providers = [];
    private static string _applied = string.Empty;
    public delegate string? HintProvider(PlayerControl local);
    public static void Register(HintProvider provider)
    {
        Providers.Add(provider);
    }
    public static void Reset()
    {
        _applied = string.Empty;
    }

    public static void Update(MeetingHud meeting)
    {
        var text = meeting.TimerText;
        var local = PlayerControl.LocalPlayer;
        if (text == null || local == null || local.Data == null || Providers.Count == 0)
        {
            return;
        }

        var wanted = local.Data.IsDead ? string.Empty : Ask(local);
        if (wanted.Length == 0 && _applied.Length == 0)
        {
            return;
        }

        var shown = text.text;
        shown = wanted == _applied
            ? TextMark.Apply(shown, wanted)
            : TextMark.Apply(TextMark.Remove(shown, _applied), wanted);
        _applied = wanted;

        if (shown != text.text)
        {
            text.text = shown;
        }
    }

    private static string Ask(PlayerControl local)
    {
        foreach (var provider in Providers)
        {
            try
            {
                var hint = provider(local);
                if (!string.IsNullOrEmpty(hint))
                {
                    return $"<color=#FFFFFF>\n{hint}</color>";
                }
            }
            catch (System.Exception)
            {
            }
        }

        return string.Empty;
    }
}
