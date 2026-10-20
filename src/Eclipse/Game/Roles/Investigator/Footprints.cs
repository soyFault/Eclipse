using System.Collections;
using System.Collections.Generic;
using Eclipse.Assets;
using Eclipse.Core;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal static class Footprints
{

    private const float PrintHeight = 0.75f;
    private const float Widen = 1.2f;
    private const int FallbackSize = 64;
    private const float FadeSeconds = 0.4f;
    private static readonly Dictionary<byte, Trail> Trails = [];
    private static Sprite? _sprite;

    public static void Tick(float deltaTime)
    {
        if (PlayerControl.LocalPlayer?.Data?.Role is not InvestigatorRole || PlayerControl.LocalPlayer.Data.IsDead || !ShipStatus.Instance)
        {
            Trails.Clear();
            return;
        }

        var options = OptionGroupSingleton<InvestigatorOptions>.Instance;
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player == null || player.AmOwner || player.Data == null || player.Data.IsDead || player.Data.Disconnected)
            {
                continue;
            }

            if (!Trails.TryGetValue(player.PlayerId, out var trail))
            {
                trail = new Trail();
                Trails[player.PlayerId] = trail;
            }

            trail.Elapsed += deltaTime;
            var moved = Vector3.Distance(trail.Last, player.transform.position);
            if (!FootprintRules.ShouldPrint(options.Mode, moved, trail.Elapsed, options.Interval))
            {
                continue;
            }

            if (!options.ShowFootprintVent && IsNextToAVent(player))
            {
                continue;
            }

            Print(player, options);
            trail.Last = player.transform.position;
            trail.Elapsed = 0f;
        }
    }

    private static bool IsNextToAVent(PlayerControl player)
    {
        foreach (var vent in ShipStatus.Instance.AllVents)
        {
            if (vent != null && Vector2.Distance(vent.transform.position, player.GetTruePosition()) < 1f)
            {
                return true;
            }
        }

        return false;
    }

    private static void Print(PlayerControl player, InvestigatorOptions options)
    {
        var sprite = GetSprite();
        var velocity = player.MyPhysics.Velocity;
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        var position = player.transform.position;
        var scale = PrintHeight / sprite.bounds.size.y * (options.FootprintSize / 10f);

        var print = new GameObject("EclipseFootprint")
        {
            layer = LayerMask.NameToLayer("Players"),
        };
        print.transform.SetParent(ShipStatus.Instance.transform);
        print.transform.position = new Vector3(position.x, position.y - 0.3f, position.z + 0.001f);
        print.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        print.transform.localScale = new Vector3(scale * Widen, scale, 1f);

        var renderer = print.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = PrintColor(player, options);

        Coroutines.Start(CoFade(print, renderer, options.FootprintDuration));
    }

    private static Color PrintColor(PlayerControl player, InvestigatorOptions options)
    {
        if (options.ShowAnonymousFootprints || CommsState.IsActive)
        {
            return new Color(0.2f, 0.2f, 0.2f, 1f);
        }
        var material = player.cosmetics.currentBodySprite != null ? player.cosmetics.currentBodySprite.BodySprite.material : null;
        return material != null ? material.GetColor(ShaderID.BodyColor) : (Color)Palette.PlayerColors[player.Data.DefaultOutfit.ColorId];
    }

    private static IEnumerator CoFade(GameObject print, SpriteRenderer renderer, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        for (var elapsed = 0f; elapsed < FadeSeconds; elapsed += Time.deltaTime)
        {
            if (renderer == null)
            {
                yield break;
            }

            var color = renderer.color;
            renderer.color = new Color(color.r, color.g, color.b, 1f - (elapsed / FadeSeconds));
            yield return null;
        }

        if (print != null)
        {
            Object.Destroy(print);
        }
    }

    private static Sprite GetSprite()
    {
        if (_sprite == null)
        {
            _sprite = EclipseAssets.Footprint()?.LoadAsset() ?? BuildFallback();
        }

        return _sprite;
    }

    private static Sprite BuildFallback()
    {
        var texture = new Texture2D(FallbackSize, FallbackSize, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Bilinear,
            hideFlags = HideFlags.HideAndDontSave,
        };

        var clear = new Color(0f, 0f, 0f, 0f);
        for (var x = 0; x < FallbackSize; x++)
        {
            for (var y = 0; y < FallbackSize; y++)
            {
                var dx = (x - (FallbackSize / 2f)) / (FallbackSize * 0.28f);
                var dy = (y - (FallbackSize / 2f)) / (FallbackSize * 0.46f);
                texture.SetPixel(x, y, (dx * dx) + (dy * dy) <= 1f ? Color.white : clear);
            }
        }

        texture.Apply();
        var sprite = Sprite.Create(texture, new Rect(0, 0, FallbackSize, FallbackSize), new Vector2(0.5f, 0.5f), FallbackSize);
        sprite.hideFlags = HideFlags.HideAndDontSave;
        return sprite;
    }

    private sealed class Trail
    {
        public Vector3 Last { get; set; }

        public float Elapsed { get; set; }
    }
}
