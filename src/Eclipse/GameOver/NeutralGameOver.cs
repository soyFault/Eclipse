using Eclipse.Api.Factions;
using Eclipse.Game;
using MiraAPI.GameEnd;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using UnityEngine;

namespace Eclipse.GameOver;

internal sealed class NeutralGameOver : CustomGameOver
{
    private NetworkedPlayerInfo[] _winners = [];
    private string _roleName = MiraLocaleManager.Get("MiraApi.RoleTeam.Neutral");
    private Color _color = FactionColors.Neutral;

    public override bool VerifyCondition(PlayerControl playerControl, NetworkedPlayerInfo[] winners)
    {
        if (winners.Length == 0)
        {
            return false;
        }

        _winners = winners;

        if (NeutralWinners.Board.Contains(winners[0].PlayerId))
        {
            var known = NeutralWinners.Board.Find(winners[0].PlayerId);
            _roleName = known.RoleName;
            _color = known.Color;
        }

        return true;
    }

    public override bool BeforeEndGameSetup(EndGameManager endGameManager)
    {
        EndGameResult.CachedWinners.Clear();
        foreach (var winner in _winners)
        {
            EndGameResult.CachedWinners.Add(new CachedPlayerData(winner) { IsDead = false });
        }

        NeutralWinners.AddEarlyWinnersTo(EndGameResult.CachedWinners);
        return true;
    }

    public override void AfterEndGameSetup(EndGameManager endGameManager)
    {
        endGameManager.BackgroundBar.material.SetColor(ShaderID.Color, _color);
        endGameManager.WinText.text = MiraLocaleManager.Get("Eclipse.GameOver.SoloWin").Replace("<role>", _roleName);
        endGameManager.WinText.color = _color;
    }
}
