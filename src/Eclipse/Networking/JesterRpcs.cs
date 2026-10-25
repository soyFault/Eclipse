using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Options.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;

namespace Eclipse.Networking;

internal static class JesterRpcs
{
    [MethodRpc((uint)EclipseRpc.JesterHaunt, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcJesterHaunt(PlayerControl jester, PlayerControl victim)
    {
        if (LobbyBehaviour.Instance || jester == null || victim == null || !JesterBook.IsHauntOpen(jester.PlayerId) || JesterBook.HasHaunted(jester.PlayerId))
        {
            return;
        }

        JesterBook.MarkHaunted(jester.PlayerId);

        var options = OptionGroupSingleton<JesterOptions>.Instance;
        if (JesterRules.AnnouncesWin(options.AfterWin, options.AnnounceWin.Value))
        {
            Helpers.CreateAndShowNotification(
                $"<b>{MiraLocaleManager.Get("Eclipse.Role.Jester.HauntNotif").Replace("<victim>", victim.Data.PlayerName)}</b>",
                EclipseColors.Jester);
        }
    }
}
