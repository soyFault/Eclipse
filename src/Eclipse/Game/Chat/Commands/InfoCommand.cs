using System.Collections.Generic;
using Eclipse.Core;
using MiraAPI.Roles;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal sealed class InfoCommand : IChatCommand
{
    public string NamesKey => "Eclipse.Command.Info.Names";

    public string DescriptionKey => "Eclipse.Command.Info.Description";

    public bool HostOnly => false;

    public void Run(string argument)
    {
        if (argument.Length == 0)
        {
            ChatCommands.Say(MiraLocaleManager.Get("Eclipse.Command.Info.Usage"));
            return;
        }

        var roles = new List<RoleBehaviour>();
        var names = new List<string>();
        foreach (var role in RoleManager.Instance.AllRoles)
        {
            if (role is ICustomRole && !role.IsDead)
            {
                roles.Add(role);
                names.Add(role.GetRoleName());
            }
        }

        var index = ChatCommandParser.FindByName(names, argument);
        if (index < 0)
        {
            ChatCommands.Say(MiraLocaleManager.Get("Eclipse.Command.Info.NotFound").Replace("<name>", argument));
            return;
        }

        var found = roles[index];
        var custom = (ICustomRole)found;
        var color = ColorUtility.ToHtmlStringRGB(RoleLabels.ColorOf(found));
        ChatCommands.Say($"<color=#{color}>{found.GetRoleName()}</color>", custom.RoleLongDescription);
    }
}
