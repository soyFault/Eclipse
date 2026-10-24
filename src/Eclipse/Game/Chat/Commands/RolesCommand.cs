using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eclipse.Api.Factions;
using Eclipse.Roles;
using MiraAPI.Roles;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal sealed class RolesCommand : IChatCommand
{
    public string NamesKey => "Eclipse.Command.Roles.Names";

    public string DescriptionKey => "Eclipse.Command.Roles.Description";

    public bool HostOnly => false;

    public void Run(string argument)
    {
        var options = GameOptionsManager.Instance.CurrentGameOptions.RoleOptions;
        var enabled = new List<RoleBehaviour>();
        foreach (var role in RoleManager.Instance.AllRoles)
        {
            if (role != null && !role.IsDead && role is ICustomRole && options.GetNumPerGame(role.Role) > 0)
            {
                enabled.Add(role);
            }
        }

        if (enabled.Count == 0)
        {
            ChatCommands.Say(MiraLocaleManager.Get("Eclipse.Command.Roles.None"));
            return;
        }

        var builder = new StringBuilder();
        foreach (var faction in new[] { Faction.Crewmate, Faction.Impostor, Faction.Neutral })
        {
            var names = enabled
                .Where(role => RoleClassification.Classify(role).Faction == faction)
                .OrderBy(role => role.GetRoleName())
                .Select(role => $"<color=#{ColorUtility.ToHtmlStringRGB(RoleLabels.ColorOf(role))}>{role.GetRoleName()}</color>")
                .ToList();
            if (names.Count > 0)
            {
                builder.Append(builder.Length > 0 ? "\n" : string.Empty)
                    .Append(MiraLocaleManager.Get("Eclipse.Faction." + faction)).Append(" (").Append(names.Count).Append("): ")
                    .Append(string.Join(", ", names));
            }
        }

        ChatCommands.Say(builder.ToString());
    }
}
