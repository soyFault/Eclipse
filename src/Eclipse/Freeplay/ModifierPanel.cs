using System;
using Eclipse.Modifiers;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Eclipse.Freeplay;

internal static class ModifierPanel
{
    public static void Configure(ShapeshifterPanel panel, int index, UniversalModifier modifier, Action onClick)
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

        var icon = Object.Instantiate(panel.Background, panel.transform).gameObject;
        icon.name = "ModifierIcon";
        icon.GetComponent<PassiveButton>().Destroy();
        icon.transform.localScale = new Vector3(0.21f, 0.9f, 1);
        icon.transform.localPosition += new Vector3(-0.964f, 0, -2);
        icon.GetComponent<SpriteRenderer>().sprite = (modifier.ModifierIcon ?? MiraAssets.CustomTeamFile).LoadAsset();
        icon.SetActive(true);

        panel.LevelNumberText.transform.parent.gameObject.SetActive(false);
        panel.NameText.color = modifier.ModifierColor;
        panel.NameText.text = $"<size=88%>{modifier.ModifierName}</size>\n<size=70%><color=white>{MiraLocaleManager.Get("Eclipse.Freeplay.ModifierKind")}</color></size>";
        panel.NameText.alignment = TextAlignmentOptions.Right;
        panel.NameText.transform.localPosition += Vector3.left * 0.05f;
    }
}
