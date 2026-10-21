using AmongUs.GameOptions;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Assets;

internal sealed class VanillaAbilityIcon(RoleTypes type, LoadableAsset<Sprite> fallback) : LoadableAsset<Sprite>
{
    public override Sprite LoadAsset()
    {
        if (LoadedAsset == null && RoleManager.InstanceExists)
        {
            var role = RoleManager.Instance.GetRole(type);
            var image = role != null && role.Ability != null ? role.Ability.Image : null;
            if (image != null)
            {
                LoadedAsset = image;
            }
        }

        return LoadedAsset != null ? LoadedAsset : fallback.LoadAsset();
    }
}
