using Eclipse.Assets;
using UnityEngine;

namespace Eclipse.Ui;

internal static class ButtonRoundLock
{
    private const float Depth = -10f;

    public static SpriteRenderer? Create(ActionButton? button)
    {
        SpriteRenderer? icon = null;
        CosmeticGuard.Run("button-round-lock", () =>
        {
            var sprite = EclipseAssets.FirstRoundLock()?.LoadAsset();
            if (button == null || sprite == null)
            {
                return;
            }

            var padlock = new GameObject("EclipseRoundLock");
            padlock.transform.SetParent(button.transform, false);
            padlock.transform.localPosition = new Vector3(0f, 0f, Depth);
            padlock.layer = button.gameObject.layer;
            icon = padlock.AddComponent<SpriteRenderer>();
            icon.sprite = sprite;
            icon.enabled = false;
        });

        return icon;
    }

    public static void Set(SpriteRenderer? icon, bool locked)
    {
        if (icon != null && icon.enabled != locked)
        {
            icon.enabled = locked;
        }
    }
}
