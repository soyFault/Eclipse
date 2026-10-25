using System;
using System.Collections.Generic;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;

namespace Eclipse.Game;

internal static class MatchReset
{
    private static readonly List<Action> Actions = [];

    public static void Register(Action reset)
    {
        Actions.Add(reset);
    }

    public static void Run()
    {
        foreach (var action in Actions)
        {
            FaultGuard.Run("match-reset", action);
        }
    }

    [RegisterEvent]
    public static void OnGameEnd(GameEndEvent gameEnd)
    {
        Run();
    }

    [RegisterEvent(-2)]
    public static void OnFreeplayStart(RoundStartEvent roundStart)
    {
        if (roundStart.TriggeredByIntro && TutorialManager.InstanceExists)
        {
            Run();
        }
    }
}
