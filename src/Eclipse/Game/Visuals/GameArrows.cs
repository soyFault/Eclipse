using System.Collections.Generic;
using Eclipse.Assets;
using Eclipse.Core;
using Eclipse.Ui;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal static class GameArrows
{
    private const string EclipseSpriteName = "EclipseArrow";

    private const float ArrowScale = 1f;

    private static readonly Color? TaskArrowTint = null;
    private static readonly Color? VentArrowTint = null;

    private static readonly Dictionary<string, Sprite?> Replacements = [];
    private static readonly HashSet<string> Seen = [];

    public static void Swap(SpriteRenderer? renderer, string kind, string artName)
    {
        if (renderer == null || renderer.sprite == null)
        {
            return;
        }

        CosmeticGuard.Run("game-arrow", () =>
        {
            var vanilla = renderer.sprite;
            if (vanilla.name == EclipseSpriteName)
            {
                return;
            }

            Audit(kind, renderer, vanilla);

            var replacement = ReplacementFor(vanilla, artName);
            if (replacement != null)
            {
                renderer.sprite = replacement;
                var tint = kind == "vent" ? VentArrowTint : TaskArrowTint;
                if (tint is { } color)
                {
                    renderer.color = color;
                }
            }
        });
    }

    private static Sprite? ReplacementFor(Sprite vanilla, string artName)
    {
        var key = $"{artName}|{vanilla.name}";
        if (Replacements.TryGetValue(key, out var cached) && (cached == null || cached.texture != null))
        {
            return cached;
        }

        var art = EclipseBundle.Find<Sprite>(BundleArt.SpritesFolder, artName);
        if (art == null && artName != "Arrow")
        {
            art = EclipseBundle.Find<Sprite>(BundleArt.SpritesFolder, "Arrow");
        }

        Sprite? made = null;
        if (art != null)
        {
            var pixelsPerUnit = art.rect.width / (vanilla.bounds.size.x * ArrowScale);
            var pivot = new Vector2(vanilla.pivot.x / vanilla.rect.width, vanilla.pivot.y / vanilla.rect.height);
            made = Sprite.Create(art.texture, art.rect, pivot, pixelsPerUnit, 0, SpriteMeshType.FullRect);
            made.name = EclipseSpriteName;
            made.hideFlags = HideFlags.HideAndDontSave;
        }

        Replacements[key] = made;
        return made;
    }

    private static void Audit(string kind, SpriteRenderer renderer, Sprite vanilla)
    {
        if (!Seen.Add($"{kind}|{vanilla.name}"))
        {
            return;
        }

        Logger<EclipsePlugin>.Info(
            $"Arrow audit [{kind}]: sprite '{vanilla.name}' {vanilla.rect.width}x{vanilla.rect.height} px, {vanilla.pixelsPerUnit} ppu, " +
            $"{vanilla.bounds.size.x:0.##} units wide, color {renderer.color}, on '{renderer.gameObject.name}' (parent '{renderer.transform.parent?.name}').");
    }
}
