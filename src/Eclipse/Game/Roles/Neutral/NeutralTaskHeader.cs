using System.Linq;
using Eclipse.Api.Factions;
using Eclipse.Core;
using MiraAPI.Translation;
using MiraAPI.Utilities;

namespace Eclipse.Game;

internal static class NeutralTaskHeader
{
    private const string TaskName = "NeutralRoleText";

    public static void Show(PlayerControl player, string? alignmentId, bool alive)
    {
        if (!player.AmOwner)
        {
            return;
        }

        var tab = NeutralTabRules.Choose(alive, HasWon(player.PlayerId), alignmentId);
        var text = MiraLocaleManager.Get(NeutralTabRules.KeyOf(tab));
        if (NeutralTabRules.UsesTeamColor(tab))
        {
            text = $"{FactionColors.Neutral.ToTextColor()}{text}</color>";
        }

        var task = PlayerTask.GetOrCreateTask<ImportantTextTask>(player, 0);
        task.Text = text;
        task.name = TaskName;
    }

    public static void Clear(PlayerControl player)
    {
        if (!player.AmOwner)
        {
            return;
        }

        var task = player.myTasks.ToArray().FirstOrDefault(x => x is ImportantTextTask && x.name == TaskName);
        if (task != null)
        {
            player.myTasks.Remove(task);
            task.gameObject.DeepDestroy();
        }
    }

    public static bool HasWon(byte playerId)
    {
        return NeutralWinners.Early.Contains(playerId) || NeutralWinners.Board.Contains(playerId);
    }
}
