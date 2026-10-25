using AmongUs.GameOptions;
using Eclipse.Core;
using MiraAPI.Roles;
using Reactor.Utilities;

namespace Eclipse.Game;

internal static class BecomeRoles
{
    public static RoleTypes Resolve(BecomeOption option)
    {
        var id = BecomeRules.RoleIdOf(option);
        if (id == null)
        {
            return RoleTypes.Crewmate;
        }

        foreach (var role in CustomRoleManager.CustomRoleBehaviours)
        {
            if (role is ICustomRole custom && custom.IdPrefix == "Eclipse.Role" && custom.IdPart == id)
            {
                return role.Role;
            }
        }

        Logger<EclipsePlugin>.Warning($"The role '{id}' is not there yet: the player becomes a crewmate.");
        return RoleTypes.Crewmate;
    }

    // Used when a player turns into something else because of their role (a target that died). It happens in every game by itself.
    public static void Become(PlayerControl player, BecomeOption option)
    {
        RoleChanger.ChangeRole(player, Resolve(option));
    }
}
