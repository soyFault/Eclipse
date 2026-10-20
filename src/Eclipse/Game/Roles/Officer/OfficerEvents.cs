using Eclipse.Core;
using Eclipse.Networking;
using Eclipse.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Killing;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting;
using UnityEngine;

namespace Eclipse.Game;


internal static class OfficerEvents
{
    [RegisterEvent]
    public static void OnStartMeeting(StartMeetingEvent meeting)
    {
        var local = PlayerControl.LocalPlayer;
        if (local?.Data?.Role is not OfficerRole officer)
        {
            return;
        }

        var fresh = OfficerRules.IsFresh(Time.time - officer.LastLoadTime);
        var (rounds, loaded) = OfficerRules.AfterMeetingStart(officer.RoundsBeforeReset, fresh);
        OfficerRpcs.RpcOfficerSync(local, rounds, officer.BulletsLeft, loaded);
    }
}
