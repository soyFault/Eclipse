using AmongUs.GameOptions;
using UnityEngine;

namespace Eclipse.Game;

internal static class RoleChanger
{
    public static void ChangeRole(PlayerControl player, RoleTypes newRoleType)
    {
        var data = player.Data;
        if (data == null)
        {
            return;
        }

        player.roleAssigned = false;

        if (data.Role)
        {
            data.Role.Deinitialize(player);
        }

        var template = RoleManager.Instance.GetRole(newRoleType);
        var role = Object.Instantiate(template, data.gameObject.transform);
        role.Initialize(player);

        if (player.AmOwner && HudManager.Instance)
        {
            HudManager.Instance.SetHudActive(player, role, true);

            if (MeetingHud.Instance || ExileController.Instance || IntroCutscene.Instance)
            {
                HudManager.Instance.SetHudActive(player, role, false);
            }
        }

        data.Role = role;
        data.RoleType = role.Role;

        if (!role.IsDead)
        {
            data.RoleWhenAlive = new Il2CppSystem.Nullable<RoleTypes>(role.Role);
        }

        role.AdjustTasks(player);
        data.Role.SpawnTaskHeader(player);
    }
}
