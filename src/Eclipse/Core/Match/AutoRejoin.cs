using System.Collections;
using Eclipse.Core;
using MiraAPI.LocalSettings;
using Reactor.Utilities;
using TMPro;
using UnityEngine;

namespace Eclipse.Game;

internal static class AutoRejoin
{
    private const float GiveUpAfterSeconds = 25f;

    private static int _run;

    public static bool Active { get; private set; }

    public static void Begin(EndGameManager manager)
    {
        var settings = LocalSettingsTabSingleton<EclipseLocalTabPreferences>.Instance;
        var client = AmongUsClient.Instance;
        if (client == null || !AutoRejoinRules.WillRejoin(settings.AutoRejoin.Value, client.AmHost, TutorialManager.InstanceExists))
        {
            return;
        }

        Active = true;
        var run = ++_run;
        Coroutines.Start(CoCountdown(manager, settings.AutoRejoinDelay.Value, run));
    }

    public static void Cancel()
    {
        if (Active)
        {
            Logger<EclipsePlugin>.Info("AutoRejoin cancelled.");
        }

        Active = false;
        _run++;
    }

    private static IEnumerator CoCountdown(EndGameManager manager, float delay, int run)
    {
        var label = CreateLabel(manager);
        var left = delay;
        var shown = -1;
        while (left > 0f)
        {
            if (run != _run || manager == null)
            {
                DestroyLabel(label);
                yield break;
            }

            var seconds = AutoRejoinRules.SecondsToShow(left);
            if (seconds != shown && label != null)
            {
                shown = seconds;
                label.text = string.Format(MiraAPI.Translation.MiraLocaleManager.Get("Eclipse.Local.RejoinCountdown"), seconds);
            }

            left -= Time.unscaledDeltaTime;
            yield return null;
        }

        var waited = 0f;
        while (waited < GiveUpAfterSeconds)
        {
            if (run != _run || manager == null)
            {
                DestroyLabel(label);
                yield break;
            }

            var button = manager.Navigation != null && manager.Navigation.PlayAgainButton != null
                ? manager.Navigation.PlayAgainButton.GetComponent<PassiveButton>()
                : null;
            if (button != null && button.gameObject.activeInHierarchy)
            {
                Logger<EclipsePlugin>.Info("AutoRejoin: pressing Play again.");
                button.OnClick.Invoke();
                break;
            }

            waited += Time.unscaledDeltaTime;
            yield return null;
        }

        DestroyLabel(label);
        Active = false;
    }

    private static TextMeshPro? CreateLabel(EndGameManager manager)
    {
        return EndScreenText.Create(manager, "EclipseRejoinCountdown", 2.6f, TextAlignmentOptions.Center, 0.5f, 0.07f);
    }

    private static void DestroyLabel(TextMeshPro? label)
    {
        if (label != null)
        {
            Object.Destroy(label.gameObject);
        }
    }
}
