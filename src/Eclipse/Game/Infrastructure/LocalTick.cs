using System;
using System.Collections.Generic;

namespace Eclipse.Game;

internal static class LocalTick
{
    private static readonly List<Action<float>> Ticks = [];

    public static void Register(Action<float> tick)
    {
        Ticks.Add(tick);
    }
    public static void Run(float deltaTime)
    {
        foreach (var tick in Ticks)
        {
            FaultGuard.Run("A local tick", () => tick(deltaTime));
        }
    }
}
