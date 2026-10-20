
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Eclipse.Game;

internal static class NameColors
{
    private static readonly List<ColorProvider> Providers = [];
    private static readonly Dictionary<int, Color> Originals = [];

    public delegate Color? ColorProvider(PlayerControl viewer, PlayerControl target);

    public static void Register(ColorProvider provider)
    {
        Providers.Add(provider);
    }

    public static void Reset()
    {
        Originals.Clear();
    }

    public static void UpdateInWorld(PlayerControl target)
    {
        Update(target, target.cosmetics?.nameText);
    }

    public static void UpdateInMeeting(PlayerControl target, TextMeshPro? text)
    {
        Update(target, text);
    }

    private static void Update(PlayerControl target, TextMeshPro? text)
    {
        var viewer = PlayerControl.LocalPlayer;
        if (text == null || viewer == null || viewer.Data == null || target.Data == null || Providers.Count == 0)
        {
            return;
        }

        var wanted = Ask(viewer, target);
        var id = text.GetInstanceID();
        if (wanted is { } color)
        {
            if (!Originals.ContainsKey(id))
            {
                Originals[id] = text.color;
            }

            text.color = color;
        }
        else if (Originals.TryGetValue(id, out var original))
        {
            text.color = original;
            Originals.Remove(id);
        }
    }

    private static Color? Ask(PlayerControl viewer, PlayerControl target)
    {
        if (target.CurrentOutfitType != PlayerOutfitType.Default)
        {
            return null;
        }

        foreach (var provider in Providers)
        {
            try
            {
                var color = provider(viewer, target);
                if (color != null)
                {
                    return color;
                }
            }
            catch (System.Exception)
            {
            }
        }

        return null;
    }
}
