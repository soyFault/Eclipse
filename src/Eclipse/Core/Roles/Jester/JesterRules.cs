namespace Eclipse.Core;

internal enum JestWin
{
    EndsGame = 0,
    Haunts = 1,
    Nothing = 2,
}

internal static class JesterRules
{
    // The Jester's win ends the match right away only with this setting; with the other two the match goes on and the Jester is a winner at the end.
    public static bool EndsGame(JestWin mode)
    {
        return mode == JestWin.EndsGame;
    }

    // The haunt (one kill from beyond the grave, on somebody who voted for the Jester) opens only with "Haunts" and when someone alive voted for them.
    public static bool OpensHaunt(JestWin mode, int aliveVoters)
    {
        return mode == JestWin.Haunts && aliveVoters > 0;
    }

    // Everyone else is told about the win only when the match goes on and the setting asks for it.
    public static bool AnnouncesWin(JestWin mode, bool announceSetting)
    {
        return mode != JestWin.EndsGame && announceSetting;
    }

    // The Haunt button: while the haunt is open and unused, never in a meeting.
    public static bool CanHaunt(bool open, bool alreadyHaunted, bool inMeeting)
    {
        return open && !alreadyHaunted && !inMeeting;
    }
}
