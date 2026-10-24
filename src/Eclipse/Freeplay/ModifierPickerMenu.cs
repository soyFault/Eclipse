using System;
using Eclipse.Ui;
using Eclipse.Game;
using Eclipse.Modifiers;
using MiraAPI.Hud;

namespace Eclipse.Freeplay;

internal sealed class ModifierPickerMenu : CustomPaginableMenu
{
    protected override string Name => "EclipseModifierPicker";

    protected override TextBoxTMP? PrefabTextbox => null;

    public static ModifierPickerMenu Open(Action<UniversalModifier> onModifierPicked)
    {
        var menu = Create<ModifierPickerMenu>();
        menu.Begin(() => menu.RegisterPanels(
            FreeplayModifiers.Samples,
            modifier =>
            {
                if (modifier == null)
                {
                    return;
                }

                try
                {
                    onModifierPicked(modifier);
                }
                finally
                {
                    menu.ForceClose();
                }
            },
            (panel, index, modifier, onClick) => CosmeticGuard.Run("freeplay-modifier-panel", () => ModifierPanel.Configure(panel, index, modifier, onClick)),
            (panel, modifier) => new MenuEntry(panel, modifier.ModifierName)));

        return menu;
    }
}
