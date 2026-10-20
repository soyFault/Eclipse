using System.Collections;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal static class ScreenFlash
{
    private static SpriteRenderer? _overlay;
    private static int _latest;

    public static void Flash(Color color, float seconds = 1f, float alpha = 0.3f)
    {
        if (!HudManager.InstanceExists || HudManager.Instance.FullScreen == null)
        {
            return;
        }

        Coroutines.Start(CoFlash(color, seconds, alpha));
    }

    private static IEnumerator CoFlash(Color color, float seconds, float alpha)
    {
        var overlay = GetOverlay();
        if (overlay == null)
        {
            yield break;
        }

        var mine = ++_latest;
        color.a = alpha;
        overlay.color = color;
        overlay.enabled = true;
        overlay.gameObject.SetActive(true);

        yield return new WaitForSeconds(seconds);
        if (mine == _latest && overlay != null)
        {
            overlay.enabled = false;
        }
    }

    private static SpriteRenderer? GetOverlay()
    {
        if (_overlay == null)
        {
            var hud = HudManager.Instance;
            if (hud == null || hud.FullScreen == null)
            {
                return null;
            }

            _overlay = Object.Instantiate(hud.FullScreen, hud.FullScreen.transform.parent);
            _overlay.transform.localScale *= 10f;
        }

        return _overlay;
    }
}
