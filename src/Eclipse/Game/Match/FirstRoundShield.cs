using System.Collections.Generic;
using System.Linq;
using Eclipse.Buttons;
using Eclipse.Core;
using Eclipse.Networking;
using Eclipse.Options.Host;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Translation;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal static class FirstRoundShield
{
    private static readonly FirstDeadBook Book = new();

    private static byte? _holder;

    public static bool Holds(byte playerId)
    {
        return _holder == playerId;
    }

    [RegisterEvent]
    public static void OnPlayerDeath(PlayerDeathEvent death)
    {
        if (death.Player?.Data != null && !TutorialManager.InstanceExists && !LobbyBehaviour.Instance)
        {
            Book.Record(death.Player.Data.PlayerName);
        }

        if (death.Player != null && Holds(death.Player.PlayerId))
        {
            _holder = null;
        }
    }

    [RegisterEvent(-1)]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        if (!roundStart.TriggeredByIntro)
        {
            _holder = null;
            return;
        }

        _holder = null;
        if (!AmongUsClient.Instance.AmHost)
        {
            Book.Clear();
            return;
        }

        var present = new List<string>();
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player != null && player.Data != null)
            {
                present.Add(player.Data.PlayerName);
            }
        }

        var recipientName = Book.Recipient(present);
        Book.Clear();
        var recipient = recipientName == null ? null : PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.Data.PlayerName == recipientName);
        var give = FirstRoundShieldRules.ShouldGive(
            OptionGroupSingleton<RoundStartOptions>.Instance.FirstDeathShield,
            true,
            TutorialManager.InstanceExists,
            recipient != null);
        if (give)
        {
            Logger<EclipsePlugin>.Info($"First-round shield for {recipientName}.");
            FirstRoundRpcs.RpcFirstRoundShield(recipient!);
        }
    }

    public static void Give(PlayerControl recipient)
    {
        _holder = recipient.PlayerId;
        if (recipient.AmOwner)
        {
            Notify();
        }
    }

    [RegisterEvent]
    public static void OnStartMeeting(StartMeetingEvent meeting)
    {
        _holder = null;
    }

    [RegisterEvent(-850)]
    public static void OnBeforeMurder(BeforeMurderEvent murder)
    {
        if (murder.IsCancelled || !Stops(murder.Source, murder.Target, murder.IgnoreDefense))
        {
            return;
        }

        murder.Cancel();
        if (murder.Source.AmOwner)
        {
            ShieldGuard.ResetStoppedAttacker(murder.Source);
            ScreenFlash.Flash(new Color(0.39f, 0.86f, 0.39f));
        }
    }

    [RegisterEvent(-850)]
    public static void OnButtonClick(MiraButtonClickEvent click)
    {
        if (click.IsCancelled || click.Button is not CustomActionButton<PlayerControl> button || button is not IKillButton)
        {
            return;
        }

        var target = button.Target;
        if (target == null || !button.CanClick() || !Stops(PlayerControl.LocalPlayer, target, false))
        {
            return;
        }

        click.Cancel();
        button.SetTimer(ShieldGuard.BlockedCooldown);
        ScreenFlash.Flash(new Color(0.39f, 0.86f, 0.39f));
    }

    public static string? Mark(PlayerControl viewer, PlayerControl target)
    {
        return Holds(target.PlayerId) ? " <color=#64DC64>" + MiraLocaleManager.Get("Eclipse.FirstRound.Mark") + "</color>" : null;
    }

    private static bool Stops(PlayerControl source, PlayerControl target, bool ignoresDefense)
    {
        return FirstRoundShieldRules.Blocks(Holds(target.PlayerId), source.PlayerId, target.PlayerId, ignoresDefense, MeetingHud.Instance || ExileController.Instance);
    }

    private static void Notify()
    {
        MiraAPI.Utilities.Helpers.CreateAndShowNotification(MiraLocaleManager.Get("Eclipse.FirstRound.Notice"), new Color(0.39f, 0.86f, 0.39f));
    }
}
