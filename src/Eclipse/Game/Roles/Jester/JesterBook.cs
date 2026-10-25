using System.Collections.Generic;
using System.Linq;
using Eclipse.Roles.Neutral;
using MiraAPI.Roles;
using MiraAPI.Voting;

namespace Eclipse.Game;

internal static class JesterBook
{
    private sealed class Entry
    {
        public List<byte> Voters { get; } = [];

        public bool HauntOpen { get; set; }

        public bool Haunted { get; set; }
    }

    private static readonly Dictionary<byte, Entry> Entries = [];

    public static void RecordVoters(IEnumerable<CustomVote> votes)
    {
        var all = votes.ToList();
        foreach (var jester in CustomRoleUtils.GetActiveRolesOfType<JesterRole>())
        {
            if (jester.Player == null)
            {
                continue;
            }

            var entry = Get(jester.Player.PlayerId);
            entry.Voters.Clear();
            foreach (var vote in all)
            {
                if (vote.Suspect == jester.Player.PlayerId && !entry.Voters.Contains(vote.Voter))
                {
                    entry.Voters.Add(vote.Voter);
                }
            }
        }
    }

    public static int AliveVoterCount(byte jesterId)
    {
        return AliveVoters(jesterId).Count;
    }

    public static List<byte> AliveVoters(byte jesterId)
    {
        if (!Entries.TryGetValue(jesterId, out var entry))
        {
            return [];
        }

        return entry.Voters
            .Where(id => GameData.Instance?.GetPlayerById(id) is { IsDead: false, Disconnected: false })
            .ToList();
    }

    public static void OpenHaunt(byte jesterId, bool open)
    {
        Get(jesterId).HauntOpen = open;
    }

    public static bool IsHauntOpen(byte jesterId)
    {
        return Entries.TryGetValue(jesterId, out var entry) && entry.HauntOpen;
    }

    public static bool HasHaunted(byte jesterId)
    {
        return Entries.TryGetValue(jesterId, out var entry) && entry.Haunted;
    }

    public static void MarkHaunted(byte jesterId)
    {
        var entry = Get(jesterId);
        entry.Haunted = true;
        entry.HauntOpen = false;
    }

    public static void CloseAllHaunts()
    {
        foreach (var entry in Entries.Values)
        {
            entry.HauntOpen = false;
        }
    }

    public static void Reset()
    {
        Entries.Clear();
    }

    private static Entry Get(byte jesterId)
    {
        if (!Entries.TryGetValue(jesterId, out var entry))
        {
            entry = new Entry();
            Entries[jesterId] = entry;
        }

        return entry;
    }
}
