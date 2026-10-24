namespace Eclipse.Game;

internal sealed class KickCommand() : RemovePlayerCommand(false)
{
    public override string NamesKey => "Eclipse.Command.Kick.Names";

    public override string DescriptionKey => "Eclipse.Command.Kick.Description";
}
