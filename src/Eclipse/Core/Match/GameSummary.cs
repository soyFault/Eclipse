using System.Collections.Generic;
using System.Text;

namespace Eclipse.Core;

internal enum PlayerFate
{
    Alive = 0,

    Killed = 1,

    Exiled = 2,

    Disconnected = 3,

    Died = 4,
}

internal enum EndSummaryMode
{
    Hidden = 0,

    Left = 1,
}

internal sealed record DeathNote(PlayerFate Fate, byte KillerId, int Round);

internal sealed class DeathBook
{
    public const byte NoKiller = byte.MaxValue;

    private readonly Dictionary<byte, DeathNote> _notes = [];

    public int Count => _notes.Count;

    public void Record(byte playerId, PlayerFate fate, byte killerId, int round)
    {
        _notes.TryAdd(playerId, new DeathNote(fate, killerId, round));
    }

    public DeathNote? Get(byte playerId)
    {
        return _notes.TryGetValue(playerId, out var note) ? note : null;
    }

    public void Clear()
    {
        _notes.Clear();
    }
}

internal sealed record SummaryLine(
    string Name,
    string NameColor,
    string RoleName,
    string RoleColor,
    PlayerFate Fate,
    string? KillerName,
    int Round,
    int TasksDone,
    int TasksTotal,
    bool Winner,
    string RoleIcon = "");

internal sealed record SummaryLabels(string Alive, string Killed, string Exiled, string Disconnected, string Died, string Winner);

internal static class SummaryFormatter
{
    public static string Line(SummaryLine line, SummaryLabels labels)
    {
        var builder = new StringBuilder();
        if (line.Winner)
        {
            builder.Append("<color=#FFD700>").Append(labels.Winner).Append("</color> ");
        }

        builder.Append("<color=#").Append(line.NameColor).Append('>').Append(line.Name).Append("</color> - ");
        builder.Append(line.RoleIcon.Length > 0 ? line.RoleIcon + " " : string.Empty);
        builder.Append("<color=#").Append(line.RoleColor).Append('>').Append(line.RoleName).Append("</color> - ");
        builder.Append(FateText(line, labels));
        if (line.TasksTotal > 0)
        {
            builder.Append(" <alpha=#AA>").Append(line.TasksDone).Append('/').Append(line.TasksTotal).Append("<alpha=#FF>");
        }

        return builder.ToString();
    }

    public static float FontSize(int lines)
    {
        const float roomInLines = 330f / 28f;
        return System.Math.Clamp(roomInLines / System.Math.Max(lines, 1), MinFontSize, MaxFontSize);
    }

    public const float MinFontSize = 0.7f;

    public const float MaxFontSize = 1.2f;

    public static string Build(IReadOnlyList<SummaryLine> lines, SummaryLabels labels, int maxLines)
    {
        var builder = new StringBuilder();
        var shown = 0;
        foreach (var line in lines)
        {
            if (shown == maxLines)
            {
                builder.Append("<alpha=#AA>… +").Append(lines.Count - shown).Append("<alpha=#FF>");
                break;
            }

            builder.Append(Line(line, labels)).Append('\n');
            shown++;
        }

        return builder.ToString().TrimEnd('\n');
    }
    public static int Compare(SummaryLine a, SummaryLine b)
    {
        var byWinner = b.Winner.CompareTo(a.Winner);
        if (byWinner != 0)
        {
            return byWinner;
        }

        var byAlive = (b.Fate == PlayerFate.Alive).CompareTo(a.Fate == PlayerFate.Alive);
        if (byAlive != 0)
        {
            return byAlive;
        }

        var byRound = a.Round.CompareTo(b.Round);
        return byRound != 0 ? byRound : string.CompareOrdinal(a.Name, b.Name);
    }

    private static string FateText(SummaryLine line, SummaryLabels labels)
    {
        return line.Fate switch
        {
            PlayerFate.Alive => labels.Alive,
            PlayerFate.Killed => line.KillerName is { Length: > 0 }
                ? $"{labels.Killed} <alpha=#AA>({line.KillerName}, {Round(line)})<alpha=#FF>"
                : $"{labels.Killed} <alpha=#AA>({Round(line)})<alpha=#FF>",
            PlayerFate.Exiled => $"{labels.Exiled} <alpha=#AA>({Round(line)})<alpha=#FF>",
            PlayerFate.Disconnected => labels.Disconnected,
            _ => $"{labels.Died} <alpha=#AA>({Round(line)})<alpha=#FF>",
        };
    }

    private static string Round(SummaryLine line)
    {
        return "R" + line.Round;
    }
}
