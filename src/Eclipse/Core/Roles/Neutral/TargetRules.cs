using System;
using System.Collections.Generic;
using System.Linq;
using Eclipse.Api.Factions;

namespace Eclipse.Core;

internal readonly record struct TargetCandidate(
    byte Id,
    bool Alive,
    Faction Faction,
    string? AlignmentId,
    bool IsExecutioner,
    bool IsFairy,
    bool IsExecutionerTarget,
    bool IsFairyTarget);

internal static class TargetRules
{
    public static bool CanBeExecutionerTarget(TargetCandidate candidate, byte holderId)
    {
        return candidate.Alive
               && candidate.Id != holderId
               && candidate.Faction == Faction.Crewmate
               && candidate.AlignmentId != AlignmentIds.CrewPower
               && !candidate.IsFairyTarget;
    }

    public static bool CanBeFairyTarget(TargetCandidate candidate, byte holderId)
    {
        return candidate.Alive
               && candidate.Id != holderId
               && !candidate.IsFairy
               && candidate.AlignmentId != AlignmentIds.NeutralOutlier
               && candidate.AlignmentId != AlignmentIds.NeutralEvil
               && !candidate.IsExecutionerTarget;
    }

    public static bool IsEvil(TargetCandidate candidate)
    {
        return candidate.Faction == Faction.Impostor || candidate.AlignmentId == AlignmentIds.NeutralKilling;
    }

    public static byte? ChooseExecutionerTarget(IEnumerable<TargetCandidate> players, byte holderId, Func<int, int> next)
    {
        var pool = players.Where(p => CanBeExecutionerTarget(p, holderId)).ToList();
        return pool.Count == 0 ? null : pool[next(pool.Count)].Id;
    }

    public static byte? ChooseFairyTarget(IEnumerable<TargetCandidate> players, byte holderId, int evilPercent, Func<int, int> next)
    {
        var allowed = players.Where(p => CanBeFairyTarget(p, holderId)).ToList();
        var wantsEvil = evilPercent > 0 && next(100) + 1 <= evilPercent;
        var pool = wantsEvil ? allowed.Where(IsEvil).ToList() : [];
        if (pool.Count == 0)
        {
            pool = allowed.Where(p => p.Faction == Faction.Crewmate).ToList();
        }

        if (pool.Count == 0)
        {
            pool = allowed;
        }

        return pool.Count == 0 ? null : pool[next(pool.Count)].Id;
    }

    public static string RoleIdOf(TargetKind kind)
    {
        return kind == TargetKind.Executioner ? "Executioner" : "Fairy";
    }
}
