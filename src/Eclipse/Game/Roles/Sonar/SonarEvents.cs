using Eclipse.Buttons.Crewmate;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Hud;

namespace Eclipse.Game;

internal static class SonarEvents
{
    [RegisterEvent]
    public static void OnCompleteTask(CompleteTaskEvent task)
    {
        var options = OptionGroupSingleton<SonarOptions>.Instance;
        if (!task.Player.AmOwner || task.Player.Data?.Role is not SonarRole || !options.TaskUses || options.ResetOnNewRound)
        {
            return;
        }

        var button = CustomButtonSingleton<SonarTrackButton>.Instance;
        button.SetUses(button.UsesLeft + 1);
    }

    [RegisterEvent]
    public static void OnStartMeeting(StartMeetingEvent meeting)
    {
        var options = OptionGroupSingleton<SonarOptions>.Instance;
        if (!options.ResetOnNewRound || PlayerControl.LocalPlayer?.Data?.Role is not SonarRole role)
        {
            return;
        }

        role.Clear();
        CustomButtonSingleton<SonarTrackButton>.Instance.SetUses((int)options.MaxTracks);
    }
}
