using System.Linq;
using Eclipse.Assets;
using Eclipse.Freeplay;
using Eclipse.Networking;
using Eclipse.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.Hud;
using MiraAPI.Roles;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Freeplay;

internal sealed class FreeplayShieldButton : EclipseActionButton
{
    public override string Name => "Eclipse.Freeplay.GiveShield";

    public override float Cooldown => 0.001f;

    public override float InitialCooldown => 0.001f;

    public override ButtonLocation Location => ButtonLocation.BottomLeft;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.FreeplayIcon("FreeplayGiveShield", MiraAssets.CrewmateFile);

    public override bool Enabled(RoleBehaviour? role)
    {
        return FreeplayState.IsActive;
    }

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.NetTransform.Halt();
        if (Minigame.Instance)
        {
            return;
        }

        var medics = CustomRoleUtils.GetActiveRolesOfType<MedicRole>()
            .Where(role => role.Player != null && !role.Player.Data.IsDead)
            .Select(role => role.Player.PlayerId)
            .ToHashSet();
        if (medics.Count == 0)
        {
            Helpers.CreateAndShowNotification(MiraLocaleManager.Get("Eclipse.Freeplay.NoMedic"), Color.white);
            return;
        }

        var medicMenu = CustomPlayerMenu.Create();
        medicMenu.Begin(
            player => medics.Contains(player.PlayerId),
            medic =>
            {
                medicMenu.ForceClose();
                if (medic == null)
                {
                    return;
                }

                var targetMenu = CustomPlayerMenu.Create();
                targetMenu.Begin(
                    player => player.Data != null && !player.Data.IsDead && !player.Data.Disconnected,
                    target =>
                    {
                        targetMenu.ForceClose();
                        if (target != null)
                        {
                            MedicRpcs.RpcMedicShield(medic, target);
                        }
                    });
            });
    }
}
