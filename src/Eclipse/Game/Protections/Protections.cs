using System.Collections.Generic;
using System.Linq;
using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Options.Roles.Crewmate;
using MiraAPI.GameOptions;
using PowerTools;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal static class Protections
{

    private const float BarrierY = -0.30f;
    private const float BarrierZ = 1.5f;

    private static readonly Dictionary<ProtectionKind, (string Prefab, float Speed)> Looks = new()
    {
        [ProtectionKind.Fortify] = ("WardenFort", 0.75f),
        [ProtectionKind.Barrier] = ("ClericBarrier", 1f),
    };

    private static readonly Dictionary<(ProtectionKind, byte, byte), Visual> Visuals = [];

    public static ProtectionLedger Ledger { get; } = new();

    public static double Now => Time.timeAsDouble;

    public static Protection? Blocker(PlayerControl source, PlayerControl target, bool ignoresDefense, bool isAttack = true)
    {
        var blocker = Ledger.BlockerOf(target.PlayerId, Now, isAttack);
        var blocks = ShieldRules.Blocks(blocker != null, source.PlayerId, target.PlayerId, ignoresDefense, MeetingHud.Instance || ExileController.Instance);
        return blocks ? blocker : null;
    }

    public static void OnBlocked(ProtectionKind kind, PlayerControl source, PlayerControl protector, bool wasAttack)
    {
        if (source.AmOwner || (protector.AmOwner && ProtectionRules.NotifiesProtector(kind, OptionGroupSingleton<ClericOptions>.Instance.AttackNotification)))
        {
            ScreenFlash.Flash(ColorOf(kind));
        }

        if (source.AmOwner && wasAttack)
        {
            ShieldGuard.ResetStoppedAttacker(source);
        }
    }

    public static void Reset()
    {
        Ledger.Clear();
        SyncVisuals();
    }

    public static void Tick(float deltaTime)
    {
        Ledger.Prune(Now);
        SyncVisuals();
    }

    private static Color ColorOf(ProtectionKind kind)
    {
        return kind switch
        {
            ProtectionKind.Barrier => EclipseColors.Cleric,
            ProtectionKind.Vest => EclipseColors.Survivor,
            ProtectionKind.Guardian => EclipseColors.Fairy,
            _ => EclipseColors.Warden,
        };
    }

    private static void SyncVisuals()
    {
        var local = PlayerControl.LocalPlayer;
        var wanted = new HashSet<(ProtectionKind, byte, byte)>();
        if (local != null && local.Data != null && MeetingHud.Instance == null)
        {
            var dead = local.Data.IsDead;
            foreach (var p in Ledger.Active(ProtectionKind.Fortify, Now))
            {
                var shows = ProtectionRules.ShowsFortify(
                    OptionGroupSingleton<WardenOptions>.Instance.ShowFortified,
                    local.PlayerId == p.TargetId,
                    local.PlayerId == p.ProtectorId,
                    dead);
                if (shows)
                {
                    wanted.Add((p.Kind, p.ProtectorId, p.TargetId));
                }
            }

            foreach (var p in Ledger.Active(ProtectionKind.Barrier, Now))
            {
                var shows = ProtectionRules.ShowsBarrier(
                    OptionGroupSingleton<ClericOptions>.Instance.ShowBarrier,
                    local.PlayerId == p.TargetId,
                    local.PlayerId == p.ProtectorId,
                    dead);
                if (shows)
                {
                    wanted.Add((p.Kind, p.ProtectorId, p.TargetId));
                }
            }
        }

        foreach (var key in Visuals.Keys.Where(k => !wanted.Contains(k)).ToList())
        {
            Visuals[key].Destroy();
            Visuals.Remove(key);
        }

        foreach (var key in wanted)
        {
            var player = GameData.Instance.GetPlayerById(key.Item3)?.Object;
            if (player == null)
            {
                continue;
            }

            if (!Visuals.TryGetValue(key, out var visual) || visual.IsGone)
            {
                visual = Visual.Create(player, key.Item1);
                Visuals[key] = visual;
            }

            visual.Update(player, !player.inVent && !player.Data.IsDead);
        }
    }

    private sealed class Visual(GameObject? holder, SpriteRenderer[] renderers)
    {
        private bool? _shown;

        public bool IsGone => holder == null && renderers.Length > 0;

        public static Visual Create(PlayerControl player, ProtectionKind kind)
        {
            var (prefabName, speed) = Looks[kind];
            var prefab = EclipseBundle.Find<GameObject>(BundleArt.PrefabsFolder, prefabName);
            if (prefab == null)
            {
                Logger<EclipsePlugin>.Warning($"Protection visual on player {player.PlayerId}: prefab '{prefabName}' is not in the bundle.");
                return new Visual(null, []);
            }

            var barrier = Object.Instantiate(prefab, player.transform);
            barrier.name = "Eclipse" + prefabName;
            var playerScale = player.transform.localScale;
            barrier.transform.localScale = new Vector3(prefab.transform.localScale.x / playerScale.x, prefab.transform.localScale.y / playerScale.y, 1f);

            var holder = new GameObject("A_Eclipse" + prefabName);
            holder.transform.SetParent(player.cosmetics.transform, false);
            holder.transform.localPosition = new Vector3(0f, BarrierY, BarrierZ);
            barrier.transform.SetParent(holder.transform, true);

            foreach (var animation in barrier.GetComponentsInChildren<SpriteAnim>(true))
            {
                animation.SetSpeed(speed);
                if (animation.m_defaultAnim == null)
                {
                    Logger<EclipsePlugin>.Warning($"Protection visual '{prefabName}': a SpriteAnim has no animation clip, so nothing will play. Rebuild the prefab and the bundles.");
                }
            }

            var renderers = barrier.GetComponentsInChildren<SpriteRenderer>(true);
            Logger<EclipsePlugin>.Info($"Protection visual '{prefabName}' created on player {player.PlayerId}: {renderers.Length} sprite renderer(s).");
            return new Visual(holder, renderers);
        }

        public void Update(PlayerControl player, bool visible)
        {
            if (holder == null)
            {
                return;
            }

            if (_shown != visible)
            {
                _shown = visible;
                Logger<EclipsePlugin>.Info($"Protection visual on player {player.PlayerId}: shown={visible} (inVent={player.inVent}, dead={player.Data.IsDead}).");
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
