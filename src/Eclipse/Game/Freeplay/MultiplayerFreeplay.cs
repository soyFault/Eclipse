using AmongUs.GameOptions;
using System.Collections.Generic;
using System.Linq;
using Eclipse.Core;
using Eclipse.Networking;
using Eclipse.Options.Host;
using InnerNet;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Networking;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal static class MultiplayerFreeplay
{
    private sealed record Snapshot(RoleTypes Role, Vector2 Position, List<uint> Modifiers);

    private static readonly Dictionary<byte, Snapshot> Baseline = [];

    private static bool _captured;

    public static bool Enabled
    {
        get
        {
            var client = AmongUsClient.Instance;
            return client != null && MultiplayerFreeplayRules.Enabled(
                OptionGroupSingleton<HostOptions>.Instance.MultiplayerFreeplay,
                TutorialManager.InstanceExists,
                client.GameState == InnerNetClient.GameStates.Started);
        }
    }

    public static bool LocalMayUse => MultiplayerFreeplayRules.MayUse(
        Enabled,
        OptionGroupSingleton<HostOptions>.Instance.FreeplayHostOnly,
        AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost);

    public static bool PreventsGameEnd => MultiplayerFreeplayRules.PreventsGameEnd(Enabled, OptionGroupSingleton<HostOptions>.Instance.FreeplayNoEnd);

    [RegisterEvent]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        if (roundStart.TriggeredByIntro && AmongUsClient.Instance.AmHost && Enabled)
        {
            CaptureBaseline();
        }
    }

    [RegisterEvent]
    public static void OnGameEnd(GameEndEvent gameEnd)
    {
        Baseline.Clear();
        _captured = false;
    }

    public static void Handle(PlayerControl sender, FreeplayAction action, byte targetId, byte otherId, uint data)
    {
        var client = AmongUsClient.Instance;
        if (client == null || sender == null || sender.Data == null)
        {
            return;
        }

        var options = OptionGroupSingleton<HostOptions>.Instance;
        var accepted = MultiplayerFreeplayRules.HostAccepts(
            client.AmHost,
            Enabled,
            options.FreeplayHostOnly,
            client.HostId == sender.OwnerId,
            !sender.Data.Disconnected);
        if (!accepted)
        {
            return;
        }

        var target = Find(targetId);
        switch (action)
        {
            case FreeplayAction.SetRole when target != null:
                Revive(target);
                FreeplayRpcs.RpcApplyRole(target, (ushort)data);
                break;

            case FreeplayAction.Revive when target != null:
                Revive(target);
                break;

            case FreeplayAction.RemoteKill when target != null && Find(otherId) is { } victim && !victim.Data.IsDead:
                target.RpcCustomMurder(victim, MeetingCheck.OutsideMeeting);
                break;

            case FreeplayAction.ToggleModifier when target != null:
                ToggleModifier(target, data);
                break;

            case FreeplayAction.Reset:
                Reset();
                break;
        }
    }

    private static PlayerControl? Find(byte playerId)
    {
        var player = GameData.Instance?.GetPlayerById(playerId)?.Object;
        return player == null || player.Data == null || player.Data.Disconnected ? null : player;
    }

    private static void Revive(PlayerControl player)
    {
        if (player.Data.IsDead)
        {
            FreeplayRpcs.RpcRevive(player);
        }
    }

    public static void ToggleModifier(PlayerControl target, uint typeId)
    {
        var existing = target.GetModifiers<BaseModifier>().FirstOrDefault(modifier => modifier.TypeId == typeId);
        if (existing != null)
        {
            target.RpcRemoveModifier(existing.UniqueId);
            return;
        }

        target.RpcAddModifier(typeId);
    }

    private static void CaptureBaseline()
    {
        Baseline.Clear();
        foreach (var player in PlayerControl.AllPlayerControls.ToArray())
        {
            if (player == null || player.Data?.Role == null)
            {
                continue;
            }

            var modifiers = player.GetModifiers<GameModifier>().Select(modifier => modifier.TypeId).Distinct().ToList();
            Baseline[player.PlayerId] = new Snapshot(player.Data.Role.Role, player.transform.position, modifiers);
        }

        _captured = true;
        Logger<EclipsePlugin>.Info($"Multiplayer Freeplay: baseline of {Baseline.Count} player(s) saved.");
    }

    private static void Reset()
    {
        if (!_captured)
        {
            Logger<EclipsePlugin>.Warning("Multiplayer Freeplay: no baseline was saved, so there is nothing to go back to.");
            return;
        }

        FreeplayRpcs.RpcResetState(PlayerControl.LocalPlayer);
        foreach (var player in PlayerControl.AllPlayerControls.ToArray())
        {
            if (player == null || player.Data == null || player.Data.Disconnected || !Baseline.TryGetValue(player.PlayerId, out var snapshot))
            {
                continue;
            }

            foreach (var modifier in player.GetModifiers<GameModifier>().ToList())
            {
                player.RpcRemoveModifier(modifier.UniqueId);
            }

            FreeplayRpcs.RpcRestorePlayer(player, (ushort)snapshot.Role, snapshot.Position.x, snapshot.Position.y);
            foreach (var typeId in snapshot.Modifiers)
            {
                player.RpcAddModifier(typeId);
            }
        }
    }
}
