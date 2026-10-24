using System.Collections.Generic;
using Eclipse.Core;
using MiraAPI.Translation;

namespace Eclipse.Game;

internal abstract class RemovePlayerCommand(bool ban) : IChatCommand
{
    public abstract string NamesKey { get; }

    public abstract string DescriptionKey { get; }

    public bool HostOnly => true;

    public void Run(string argument)
    {
        var players = new List<PlayerControl>();
        var names = new List<string>();
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player != null && player.Data != null && !player.AmOwner)
            {
                players.Add(player);
                names.Add(player.Data.PlayerName);
            }
        }

        var index = ChatCommandParser.FindByName(names, argument);
        if (index < 0)
        {
            ChatCommands.Say(MiraLocaleManager.Get("Eclipse.Command.Remove.NotFound").Replace("<name>", argument));
            return;
        }

        var target = players[index];
        AmongUsClient.Instance.KickPlayer(target.OwnerId, ban);
        ChatCommands.Say(MiraLocaleManager.Get(ban ? "Eclipse.Command.Ban.Done" : "Eclipse.Command.Kick.Done").Replace("<name>", target.Data.PlayerName));
    }
}
