using Eclipse.Core;
using Eclipse.Modifiers;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Modifiers;

namespace Eclipse.Game;

internal static class SleuthEvents
{
    // The player who reports a body learns its role.
    [RegisterEvent]
    public static void OnReportBody(ReportBodyEvent report)
    {
        var sleuth = report.Reporter != null ? report.Reporter.GetModifier<SleuthModifier>() : null;
        if (UniversalRules.SleuthLearns(sleuth != null, report.Target != null))
        {
            sleuth!.Learn(report.Target!.PlayerId);
        }
    }

    // The role label of a reported player is shown to the Sleuth who reported them (RoleLabels asks this for everybody).
    public static bool SleuthSees(PlayerControl viewer, PlayerControl target)
    {
        var sleuth = viewer.GetModifier<SleuthModifier>();
        return sleuth != null && sleuth.Knows(target.PlayerId);
    }
}
