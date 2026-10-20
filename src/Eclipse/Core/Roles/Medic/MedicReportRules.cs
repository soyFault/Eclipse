namespace Eclipse.Core;

internal enum MedicReportKind
{
    NoInformation,
    TooOld,
    Suicide,
    KillerName,
    KillerLighter,
    KillerDarker,
}

internal static class MedicReportRules
{
    public static MedicReportKind Decide(bool killRecorded, bool isSuicide, double ageSeconds, float colorWindowSeconds, float nameWindowSeconds, ColorTone killerTone)
    {
        if (!killRecorded)
        {
            return MedicReportKind.NoInformation;
        }

        if (colorWindowSeconds > 0 && ageSeconds > colorWindowSeconds)
        {
            return MedicReportKind.TooOld;
        }

        if (isSuicide)
        {
            return MedicReportKind.Suicide;
        }

        if (ageSeconds < nameWindowSeconds)
        {
            return MedicReportKind.KillerName;
        }

        return killerTone == ColorTone.Lighter ? MedicReportKind.KillerLighter : MedicReportKind.KillerDarker;
    }
}
