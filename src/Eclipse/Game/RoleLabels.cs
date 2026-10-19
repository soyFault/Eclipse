using System;
using System.Collections.Generic;
using Eclipse.Api.Factions;
using Eclipse.Core;
using Eclipse.Roles;
using MiraAPI.LocalSettings;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using AmongUs.GameOptions;
using UnityEngine;

namespace Eclipse.Game;

internal static class RoleLabels
{
    private static readonly List<RevealProvider> Reveals = [];
    private static readonly Dictionary<RoleTypes, string> IconTags = [];


    public delegate bool RevealProvider(PlayerControl viewer, PlayerControl target);

    public static RoleNameStyle Style
    {
        get
        {
            try
            {
                return LocalSettingsTabSingleton<EclipseLocalTabPlayers>.Instance.RoleNameStyle.Value;
            }
            catch (Exception)
            {
                return RoleNameStyle.TopSmall;
            }
        }
    }

    public static void RegisterReveal(RevealProvider provider)
    {
        Reveals.Add(provider);
    }

    public static string? Line(PlayerControl viewer, PlayerControl target)
    {
        var role = RoleToShow(viewer, target);
        if (role == null)
        {
            return null;
        }

        var color = ColorOf(role);
        return RoleLabelRules.FormatLine(role.GetRoleName(), ColorUtility.ToHtmlStringRGB(color), IconTag(role), Style);
    }

    public static Color? NameColor(PlayerControl viewer, PlayerControl target)
    {
        if (!ColorPlayerNames)
        {
            return null;
        }

        var role = RoleToShow(viewer, target);
        return role == null ? null : ColorOf(role);
    }

    private static bool InGame =>
        ShipStatus.Instance != null && (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started || TutorialManager.InstanceExists);

    private static bool ColorPlayerNames
    {
        get
        {
            try
            {
                return LocalSettingsTabSingleton<EclipseLocalTabPlayers>.Instance.ColorPlayerNames.Value;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    private static bool ShowIcons
    {
        get
        {
            try
            {
                return LocalSettingsTabSingleton<EclipseLocalTabPlayers>.Instance.ShowRoleIcons.Value;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }

    private static RoleBehaviour? RoleToShow(PlayerControl viewer, PlayerControl target)
    {
        if (viewer.Data?.Role == null || target.Data?.Role == null || !InGame)
        {
            return null;
        }

        var bothImpostors = IsImpostor(viewer.Data.Role) && IsImpostor(target.Data.Role);
        if (!RoleLabelRules.Shows(target == viewer, viewer.Data.IsDead, bothImpostors, IsRevealed(viewer, target)))
        {
            return null;
        }

        if (target.Data.IsDead && target.Data.RoleWhenAlive.HasValue)
        {
            return RoleManager.Instance.GetRole(target.Data.RoleWhenAlive.Value);
        }

        return target.Data.Role;
    }

    internal static bool IsPublicToLocal(PlayerControl target)
    {
        return PlayerControl.LocalPlayer != null && IsRevealed(PlayerControl.LocalPlayer, target);
    }

    private static bool IsRevealed(PlayerControl viewer, PlayerControl target)
    {
        foreach (var reveal in Reveals)
        {
            try
            {
                if (reveal(viewer, target))
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
        }

        return false;
    }

    private static bool IsImpostor(RoleBehaviour role)
    {
        return RoleClassification.Classify(role).Faction == Faction.Impostor;
    }

    internal static Color ColorOf(RoleBehaviour role)
    {
        return role is ICustomRole custom ? custom.RoleColor : role.TeamColor;
    }

    private static string IconTag(RoleBehaviour role)
    {
        if (!ShowIcons || role is not ICustomRole custom)
        {
            return string.Empty;
        }

        if (!IconTags.TryGetValue(role.Role, out var tag))
        {
            var icon = custom.Configuration.IconTmp;
            tag = icon == null ? string.Empty : $"<sprite name=\"{icon.name}\">";
            IconTags[role.Role] = tag;
        }

        return tag;
    }
}
