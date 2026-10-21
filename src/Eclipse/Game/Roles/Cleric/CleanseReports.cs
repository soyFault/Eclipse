using System.Linq;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Translation;

namespace Eclipse.Game;

internal static class CleanseReports
{
    private static readonly CleanseBook Book = new();

    // Runs on every game (the message arrives for everyone): the effects come off the player everywhere, and only the Cleric keeps the report.
    public static void Apply(PlayerControl cleric, PlayerControl target)
    {
        var found = NegativeEffects.On(target);
        foreach (var effect in found)
        {
            effect.Remove(target);
        }

        if (!cleric.AmOwner)
        {
            return;
        }

        Book.Add(target.PlayerId, found.Select(e => e.NameKey).ToList());
        if (found.Count > 0)
        {
            ScreenFlash.Flash(EclipseColors.Cleric);
        }
    }

    [RegisterEvent]
    public static void OnStartMeeting(StartMeetingEvent meeting)
    {
        var entries = Book.Drain();
        if (PlayerControl.LocalPlayer?.Data?.Role is not ClericRole)
        {
            return;
        }

        var title = MiraLocaleManager.Get("Eclipse.Role.Cleric.Report.Title");
        foreach (var entry in entries)
        {
            var name = GameData.Instance.GetPlayerById(entry.TargetId)?.PlayerName ?? string.Empty;
            var text = entry.Effects.Count == 0
                ? MiraLocaleManager.Get("Eclipse.Role.Cleric.Report.None").Replace("<player>", name)
                : MiraLocaleManager.Get("Eclipse.Role.Cleric.Report.Found").Replace("<player>", name)
                  + " " + string.Join(", ", entry.Effects.Select(key => MiraLocaleManager.Get(key)));
            ChatReports.Add(PlayerControl.LocalPlayer, title, EclipseColors.Cleric, text);
        }
    }

    [RegisterEvent(-1)]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        if (roundStart.TriggeredByIntro)
        {
            Book.Clear();
        }
    }
}
