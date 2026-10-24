using Reactor.Utilities;
using TMPro;
using UnityEngine;

namespace Eclipse.Game;

internal static class EndScreenText
{
    public static TextMeshPro? Create(EndGameManager manager, string name, float fontSize, TextAlignmentOptions alignment, float viewportX, float viewportY)
    {
        var title = manager.WinText;
        var camera = Camera.main;
        if (title == null || camera == null)
        {
            Logger<EclipsePlugin>.Warning($"End screen text '{name}': no title or no camera, so it was not drawn.");
            return null;
        }

        var holder = new GameObject(name)
        {
            layer = title.gameObject.layer,
        };
        holder.transform.SetParent(title.transform.parent, false);
        holder.transform.localScale = Vector3.one;

        var label = holder.AddComponent<TextMeshPro>();
        label.font = title.font;
        label.fontSharedMaterial = title.fontSharedMaterial;
        label.color = Color.white;
        label.enableAutoSizing = false;
        label.fontSize = fontSize;
        label.alignment = alignment;
        label.enableWordWrapping = false;
        label.rectTransform.sizeDelta = new Vector2(14f, 6f);

        var anchoredLeft = alignment == TextAlignmentOptions.TopLeft || alignment == TextAlignmentOptions.Left || alignment == TextAlignmentOptions.BottomLeft;
        label.rectTransform.pivot = new Vector2(anchoredLeft ? 0f : 0.5f, alignment == TextAlignmentOptions.TopLeft ? 1f : 0.5f);
        label.sortingOrder = 100;
        label.text = string.Empty;

        var world = camera.ViewportToWorldPoint(new Vector3(viewportX, viewportY, 0f));
        world.z = title.transform.position.z - 1f;
        holder.transform.position = world;

        Logger<EclipsePlugin>.Info(
            $"End screen text '{name}': at {world}, layer {holder.layer}, parent '{holder.transform.parent.name}' scale {holder.transform.parent.lossyScale}, title active {title.gameObject.activeInHierarchy}.");
        return label;
    }
}
