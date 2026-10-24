using Eclipse.Core;
using Eclipse.Options.Host;
using MiraAPI.GameOptions;

namespace Eclipse.Game;

internal static class NameSightTweak
{
    private const float IntervalSeconds = 0.1f;

    private static float _elapsed;
    private static bool _applied;

    public static void Tick(float deltaTime)
    {
        _elapsed += deltaTime;
        if (_elapsed < IntervalSeconds)
        {
            return;
        }

        _elapsed = 0f;

        var mode = OptionGroupSingleton<VanillaTweakOptions>.Instance.HideNamesOutOfSight;
        if (!HudManager.InstanceExists || HudManager.Instance.ShadowQuad == null || !HudManager.Instance.ShadowQuad.gameObject.activeSelf)
        {
            mode = NameSight.Basic;
        }

        var local = PlayerControl.LocalPlayer;
        if (local == null || local.Data == null || ShipStatus.Instance == null || MeetingHud.Instance != null)
        {
            return;
        }

        if (mode == NameSight.Basic && !_applied)
        {
            return;
        }

        var players = PlayerControl.AllPlayerControls;
        for (var i = 0; i < players.Count; i++)
        {
            var player = players[i];
            if (player == null || player.Data == null || player.cosmetics == null || player.cosmetics.nameText == null)
            {
                continue;
            }

            var isSelf = player.AmOwner;
            var inSight = isSelf || mode != NameSight.Basic && !local.Data.IsDead && Sight.InSight(local, player);
            var sameRoom = mode == NameSight.RoomBased && !isSelf && !inSight && Sight.InSameRoom(local.GetTruePosition(), player.GetTruePosition());
            var show = NameSightRules.NameVisible(mode, local.Data.IsDead, isSelf, inSight, sameRoom);
            if (player.cosmetics.nameText.enabled != show)
            {
                player.cosmetics.nameText.enabled = show;
            }

            var colorBlind = player.cosmetics.colorBlindText;
            if (colorBlind != null && colorBlind.enabled != show)
            {
                colorBlind.enabled = show;
            }
        }

        _applied = mode != NameSight.Basic;
    }
}
