using Eclipse.Core;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;

namespace Eclipse.Game;

internal static class MatchRounds
{
    private static readonly RoundCounter Counter = new();

    public static int Current => Counter.Current;

    public static bool IsFirstRound => Counter.IsFirstRound;

    public static bool IsFirstRoundLocked(bool allowedInFirstRound)
    {
        return FirstRoundRule.IsLocked(allowedInFirstRound, Counter.IsFirstRound, TutorialManager.InstanceExists);
    }

    [RegisterEvent(-1)]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        Counter.OnRoundStart(roundStart.TriggeredByIntro);
    }
}
