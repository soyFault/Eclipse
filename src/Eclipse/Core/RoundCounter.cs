namespace Eclipse.Core;

internal sealed class RoundCounter
{
    public int Current { get; private set; } = 1;

    public bool IsFirstRound => Current == 1;

    public void OnRoundStart(bool triggeredByIntro)
    {
        Current = triggeredByIntro ? 1 : Current + 1;
    }
}
