using System;
using System.Collections.Generic;
using Eclipse.Core;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TMPro;
using UnityEngine;

namespace Eclipse.Assets;

internal static class EclipseAssets
{
    private const float RoleIconTextScale = 1.45f;

    private static readonly Dictionary<string, LoadableAsset<Sprite>?> RoleIcons = [];
    private static readonly Dictionary<string, TMP_SpriteAsset?> RoleTextIcons = [];
    private static readonly Dictionary<string, LoadableAsset<Sprite>> ButtonIcons = [];
    private static readonly Dictionary<string, LoadableAsset<Sprite>?> ModifierIcons = [];
    private static LoadableAsset<Sprite>? _firstRoundLock;
    private static bool _firstRoundLockLooked;
    private static LoadableAsset<Sprite>? _commsLock;
    private static bool _commsLockLooked;

    public static LoadableAsset<Sprite>? RoleIcon(string idPart)
    {
        if (!RoleIcons.TryGetValue(idPart, out var icon))
        {
            icon = EclipseBundle.Has(BundleArt.IconsFolder, idPart) ? new BundleSprite(BundleArt.IconsFolder, idPart) : null;
            RoleIcons[idPart] = icon;
        }

        return icon;
    }

    public static LoadableAsset<Sprite> FreeplayIcon(string key, LoadableAsset<Sprite> placeholder)
    {
        if (!ButtonIcons.TryGetValue("freeplay/" + key, out var icon))
        {
            icon = EclipseBundle.Has(BundleArt.FreeplayFolder, key) ? new BundleSprite(BundleArt.FreeplayFolder, key) : placeholder;
            ButtonIcons["freeplay/" + key] = icon;
        }

        return icon;
    }

    public static TMP_SpriteAsset? RoleIconTmp(string idPart)
    {
        if (RoleTextIcons.TryGetValue(idPart, out var cached))
        {
            return cached;
        }

        TMP_SpriteAsset? asset = null;
        var icon = RoleIcon(idPart);
        if (icon != null)
        {
            try
            {
                asset = TmpSpriteUtils.CreateSpriteAsset(icon.LoadAsset(), $"Eclipse.Role.{idPart}", RoleIconTextScale);
            }
            catch (Exception exception)
            {
                Logger<EclipsePlugin>.Error($"Art: el icono del rol {idPart} no pudo convertirse en sprite de texto: {exception}");
            }
        }

        RoleTextIcons[idPart] = asset;
        return asset;
    }

    public static LoadableAsset<Sprite> ButtonIcon(string key, LoadableAsset<Sprite> placeholder)
    {
        if (!ButtonIcons.TryGetValue(key, out var icon))
        {
            icon = EclipseBundle.Has(BundleArt.ButtonsFolder, key) ? new BundleSprite(BundleArt.ButtonsFolder, key) : placeholder;
            ButtonIcons[key] = icon;
        }

        return icon;
    }

    public static LoadableAsset<Sprite>? ModifierIcon(string idPart)
    {
        if (!ModifierIcons.TryGetValue(idPart, out var icon))
        {
            icon = EclipseBundle.Has(BundleArt.ModifiersFolder, idPart) ? new BundleSprite(BundleArt.ModifiersFolder, idPart) : null;
            ModifierIcons[idPart] = icon;
        }

        return icon;
    }

    private sealed class BundleSprite(string folder, string key) : LoadableAsset<Sprite>
    {
        public override Sprite LoadAsset()
        {
            return LoadedAsset ??= EclipseBundle.Find<Sprite>(folder, key) ?? throw new InvalidOperationException($"INVALID ASSET: {folder}/{key}");
        }
    }

    public static LoadableAsset<Sprite> SpriteIcon(string key, LoadableAsset<Sprite> placeholder)
    {
        if (!ButtonIcons.TryGetValue("sprites/" + key, out var icon))
        {
            icon = EclipseBundle.Has(BundleArt.SpritesFolder, key) ? new BundleSprite(BundleArt.SpritesFolder, key) : placeholder;
            ButtonIcons["sprites/" + key] = icon;
        }

        return icon;
    }

    public static LoadableAsset<Sprite>? FirstRoundLock()
    {
        if (!_firstRoundLockLooked)
        {
            _firstRoundLockLooked = true;
            _firstRoundLock = EclipseBundle.Has(BundleArt.SpritesFolder, "FirstRoundLock") ? new BundleSprite(BundleArt.SpritesFolder, "FirstRoundLock") : null;
        }

        return _firstRoundLock;
    }

    public static LoadableAsset<Sprite>? CommsLock()
    {
        if (!_commsLockLooked)
        {
            _commsLockLooked = true;
            _commsLock = EclipseBundle.Has(BundleArt.SpritesFolder, "CommsLock") ? new BundleSprite(BundleArt.SpritesFolder, "CommsLock") : null;
        }

        return _commsLock;
    }

    public static LoadableAsset<Sprite>? KillBackground(string name)
    {
        return EclipseBundle.Has(BundleArt.SpritesFolder, name) ? new BundleSprite(BundleArt.SpritesFolder, name) : null;
    }


}
