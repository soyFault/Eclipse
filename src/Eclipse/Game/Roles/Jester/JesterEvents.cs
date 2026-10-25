using Eclipse.Buttons.Neutral;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Options.Roles.Neutral;
using Eclipse.Roles.Neutral;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Meeting.Voting;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Roles;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal static class JesterEvents
{
    private static bool _hauntShown;

    [RegisterEvent]
    public static void OnPopulateResults(PopulateResultsEvent results)
    {
        JesterBook.RecordVoters(results.Votes);
    }

    [RegisterEvent]
    public static void OnEjection(EjectionEvent ejection)
    {
        var exiled = ejection.ExileController?.initData?.networkedPlayer;
        if (exiled == null || exiled.Role is not JesterRole role || exiled.Object == null)
        {
            return;
        }

        var options = OptionGroupSingleton<JesterOptions>.Instance;
        var mode = options.AfterWin;
        var player = exiled.Object;
        ICustomRole custom = role;

        if (JesterRules.EndsGame(mode))
        {
            NeutralWinners.Declare(player, custom.RoleName, custom.RoleColor);
            return;
        }

        NeutralWinners.DeclareEarly(player, custom.RoleName, custom.RoleColor);
        var open = JesterRules.OpensHaunt(mode, JesterBook.AliveVoterCount(player.PlayerId));
        JesterBook.OpenHaunt(player.PlayerId, open);

        if (player.AmOwner)
        {
            Say(MiraLocaleManager.Get("Eclipse.Role.Jester.WinOwner"), EclipseColors.Jester);
            if (open)
            {
                Say(MiraLocaleManager.Get("Eclipse.Role.Jester.HauntOwner"), Color.white);
            }
        }
        else if (JesterRules.AnnouncesWin(mode, options.AnnounceWin.Value))
        {
            Say(MiraLocaleManager.Get("Eclipse.Role.Jester.WinGlobal").Replace("<player>", player.Data.PlayerName), EclipseColors.Jester);
        }
    }

    [RegisterEvent]
    public static void OnStartMeeting(StartMeetingEvent meeting)
    {
        JesterBook.CloseAllHaunts();
    }

    [RegisterEvent(-1)]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        if (roundStart.TriggeredByIntro)
        {
            NeutralWinners.Reset();
            JesterBook.Reset();
        }
    }

    [RegisterEvent]
    public static void OnGameEnd(GameEndEvent gameEnd)
    {
        NeutralWinners.Reset();
        JesterBook.Reset();
    }

    public static void Tick(float deltaTime)
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null || local.Data == null)
        {
            return;
        }

        var show = JesterRules.CanHaunt(JesterBook.IsHauntOpen(local.PlayerId), JesterBook.HasHaunted(local.PlayerId), MeetingHud.Instance != null)
                   && local.Data.IsDead;
        if (show == _hauntShown)
        {
            return;
        }

        _hauntShown = show;
        CustomButtonSingleton<JesterHauntButton>.Instance.SetActive(show, local.Data.Role);
    }

    private static void Say(string text, Color color)
    {
        Helpers.CreateAndShowNotification($"<b>{text}</b>", color);
    }
}
