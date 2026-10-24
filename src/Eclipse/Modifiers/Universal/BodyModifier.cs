using Eclipse.Core;
using UnityEngine;

namespace Eclipse.Modifiers;

internal abstract class BodyModifier : UniversalModifier
{
    protected abstract float SpeedMultiplier { get; }

    protected abstract float Scale { get; }

    public override void OnActivate()
    {
        Apply(SpeedMultiplier, Scale);
    }

    public override void OnDeactivate()
    {
        Apply(1f, UniversalRules.NormalScale);
    }

    private void Apply(float speed, float scale)
    {
        if (Player == null)
        {
            return;
        }

        Player.MyPhysics.Speed = UniversalRules.SpeedFor(speed);
        Player.transform.localScale = new Vector3(scale, scale, 1f);
    }
}
