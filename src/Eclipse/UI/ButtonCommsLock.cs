using Eclipse.Assets;
using UnityEngine;

namespace Eclipse.Ui;

internal static class ButtonCommsLock
{
    private const float Depth = -9f;

    public static SpriteRenderer? Create(ActionButton? button, SpriteRenderer? gameIcon)
    {
        SpriteRenderer? icon = null;
        CosmeticGuard.Run("button-comms-lock", () =>
        {
            var own = EclipseAssets.CommsLock()?.LoadAsset();
            var sprite = own != null ? own : gameIcon != null ? gameIcon.sprite : null;
            if (button == null || sprite == null)
            {
                return;
            }

            var overlay = new GameObject("EclipseCommsLock");
            overlay.transform.SetParent(button.transform, false);
            overlay.layer = button.gameObject.layer;
            icon = overlay.AddComponent<SpriteRenderer>();
            icon.sprite = sprite;
            icon.enabled = false;
            if (own == null && gameIcon != null)
            {
                overlay.transform.localPosition = gameIcon.transform.localPosition;
                overlay.transform.localScale = gameIcon.transform.localScale;
                overlay.transform.localRotation = gameIcon.transform.localRotation;
                icon.color = gameIcon.color;
                icon.sharedMaterial = gameIcon.sharedMaterial;
            }

            overlay.transform.localPosition = new Vector3(overlay.transform.localPosition.x, overlay.transform.localPosition.y, Depth);
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
