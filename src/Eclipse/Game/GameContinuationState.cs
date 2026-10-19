using Eclipse.Api.Factions;
using Eclipse.Api.Roles;
using Eclipse.Core;
using Eclipse.Roles;
using Reactor.Utilities;

namespace Eclipse.Game;

internal static class GameContinuationState
{
    private static bool? _lastHeld;
    private static int _lastScope;
    public static (int Impostors, int Others, int Halters, int NeutralKillers, bool Held) Evaluate()
    {
        var impostors = 0;
        var others = 0;
        var halters = 0;
        var neutralKillers = 0;

        if (GameData.Instance)
        {

            var players = GameData.Instance.AllPlayers;
            for (var i = 0; i < players.Count; i++)
            {
                var info = players[i];
                if (info == null || info.IsDead || info.Disconnected || info.Role == null)
                {
                    continue;
                }

                var (faction, alignment) = RoleClassification.Classify(info.Role);
                if (faction == Faction.Impostor)
                {
                    impostors++;
                    continue;
                }

                if (alignment == AlignmentIds.NeutralKilling)
                {
                    neutralKillers++;
                }

                others++;
                if (info.Role is IPreventsGameEnd { PreventsGameEnd: true })
                {
                    halters++;
                }
            }
        }

        return (impostors, others, halters, neutralKillers, GameContinuation.IsDeathEndHeld(impostors, others, halters, neutralKillers));
    }

    public static void LogIfChanged((int Impostors, int Others, int Halters, int NeutralKillers, bool Held) state)
    {
        var scope = GameData.Instance ? GameData.Instance.GetInstanceID() : 0;
        if (scope != _lastScope)
        {
            _lastScope = scope;
            _lastHeld = null;
        }

        if (_lastHeld == state.Held)
        {
            return;
        }

        var first = _lastHeld == null;
        _lastHeld = state.Held;
        if (first && !state.Held)
        {
            return;
        }

        Logger<EclipsePlugin>.Info(
            $"Game-continues rule: end by deaths {(state.Held ? "HELD BACK" : "released")} " +
            $"(impostors {state.Impostors}, others {state.Others}, roles that prevent the game end {state.Halters}, neutral killers {state.NeutralKillers}).");
    }
}
