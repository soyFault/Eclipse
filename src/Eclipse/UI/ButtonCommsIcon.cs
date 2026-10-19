using UnityEngine;

namespace Eclipse.Ui;

internal static class ButtonCommsIcon
{
    public static SpriteRenderer? Find(ActionButton? button)
    {
        SpriteRenderer? icon = null;
        CosmeticGuard.Run("button-comms-icon", () =>
        {
            icon = button?.TryCast<AbilityButton>()?.commsDown?.GetComponent<SpriteRenderer>();
        });

        Hide(icon);
        return icon;
    }

    public static void Hide(SpriteRenderer? icon)
    {
        if (icon != null && icon.enabled)
        {
            icon.enabled = false;
        }
    }
}
