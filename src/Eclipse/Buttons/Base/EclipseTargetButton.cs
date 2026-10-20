// Portions derived from earlier GPLv3 work; see ATTRIBUTIONS.md.
using Eclipse.Core;
using MiraAPI.PluginLoading;
using MiraAPI.Utilities;
using UnityEngine;

namespace Eclipse.Buttons;

[MiraIgnore]
internal abstract class EclipseTargetButton<TRole, TTarget> : EclipseActionButton<TTarget>
    where TRole : RoleBehaviour
    where TTarget : MonoBehaviour
{

    protected static TRole? LocalRole => PlayerControl.LocalPlayer?.Data?.Role as TRole;

    protected virtual Color OutlineColor => TextOutlineColor;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is TRole;
    }

    public override bool IsTargetValid(TTarget? target)
    {
        return base.IsTargetValid(target) && !(target is PlayerControl { inVent: true });
    }

    public override void SetOutline(bool active)
    {
        var color = active ? OutlineColor : (Color?)null;
        switch (Target)
        {
            case PlayerControl player when player.cosmetics.currentBodySprite != null:
                player.cosmetics.currentBodySprite.BodySprite.UpdateOutline(color);
                break;
            case DeadBody body:
                foreach (var renderer in body.bodyRenderers)
                {
                    renderer.UpdateOutline(color);
                }

                break;
        }
    }
}

[MiraIgnore]
internal abstract class EclipsePlayerButton<TRole> : EclipseTargetButton<TRole, PlayerControl>
    where TRole : RoleBehaviour
{
    protected virtual bool IncludeImpostors => true;

    public override PlayerControl? GetTarget()
    {
        var local = PlayerControl.LocalPlayer;
        if (local.Data.IsDead || LocalRole is not { } role || !CanTarget(role))
        {
            return null;
        }

        return local.GetClosestPlayer(IncludeImpostors, Distance);
    }

    protected virtual bool CanTarget(TRole role)
    {
        return true;
    }
}
