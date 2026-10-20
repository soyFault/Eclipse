using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.GameOptions;
using MiraAPI.Translation;
using UnityEngine;

namespace Eclipse.Game;

internal static class MedicReports
{

    private const double SameReportSeconds = 3;

    private static byte _lastVictimId = byte.MaxValue;
    private static double _lastReportTime = double.MinValue;

    [RegisterEvent]
    public static void OnReportBody(ReportBodyEvent report)
    {
        if (report.Target == null || !report.Reporter.AmOwner || report.Reporter.Data?.Role is not MedicRole)
        {
            return;
        }

        var options = OptionGroupSingleton<MedicOptions>.Instance;
        if (!options.ShowReports)
        {
            return;
        }

        var victim = report.Target;
        if (victim.PlayerId == _lastVictimId && KillLogEvents.Now - _lastReportTime < SameReportSeconds)
        {
            return;
        }

        _lastVictimId = victim.PlayerId;
        _lastReportTime = KillLogEvents.Now;

        var record = KillLogEvents.Log.LatestOf(victim.PlayerId);
        var age = record == null ? 0 : KillLogEvents.Now - record.Time;
        var killer = record == null ? null : GameData.Instance.GetPlayerById(record.KillerId);
        var tone = killer?.DefaultOutfit != null ? PlayerColorTone.Of(killer.DefaultOutfit.ColorId) : ColorTone.Darker;

        var kind = MedicReportRules.Decide(
            record != null,
            record != null && record.KillerId == record.VictimId,
            age,
            options.ReportColorDuration.Value,
            options.ReportNameDuration.Value,
            tone);

        var text = MiraLocaleManager.Get(LocaleKey(kind))
            .Replace("<player>", killer?.PlayerName ?? string.Empty)
            .Replace("<time>", Mathf.Round((float)age).ToString(System.Globalization.CultureInfo.InvariantCulture));

        ChatReports.Add(report.Reporter, MiraLocaleManager.Get("Eclipse.Role.Medic.Report.Title"), EclipseColors.Medic, text);
    }

    private static string LocaleKey(MedicReportKind kind)
    {
        return kind switch
        {
            MedicReportKind.TooOld => "Eclipse.Role.Medic.Report.TooOld",
            MedicReportKind.Suicide => "Eclipse.Role.Medic.Report.Suicide",
            MedicReportKind.KillerName => "Eclipse.Role.Medic.Report.KillerName",
            MedicReportKind.KillerLighter => "Eclipse.Role.Medic.Report.KillerLighter",
            MedicReportKind.KillerDarker => "Eclipse.Role.Medic.Report.KillerDarker",
            _ => "Eclipse.Role.Medic.Report.NoInformation",
        };
    }
}
