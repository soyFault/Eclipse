using System.Collections.Generic;
using Eclipse.Api.Roles;
using Eclipse.Core;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Translation;

namespace Eclipse.Game;

internal static class IntroEvents
{
    [RegisterEvent]
    public static void OnRoleReveal(IntroRoleRevealEvent reveal)
    {
        FaultGuard.Run("Intro text", () => Decorate(reveal.IntroCutscene));
    }

    private static void Decorate(IntroCutscene intro)
    {
        var local = PlayerControl.LocalPlayer;
        if (intro == null || intro.RoleBlurbText == null || local == null || local.Data?.Role == null)
        {
            return;
        }

        var alignment = local.Data.Role is IEclipseRole eclipse ? eclipse.Alignment.DisplayName : null;
        var names = new List<string>();
        foreach (var modifier in local.GetModifiers<GameModifier>())
        {
            if (!modifier.HideOnUi)
            {
                names.Add(modifier.ModifierName);
            }
        }

        var modifiers = IntroText.ModifierLine(MiraLocaleManager.Get("Eclipse.Intro.Modifier"), MiraLocaleManager.Get("Eclipse.Intro.Modifiers"), names);
        intro.RoleBlurbText.text = IntroText.Compose(intro.RoleBlurbText.text, alignment, modifiers);
    }
}
