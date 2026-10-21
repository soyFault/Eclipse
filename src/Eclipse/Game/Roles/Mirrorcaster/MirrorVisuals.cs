using System.Collections.Generic;
using System.Linq;
using Eclipse.Assets;
using Eclipse.Core;
using PowerTools;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal static class MirrorVisuals
{
    // Where the drawing sits, in the player's cosmetics layer (TOUM's AnimStore.SpawnAnimBody(player, prefab, false, -1.1, -0.1, 1.5)).
    private const float DrawingY = -0.05f;
    private const float DrawingZ = 1.5f;
    private const string PrefabName = "MagicMirror";

    private static readonly Dictionary<byte, Visual> Shown = [];
    private static bool _reportedMissing;

    // Draws the Magic Mirror prefab on every mirrored player the local player may see. Without that prefab in the bundle nothing is drawn: the mark on
    // the name is all there is.
    public static void Sync()
    {
        var local = PlayerControl.LocalPlayer;
        var wanted = new HashSet<byte>();
        if (local != null && local.Data != null && MeetingHud.Instance == null)
        {
            foreach (var (targetId, mirror) in Mirrors.Carried)
            {
                if (mirror.ExpiresAt > Mirrors.Now && MirrorcasterRules.ShowsMirror(local.PlayerId == mirror.Mirrorcaster, local.Data.IsDead))
                {
                    wanted.Add(targetId);
                }
            }
        }

        foreach (var id in Shown.Keys.Where(k => !wanted.Contains(k)).ToList())
        {
            Shown[id].Destroy();
            Shown.Remove(id);
        }

        foreach (var id in wanted)
        {
            var player = GameData.Instance.GetPlayerById(id)?.Object;
            if (player == null)
            {
                continue;
            }

            if (!Shown.TryGetValue(id, out var visual) || visual.IsGone)
            {
                visual = Visual.Create(player);
                Shown[id] = visual;
            }

            visual.Update(player, !player.inVent && !player.Data.IsDead);
        }
    }

    private sealed class Visual(GameObject? holder, SpriteRenderer[] renderers)
    {
        public bool IsGone => holder == null && renderers.Length > 0;

        public static Visual Create(PlayerControl player)
        {
            var prefab = EclipseBundle.Find<GameObject>(BundleArt.PrefabsFolder, PrefabName);
            if (prefab == null)
            {
                if (!_reportedMissing)
                {
                    _reportedMissing = true;
                    Logger<EclipsePlugin>.Info($"Magic Mirror: prefab '{PrefabName}' is not in the bundle, so only the mark on the name is shown.");
                }

                return new Visual(null, []);
            }

            var drawing = Object.Instantiate(prefab, player.transform);
            drawing.name = "Eclipse" + PrefabName;
            var playerScale = player.transform.localScale;
            drawing.transform.localScale = new Vector3(prefab.transform.localScale.x / playerScale.x, prefab.transform.localScale.y / playerScale.y, 1f);

            var holder = new GameObject("A_Eclipse" + PrefabName);
            holder.transform.SetParent(player.cosmetics.transform, false);
            holder.transform.localPosition = new Vector3(0f, DrawingY, DrawingZ);
            drawing.transform.SetParent(holder.transform, true);

            foreach (var animation in drawing.GetComponentsInChildren<SpriteAnim>(true))
            {
                if (animation.m_defaultAnim == null)
                {
                    Logger<EclipsePlugin>.Warning($"Magic Mirror: a SpriteAnim has no animation clip, so nothing will play. Rebuild the prefab and the bundles.");
                }
            }

            return new Visual(holder, drawing.GetComponentsInChildren<SpriteRenderer>(true));
        }

        public void Update(PlayerControl player, bool visible)
        {
            if (holder == null)
            {
                return;
            }

            holder.SetActive(visible);
            var flip = player.cosmetics.FlipX;
            foreach (var renderer in renderers)
            {
                renderer.flipX = flip;
            }
        }

        public void Destroy()
        {
            if (holder != null)
            {
                Object.Destroy(holder);
            }
        }
    }
}
