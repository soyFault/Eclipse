using System.Collections.Generic;
using Eclipse.Api.Factions;
using Eclipse.Core;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles;
using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Translation;
using UnityEngine;

namespace Eclipse.Game;

internal static class SnitchEvents
{
    [RegisterEvent]
    public static void OnCompleteTask(CompleteTaskEvent task)
    {
        if (task.Player?.Data?.Role is SnitchRole snitch)
        {
            snitch.CheckTasks();
        }
    }

    public static void Tick(float deltaTime)
    {
        foreach (var snitch in new List<SnitchRole>(CustomRoleUtils.GetActiveRolesOfType<SnitchRole>()))
        {
            if (snitch.Player != null)
            {
                snitch.Tick(deltaTime);
            }
        }
    }

    public static Color? NameColor(PlayerControl viewer, PlayerControl target)
    {
        if (viewer.Data.Role is not SnitchRole snitch || target.Data?.Role == null)
        {
            return null;
        }

        var red = SnitchRules.SeesRedName(
            snitch.Stage,
            !viewer.Data.IsDead,
            RoleClassification.Classify(target.Data.Role).Faction == Faction.Impostor,
            !target.Data.IsDead && !target.Data.Disconnected,
            MeetingHud.Instance != null,
            OptionGroupSingleton<SnitchOptions>.Instance.SeesImpostorsInMeetings);
        return red ? Palette.ImpostorRed : null;
    }

    public static bool SnitchRoleIsPublic(PlayerControl viewer, PlayerControl target)
    {
        if (target.Data?.Role is not SnitchRole snitch || viewer.Data?.Role == null)
        {
            return false;
        }

        return SnitchRules.RevealedTo(
            snitch.Stage,
            !target.Data.IsDead,
            RoleClassification.Classify(viewer.Data.Role).Faction == Faction.Impostor,
            !viewer.Data.IsDead);
    }
}
