using System.Collections.Generic;
using Eclipse.Core;
using Eclipse.GameOver;
using MiraAPI.GameEnd;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal readonly record struct NeutralWinner(string RoleName, Color Color);

internal static class NeutralWinners
{
    public static WinnerBoard<NeutralWinner> Board { get; } = new();

    public static WinnerBoard<NeutralWinner> Early { get; } = new();

    public static void Declare(PlayerControl player, string roleName, Color roleColor)
    {
        if (Board.Declare(player.PlayerId, new NeutralWinner(roleName, roleColor)))
        {
            Logger<EclipsePlugin>.Info($"Neutral win declared for player {player.PlayerId} ({roleName}).");
        }
    }

    public static void DeclareEarly(PlayerControl player, string roleName, Color roleColor)
    {
        if (Early.Declare(player.PlayerId, new NeutralWinner(roleName, roleColor)))
        {
            Logger<EclipsePlugin>.Info($"Early neutral win declared for player {player.PlayerId} ({roleName}).");
        }
    }

    public static void AddEarlyWinnersTo(Il2CppSystem.Collections.Generic.List<CachedPlayerData> winners)
    {
        if (!GameData.Instance)
        {
            return;
        }

        foreach (var id in Early.Ids)
        {
            var info = GameData.Instance.GetPlayerById(id);
            if (info == null)
            {
                continue;
            }

            var present = false;
            for (var i = 0; i < winners.Count; i++)
            {
                if (winners[i].PlayerName == info.PlayerName)
                {
                    present = true;
                    break;
                }
            }

            if (!present)
            {

                winners.Add(new CachedPlayerData(info) { IsDead = false });
            }
        }
    }

    public static void Reset()
    {
        Board.Clear();
        Early.Clear();
    }

    public static void EndMatch()
    {
        var ids = new List<byte>(Board.Ids);
        Board.MarkEnded();

        var winners = new List<NetworkedPlayerInfo>();
        if (GameData.Instance)
        {
            foreach (var id in ids)
            {
                var found = GameData.Instance.GetPlayerById(id);
                if (found != null)
                {
                    winners.Add(found);
                }
            }
        }

        if (winners.Count == 0)
        {
            Logger<EclipsePlugin>.Warning("A neutral win was declared but its player is gone; the match goes on.");
            return;
        }

        Logger<EclipsePlugin>.Info($"Ending the match: neutral win of {winners.Count} player(s), the first is player {winners[0].PlayerId}.");
        CustomGameOver.Trigger<NeutralGameOver>(winners.ToArray());
    }
}
