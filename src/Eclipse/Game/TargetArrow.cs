using System.Collections;
using Reactor.Utilities;
using Eclipse.Assets;
using Eclipse.Core;
using UnityEngine;

namespace Eclipse.Game;

internal sealed class TargetArrow
{
    private const int Size = 64;

    private static Sprite? _sprite;

    private ArrowBehaviour? _arrow;
    public static TargetArrow Create(Transform parent, Color color)
    {
        var gameObject = new GameObject("EclipseTargetArrow")
        {
            layer = 5,
        };
        gameObject.transform.SetParent(parent, false);

        var renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = GetSprite();
        renderer.color = color;

        var behaviour = gameObject.AddComponent<ArrowBehaviour>();
        behaviour.image = renderer;

        return new TargetArrow { _arrow = behaviour };
    }

    public static TargetArrow Blip(Transform parent, Color color, Vector3 position, float seconds)
    {
        var arrow = Create(parent, color);
        arrow.PointAt(position);
        Coroutines.Start(CoRemove(arrow, seconds));
        return arrow;
    }

    private static IEnumerator CoRemove(TargetArrow arrow, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        arrow.Destroy();
    }

    public void PointAt(Vector3 position)
    {
        if (_arrow == null)
        {
            return;
        }

        _arrow.target = position;
        _arrow.Update();
    }

    public void Destroy()
    {
        if (_arrow != null)
        {
            Object.Destroy(_arrow.gameObject);
        }

        _arrow = null;
    }

    private static Sprite GetSprite()
    {
        _sprite ??= EclipseBundle.Find<Sprite>(BundleArt.SpritesFolder, "Arrow") ?? BuildSprite();
        return _sprite;
    }

    private static Sprite BuildSprite()
    {
        var texture = new Texture2D(Size, Size, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Bilinear,
            hideFlags = HideFlags.HideAndDontSave,
        };

        var clear = new Color(0f, 0f, 0f, 0f);
        for (var x = 0; x < Size; x++)
        {
            for (var y = 0; y < Size; y++)
            {
                var halfHeight = (Size - 8 - x) * 0.5f;
                var inside = x >= 8 && x <= Size - 8 && Mathf.Abs(y - (Size / 2f)) <= halfHeight;
                texture.SetPixel(x, y, inside ? Color.white : clear);
            }
        }

        texture.Apply();

        var sprite = Sprite.Create(texture, new Rect(0, 0, Size, Size), new Vector2(0.5f, 0.5f), 100f);
        sprite.hideFlags = HideFlags.HideAndDontSave;
        return sprite;
    }
}
