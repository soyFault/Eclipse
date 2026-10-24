using System.Collections.Generic;
using Eclipse.Core;
using MiraAPI.Translation;

namespace Eclipse.Game;

internal interface IChatCommand
{
    string NamesKey { get; }

    string DescriptionKey { get; }

    bool HostOnly { get; }

    void Run(string argument);
}

internal static class ChatCommands
{
    private static readonly List<IChatCommand> Commands = [];

    public static IReadOnlyList<IChatCommand> All => Commands;

    public static void Register(IChatCommand command)
    {
        Commands.Add(command);
    }

    public static bool TryRun(string? line)
    {
        var parsed = ChatCommandParser.Parse(line);
        if (!parsed.IsCommand)
        {
            return false;
        }

        foreach (var command in Commands)
        {
            if (!ChatCommandParser.Matches(parsed.Name, NamesOf(command)))
            {
                continue;
            }

            if (command.HostOnly && !(AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost))
            {
                Say(MiraLocaleManager.Get("Eclipse.Command.HostOnly"));
            }
            else
            {
                FaultGuard.Run("Chat command", () => command.Run(parsed.Argument));
            }

            return true;
        }

        Say(MiraLocaleManager.Get("Eclipse.Command.Unknown"));
        return true;
    }

    public static string MainName(IChatCommand command)
    {
        var names = NamesOf(command);
        return names.Count > 0 ? names[0] : "?";
    }

    public static List<string> NamesOf(IChatCommand command)
    {
        var names = ChatCommandParser.Split(MiraLocaleManager.Get(command.NamesKey));
        if (MiraLocaleManager.CurrentLanguage is not MiraLanguage.English)
        {
            names.AddRange(ChatCommandParser.Split(MiraLocaleManager.Get(MiraLanguage.English, command.NamesKey)));
        }

        return names;
    }

    // A message only the player who typed the command sees, as a system bubble in the chat.
    public static void Say(string message)
    {
        Say(MiraLocaleManager.Get("Eclipse.Chat.System"), message);
    }

    public static void Say(string title, string message)
    {
        FaultGuard.Run("System chat message", () => SystemChat.Add(title, message));
    }
}
