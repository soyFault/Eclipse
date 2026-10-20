using System.Linq;
using Eclipse.Assets;
using Eclipse.Core;
using Eclipse.Roles;
using Eclipse.Ui;
using HarmonyLib;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Patches;

[HarmonyPatch(typeof(OverlayKillAnimation), nameof(OverlayKillAnimation.CoShow))]
internal static class KillOverlayPatch
{
    private static Sprite? _gameSprite;
    private static Vector3? _gamePosition;
    private static bool _sizeLogged;

    [HarmonyPrefix]
    public static void Prefix(OverlayKillAnimation __instance, KillOverlay parent)
    {
        CosmeticGuard.Run("kill-background", () =>
        {
            var quad = parent.transform.Find("QuadParent");
            var flame = quad != null ? quad.Find("BackgroundFlame") : null;
            if (flame == null || !flame.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                return;
            }

            _gameSprite ??= renderer.sprite;
            _gamePosition ??= quad!.localPosition;
            if (!_sizeLogged && _gameSprite != null)
            {
                _sizeLogged = true;
                Logger<EclipsePlugin>.Info($"Kill background: the game's sprite is {_gameSprite.rect.width}x{_gameSprite.rect.height} px at {_gameSprite.pixelsPerUnit} pixels per unit.");
            }

            var killerName = __instance.initData?.killerOutfit?.PlayerName;
            var killer = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(player => player.Data != null && player.Data.PlayerName == killerName);
            var faction = killer?.Data?.Role != null ? RoleClassification.Classify(killer.Data.Role).Faction : (Api.Factions.Faction?)null;

            var art = KillBackgroundRules.For(faction);
            var sprite = art != null ? EclipseAssets.KillBackground(art)?.LoadAsset() : null;
            renderer.sprite = sprite != null ? sprite : _gameSprite;
            quad!.localPosition = sprite != null ? Vector3.zero : _gamePosition.Value;
        });
    }
}
