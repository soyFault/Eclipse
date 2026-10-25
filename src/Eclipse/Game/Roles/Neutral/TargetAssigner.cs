using System;
using System.Collections;
using System.Collections.Generic;
using Eclipse.Core;
using Eclipse.Networking;
using Eclipse.Roles;
using MiraAPI.Roles;
using Reactor.Utilities;
using UnityEngine;
using Random = System.Random;

namespace Eclipse.Game;

internal static class TargetAssigner
{
    private const float Delay = 0.5f;

    private static readonly Dictionary<TargetKind, Action<PlayerControl>> NoTargetHandlers = [];

    private static int _generation;

    public static Func<int> FairyEvilPercent { get; set; } = () => 0;

    public static void OnNoTarget(TargetKind kind, Action<PlayerControl> handler)
    {
        NoTargetHandlers[kind] = handler;
    }

    public static void Request()
    {
        if (AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost)
        {
            return;
        }

        _generation++;
        Coroutines.Start(CoAssign(_generation));
    }

    private static IEnumerator CoAssign(int generation)
    {
        yield return new WaitForSeconds(Delay);
        if (generation != _generation || LobbyBehaviour.Instance != null || !GameData.Instance)
        {
            yield break;
        }

        AssignMissing();
    }

    private static void AssignMissing()
    {
        var random = new Random();

        foreach (var kind in new[] { TargetKind.Fairy, TargetKind.Executioner })
        {
            foreach (var holder in Holders(kind))
            {
                if (Targets.Ledger.Has(kind, holder.PlayerId))
                {
                    continue;
                }

                var players = Candidates();
                var chosen = kind == TargetKind.Fairy
                    ? TargetRules.ChooseFairyTarget(players, holder.PlayerId, FairyEvilPercent(), random.Next)
                    : TargetRules.ChooseExecutionerTarget(players, holder.PlayerId, random.Next);

                if (chosen is { } id && GameData.Instance.GetPlayerById(id)?.Object is { } target)
                {
                    NeutralRpcs.RpcSetTarget(holder, (byte)kind, target);
                }
                else
                {
                    Logger<EclipsePlugin>.Warning($"Nobody can be the {kind} target of player {holder.PlayerId}.");
                    if (NoTargetHandlers.TryGetValue(kind, out var handler))
                    {
                        handler(holder);
                    }
                }
            }
        }
    }

    private static IEnumerable<PlayerControl> Holders(TargetKind kind)
    {
        var idPart = TargetRules.RoleIdOf(kind);
        var all = GameData.Instance.AllPlayers;
        for (var i = 0; i < all.Count; i++)
        {
            var info = all[i];
            if (info != null && !info.IsDead && !info.Disconnected && info.Object != null && info.Role is ICustomRole custom && custom.IdPart == idPart)
            {
                yield return info.Object;
            }
        }
    }

    private static List<TargetCandidate> Candidates()
    {
        var list = new List<TargetCandidate>();
        var all = GameData.Instance.AllPlayers;
        for (var i = 0; i < all.Count; i++)
        {
            var info = all[i];
            if (info == null || info.Disconnected || info.Role == null)
            {
                continue;
            }

            var (faction, alignment) = RoleClassification.Classify(info.Role);
            var idPart = info.Role is ICustomRole custom ? custom.IdPart : string.Empty;
            list.Add(new TargetCandidate(
                info.PlayerId,
                !info.IsDead,
                faction,
                alignment,
                idPart == "Executioner",
                idPart == "Fairy",
                Targets.Ledger.IsTarget(TargetKind.Executioner, info.PlayerId),
                Targets.Ledger.IsTarget(TargetKind.Fairy, info.PlayerId)));
        }

        return list;
    }
}
