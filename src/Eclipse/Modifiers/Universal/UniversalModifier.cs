using System.Linq;
using Eclipse.Assets;
using Eclipse.Core;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.PluginLoading;
using MiraAPI.Translation;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Modifiers;

[MiraIgnore]
internal abstract class UniversalModifier : GameModifier
{
    public override string IdPrefix => "Eclipse.Modifier";

    public abstract Color ModifierColor { get; }

    public override string ModifierName => MiraLocaleManager.Get($"{IdPrefix}.{IdPart}");

    public override LoadableAsset<Sprite>? ModifierIcon => EclipseAssets.ModifierIcon(IdPart);

    public override Color FreeplayFileColor => ModifierColor;

    public override string GetDescription()
    {
        return MiraLocaleManager.Get($"{IdPrefix}.{IdPart}.Description");
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role)
               && role.Player != null
               && UniversalRules.CanReceive(role.Player.GetModifiers<UniversalModifier>().Any());
    }
}
