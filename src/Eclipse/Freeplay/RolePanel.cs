using System;
using Eclipse.Api.Factions;
using Eclipse.Api.Roles;
using Eclipse.Roles;
using MiraAPI.Roles;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Eclipse.Freeplay;


internal static class RolePanel
{
    public static void Configure(ShapeshifterPanel panel, int index, RoleBehaviour role, Action onClick)
    {
        panel.shapeshift = onClick;
        panel.PlayerIcon.SetFlipX(false);
        panel.PlayerIcon.ToggleName(false);
        panel.PlayerIcon.gameObject.SetActive(false);

        foreach (var spriteRenderer in panel.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.material.SetInt(PlayerMaterial.MaskLayer, index + 2);
        }

        panel.PlayerIcon.SetMaskLayer(index + 2);
        panel.PlayerIcon.cosmetics.SetMaskType(PlayerMaterial.MaskType.ComplexUI);

        foreach (var hand in panel.PlayerIcon.Hands)
        {
            hand.sharedMaterial = CosmeticsLayer.GetBodyMaterial(PlayerMaterial.MaskType.ComplexUI);
            hand.gameObject.active = false;
        }

        foreach (var bodySprite in panel.PlayerIcon.OtherBodySprites)
        {
            bodySprite.sharedMaterial = CosmeticsLayer.GetBodyMaterial(PlayerMaterial.MaskType.ComplexUI);
            bodySprite.gameObject.active = false;
        }

        var (faction, alignmentName) = Describe(role);

        var roleIcon = Object.Instantiate(panel.Background, panel.transform).gameObject;
        roleIcon.name = "RoleIcon";
        roleIcon.GetComponent<PassiveButton>().Destroy();
        roleIcon.transform.localScale = new Vector3(0.21f, 0.9f, 1);
        roleIcon.transform.localPosition += new Vector3(-0.964f, 0, -2);
        roleIcon.GetComponent<SpriteRenderer>().sprite = IconFor(role, faction);
        roleIcon.SetActive(true);

        var color = role is ICustomRole custom ? custom.RoleColor : role.TeamColor;

        panel.LevelNumberText.transform.parent.gameObject.SetActive(false);
        panel.NameText.color = color;
        panel.NameText.text = $"<size=88%>{role.GetRoleName()}</size>\n<size=70%><color=white>{alignmentName}</color></size>";
        panel.NameText.alignment = TextAlignmentOptions.Right;
        panel.NameText.transform.localPosition += Vector3.left * 0.05f;
    }

    private static (Faction Faction, string AlignmentName) Describe(RoleBehaviour role)
    {
        var (faction, _) = RoleClassification.Classify(role);

        if (role is IEclipseRole eclipseRole)
        {
            return (faction, eclipseRole.Alignment.DisplayName);
        }

        var teamKey = faction switch
        {
            Faction.Crewmate => "MiraApi.RoleTeam.Crewmate",
            Faction.Impostor => "MiraApi.RoleTeam.Impostor",
            _ => "MiraApi.RoleTeam.Neutral",
        };

        return (faction, MiraLocaleManager.Get(teamKey));
    }

    private static Sprite? IconFor(RoleBehaviour role, Faction faction)
    {
        if (role is ICustomRole { Configuration.Icon: { } icon })
        {
            return icon.LoadAsset();
        }

        if (role.RoleIconSolid != null)
        {
            return role.RoleIconSolid;
        }

        LoadableAsset<Sprite> fallback = faction switch
        {
            Faction.Crewmate => MiraAssets.CrewmateFile,
            Faction.Impostor => MiraAssets.ImpostorFile,
            _ => MiraAssets.CustomTeamFile,
        };
        return fallback.LoadAsset();
    }
}
