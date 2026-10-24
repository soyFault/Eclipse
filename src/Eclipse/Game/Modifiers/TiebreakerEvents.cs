using System.Linq;
using Eclipse.Modifiers;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting.Voting;
using MiraAPI.Modifiers;
using MiraAPI.Voting;

namespace Eclipse.Game;

internal static class TiebreakerEvents
{
    // In a tie, the vote of the tiebreaker counts twice.
    [RegisterEvent]
    public static void OnProcessVotes(ProcessVotesEvent votes)
    {
        if (votes.ExiledPlayer != null)
        {
            return;
        }

        VotingUtils.GetExiled(votes.Votes, out var isTie);
        if (!isTie)
        {
            return;
        }

        var tiebreaker = ModifierUtils.GetPlayersWithModifier<TiebreakerModifier>().FirstOrDefault();
        if (tiebreaker == null)
        {
            return;
        }

        var all = votes.Votes.ToList();
        var vote = all.FirstOrDefault(candidate => candidate.Voter == tiebreaker.PlayerId);
        if (vote == default)
        {
            return;
        }

        all.Add(new CustomVote(vote.Voter, vote.Suspect));
        votes.ExiledPlayer = VotingUtils.GetExiled(all, out _);
    }
}
