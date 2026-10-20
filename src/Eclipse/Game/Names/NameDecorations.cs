using System;
using System.Collections.Generic;
using Eclipse.Core;
using TMPro;
using UnityEngine;

namespace Eclipse.Game;

internal static class NameDecorations
{
    private static readonly List<MarkProvider> Providers = [];
    private static readonly Dictionary<byte, NameParts> AppliedInWorld = [];
    private static readonly Dictionary<byte, NameParts> AppliedInMeeting = [];
    private static readonly Dictionary<int, Vector3> ColorBlindOriginals = [];

    private static readonly Vector3 ColorBlindCorner = new(-0.93f, -0.2f, -0.1f);

    public delegate string? MarkProvider(PlayerControl viewer, PlayerControl target);

    public static void Register(MarkProvider provider)
    {
        Providers.Add(provider);
    }
    public static void Reset()
    {
        AppliedInWorld.Clear();
        AppliedInMeeting.Clear();
        ColorBlindOriginals.Clear();
    }


    public static void UpdateInWorld(PlayerControl target)
    {
        var text = target.cosmetics?.nameText;
        if (Update(target, text, AppliedInWorld) && text != null)
        {
            text.alignment = TextAlignmentOptions.Bottom;
        }
    }

    public static void UpdateInMeeting(PlayerControl target, PlayerVoteArea card)
    {
        var text = card.NameText;
        var changed = Update(target, text, AppliedInMeeting);
        MoveColorBlindName(card, AppliedInMeeting.TryGetValue(target.PlayerId, out var parts) && !parts.IsEmpty);

        if (changed && text != null)
        {
            text.fontSize = 2f;
            text.ForceMeshUpdate();
            if (text.m_lineNumber > 1)
            {
                text.fontSize = 2f - (text.m_lineNumber * 0.15f);
            }
        }
    }

    private static void MoveColorBlindName(PlayerVoteArea card, bool decorated)
    {
        var helper = card.ColorBlindName != null ? card.ColorBlindName.transform : null;
        if (helper == null)
        {
            return;
        }

        var key = card.GetInstanceID();
        if (decorated)
        {
            if (!ColorBlindOriginals.ContainsKey(key))
            {
                ColorBlindOriginals[key] = helper.localPosition;
            }

            helper.localPosition = ColorBlindCorner;
        }
        else if (ColorBlindOriginals.Remove(key, out var original))
        {
            helper.localPosition = original;
        }
    }

    private static bool Update(PlayerControl target, TextMeshPro? text, Dictionary<byte, NameParts> applied)
    {
        var viewer = PlayerControl.LocalPlayer;
        if (text == null || viewer == null || viewer.Data == null || target.Data == null)
        {
            return false;
        }

        if (!applied.TryGetValue(target.PlayerId, out var current))
        {
            current = default;
        }

        var wanted = Want(viewer, target);
        if (wanted.IsEmpty && current.IsEmpty)
        {
            return false;
        }

        applied[target.PlayerId] = wanted;
        var original = text.text;
        var shown = NameDecoration.Reconcile(original, current, wanted);
        if (shown == original)
        {
            return false;
        }

        text.text = shown;
        return true;
    }

    private static NameParts Want(PlayerControl viewer, PlayerControl target)
    {
        var marks = Providers.Count == 0 ? string.Empty : Ask(viewer, target);

        var line = target.CurrentOutfitType == PlayerOutfitType.Default || target.AmOwner ? RoleLabels.Line(viewer, target) : null;
        var (prefix, tail) = RoleLabelRules.Place(line, RoleLabels.Style);
        return new NameParts(prefix, marks + tail);
    }

    private static string Ask(PlayerControl viewer, PlayerControl target)
    {
        if (target.CurrentOutfitType != PlayerOutfitType.Default)
        {
            return string.Empty;
        }

        foreach (var provider in Providers)
        {
            try
            {
                var mark = provider(viewer, target);
                if (!string.IsNullOrEmpty(mark))
                {
                    return mark;
                }
            }
            catch (Exception)
            {
            }
        }

        return string.Empty;
    }
}
