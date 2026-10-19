using System.Collections.Generic;
using Eclipse.Core;
using Eclipse.Networking;
using Eclipse.Options;
using Eclipse.Roles;
using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using UnityEngine;

namespace Eclipse.Game;

internal static class AurialEvents
{

    private static readonly Dictionary<byte, TargetArrow> Arrows = new();

    [RegisterEvent]
    public static void OnButtonClick(MiraButtonClickEvent click)
    {
        if (click.Button != null && !click.IsCancelled && click.Button.CanClick())
        {
            Tell(PlayerControl.LocalPlayer);
        }
    }

    [RegisterEvent]
    public static void OnBeforeMurder(BeforeMurderEvent murder)
    {
        if (TutorialManager.InstanceExists || murder.Source.AmOwner)
        {
            Tell(murder.Source);
        }
    }
    public static void Tell(PlayerControl source)
    {
        foreach (var role in CustomRoleUtils.GetActiveRolesOfType<AurialRole>())
        {
            if (role.Player != null && role.Player != source)
            {
                AurialRpcs.RpcAurialSense(role.Player, source);
            }
        }
    }

    public static void Show(PlayerControl aurial, PlayerControl source)
    {
        var options = OptionGroupSingleton<AurialOptions>.Instance;
        var ship = ShipStatus.Instance;
        if (!aurial.AmOwner || !ship || source == aurial)
        {
            return;
        }

        var distance = Vector2.Distance(source.GetTruePosition(), aurial.GetTruePosition());

        var sense = AurialRules.Sense(distance, options.AuraInnerRadius, options.AuraOuterRadius, ship.MaxLightRadius);
        if (sense == AuraSense.None)
        {
            return;
        }

        Color color = sense == AuraSense.PlayerColor ? (Color)Palette.PlayerColors[source.Data.DefaultOutfit.ColorId] : Color.white;

        if (Arrows.TryGetValue(source.PlayerId, out var old))
        {
            old.Destroy();
        }

        Arrows[source.PlayerId] = TargetArrow.Blip(aurial.transform, color, source.transform.position, options.SenseDuration);
    }
}
