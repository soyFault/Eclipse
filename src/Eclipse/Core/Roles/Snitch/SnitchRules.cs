namespace Eclipse.Core;

internal enum SnitchStage
{
    Unrevealed = 0,

    Revealed = 1,

    CompletedTasks = 2,
}

internal enum SnitchReveal
{
    Self = 0,

    Everyone = 1,
}

internal static class SnitchRules
{
    public static SnitchStage StageFor(int completed, int total, int remainingWhenRevealed)
    {
        if (total <= 1)
        {
            return SnitchStage.Unrevealed;
        }

        if (completed >= total)
        {
            return SnitchStage.CompletedTasks;
        }

        return total - completed <= remainingWhenRevealed ? SnitchStage.Revealed : SnitchStage.Unrevealed;
    }

    public static SnitchStage Advance(SnitchStage current, int completed, int total, int remainingWhenRevealed)
    {
        var computed = StageFor(completed, total, remainingWhenRevealed);
        return computed > current ? computed : current;
    }

    public static bool SeesRedName(
        SnitchStage stage,
        bool snitchAlive,
        SnitchReveal reveal,
        bool viewerIsSnitch,
        bool targetIsImpostor,
        bool targetAlive,
        bool inMeeting,
        bool seesInMeetings,
        bool meetingCalledBySnitch)
    {
        if (stage != SnitchStage.CompletedTasks || !snitchAlive || !targetIsImpostor || !targetAlive)
        {
            return false;
        }

        if (viewerIsSnitch)
        {
            return !inMeeting || seesInMeetings;
        }

        return reveal == SnitchReveal.Everyone && inMeeting && meetingCalledBySnitch;
    }

    public static bool RevealedTo(SnitchStage stage, bool snitchAlive, bool viewerIsImpostor, bool viewerAlive)
    {
        return stage >= SnitchStage.Revealed && snitchAlive && viewerIsImpostor && viewerAlive;
    }
}
