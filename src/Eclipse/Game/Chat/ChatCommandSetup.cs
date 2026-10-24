namespace Eclipse.Game;

internal static class ChatCommandSetup
{
    public static void RegisterAll()
    {
        ChatCommands.Register(new HelpCommand());
        ChatCommands.Register(new SummaryCommand());
        ChatCommands.Register(new RolesCommand());
        ChatCommands.Register(new InfoCommand());
        ChatCommands.Register(new NameCommand());
        ChatCommands.Register(new KickCommand());
        ChatCommands.Register(new BanCommand());
    }
}
