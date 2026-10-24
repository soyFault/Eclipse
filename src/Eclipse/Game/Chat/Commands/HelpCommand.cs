using System.Text;
using MiraAPI.Translation;

namespace Eclipse.Game;

internal sealed class HelpCommand : IChatCommand
{
    public string NamesKey => "Eclipse.Command.Help.Names";

    public string DescriptionKey => "Eclipse.Command.Help.Description";

    public bool HostOnly => false;

    public void Run(string argument)
    {
        var builder = new StringBuilder(MiraLocaleManager.Get("Eclipse.Command.Help.Title"));
        var isHost = AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost;
        foreach (var command in ChatCommands.All)
        {
            if (command.HostOnly && !isHost)
            {
                continue;
            }

            builder.Append("\n/").Append(ChatCommands.MainName(command)).Append(" - ").Append(MiraLocaleManager.Get(command.DescriptionKey));
        }

        ChatCommands.Say(builder.ToString());
    }
}
