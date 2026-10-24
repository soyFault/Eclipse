using System.Collections.Generic;
using Eclipse.Core;
using MiraAPI.Translation;

namespace Eclipse.Game;

internal sealed class NameCommand : IChatCommand
{
    public string NamesKey => "Eclipse.Command.Name.Names";

    public string DescriptionKey => "Eclipse.Command.Name.Description";

    public bool HostOnly => false;

    public void Run(string argument)
    {
        if (LobbyBehaviour.Instance == null)
        {
            ChatCommands.Say(MiraLocaleManager.Get("Eclipse.Command.Name.LobbyOnly"));
            return;
        }

        var wanted = NameSanitizer.Clean(argument).Trim();
        var inUse = new List<string>();
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player != null && player.Data != null && !player.AmOwner)
            {
                inUse.Add(player.Data.PlayerName);
            }
        }

        if (!ChatCommandParser.IsValidNewName(wanted, inUse))
        {
            ChatCommands.Say(MiraLocaleManager.Get("Eclipse.Command.Name.Invalid"));
            return;
        }

        PlayerControl.LocalPlayer.CmdCheckName(wanted);
        ChatCommands.Say(MiraLocaleManager.Get("Eclipse.Command.Name.Done").Replace("<name>", wanted));
    }
}
