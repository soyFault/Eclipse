using System.Linq;
using Eclipse.Assets;
using Eclipse.Freeplay;
using Eclipse.Game;
using MiraAPI.Hud;
using MiraAPI.Translation;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Buttons.FreeplayTools;

internal sealed class FreeplayResetButton : CustomActionButton
{
    public override string Name => "Eclipse.Freeplay.Reset";

    public override float Cooldown => 0.001f;

    public override float InitialCooldown => 0.001f;

    public override ButtonLocation Location => ButtonLocation.BottomLeft;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.FreeplayIcon("FreeplayReset", MiraAssets.RefreshIcon);

    public override bool Enabled(RoleBehaviour? role)
    {
        return FreeplayState.IsActive;
    }

    protected override void OnClick()
    {
        var snapshot = PlayerControl.AllPlayerControls.ToArray()
            .Where(p => p != null && p.Data?.Role != null)
            .Select(p => (Player: p, Role: p.Data.Role.Role))
            .ToArray();

        HudManager.Instance.ShowPopUp(MiraLocaleManager.Get("Eclipse.Freeplay.ResetPopup"));
        ShipStatus.Instance.Begin();
        if (GameManager.Instance)
        {
            GameManager.Instance.ReviveEveryoneFreeplay();
        }

        foreach (var (player, role) in snapshot)
        {
            RoleChanger.ChangeRole(player, role);
        }

        Logger<EclipsePlugin>.Info($"Freeplay reset: {snapshot.Length} role(s) handed out again.");
    }
}
