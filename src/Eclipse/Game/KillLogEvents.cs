using Eclipse.Core;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using UnityEngine;

namespace Eclipse.Game;

internal static class KillLogEvents
{
    public static KillLog Log { get; } = new();

    public static double Now => Time.timeAsDouble;

    [RegisterEvent]
    public static void OnAfterMurder(AfterMurderEvent murder)
    {
        Log.Add(murder.Source.PlayerId, murder.Target.PlayerId, Now);
    }

    [RegisterEvent(-1)]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        if (roundStart.TriggeredByIntro)
        {
            Log.Clear();
        }
    }
}
