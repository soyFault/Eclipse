using MiraAPI.Translation;

namespace Eclipse.Game;

internal sealed class SummaryCommand : IChatCommand
{
    public string NamesKey => "Eclipse.Command.Summary.Names";

    public string DescriptionKey => "Eclipse.Command.Summary.Description";

    public bool HostOnly => false;

    public void Run(string argument)
    {
        ChatCommands.Say(SummaryEvents.LastText.Length == 0 ? MiraLocaleManager.Get("Eclipse.Command.Summary.None") : SummaryEvents.LastText);
    }
}
