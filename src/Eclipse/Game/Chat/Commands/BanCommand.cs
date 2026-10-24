namespace Eclipse.Game;

internal sealed class BanCommand() : RemovePlayerCommand(true)
{
    public override string NamesKey => "Eclipse.Command.Ban.Names";

    public override string DescriptionKey => "Eclipse.Command.Ban.Description";
}
