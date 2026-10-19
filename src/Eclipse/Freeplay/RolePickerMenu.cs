using System;
using System.Collections.Generic;
using Eclipse.Core;
using Eclipse.Ui;
using MiraAPI.Hud;
using MiraAPI.Roles;
using MiraAPI.Utilities;

namespace Eclipse.Freeplay;

internal sealed class RolePickerMenu : CustomPaginableMenu
{
    protected override string Name => "EclipseRolePicker";

    protected override TextBoxTMP? PrefabTextbox => null;

    public static RolePickerMenu Open(Action<RoleBehaviour> onRolePicked, IEnumerable<RoleBehaviour>? roles = null)
    {
        var menu = Create<RolePickerMenu>();
        menu.Begin(() => menu.RegisterPanels(
            roles ?? SelectableRoles(),
            role =>
            {
                if (role == null)
                {
                    return;
                }

                try
                {
                    onRolePicked(role);
                }
                finally
                {
                    menu.ForceClose();
                }
            },

            (panel, index, role, onClick) => CosmeticGuard.Run("freeplay-role-panel", () => RolePanel.Configure(panel, index, role, onClick)),
            (panel, role) => new MenuEntry(panel, FreeplayRolePicker.SortKey(role.GetRoleName()))));

        return menu;
    }

    private static IEnumerable<RoleBehaviour> SelectableRoles()
    {
        foreach (var role in RoleManager.Instance.AllRoles)
        {
            if (role == null)
            {
                continue;
            }

            var hidden = role is ICustomRole { Configuration.ShowInFreeplay: false };
            if (FreeplayRolePicker.IsSelectable(role.IsDead, hidden))
            {
                yield return role;
            }
        }
    }
}
