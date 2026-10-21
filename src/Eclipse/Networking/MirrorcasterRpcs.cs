using Eclipse.Api.Alignments;
using Eclipse.Api.Factions;
using Eclipse.Assets;
using Eclipse.Buttons.Crewmate;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using UnityEngine;

namespace Eclipse.Networking;

internal static class MirrorcasterRpcs
{
    private static readonly Color DangerColor = new Color32(144, 162, 195, 255);

    [MethodRpc((uint)EclipseRpc.MirrorPlace, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcMirrorPlace(PlayerControl mirrorcaster, PlayerControl target)
    {
        if (LobbyBehaviour.Instance || MeetingHud.Instance || mirrorcaster == null || target == null || mirrorcaster == target || mirrorcaster.Data == null
            || mirrorcaster.Data.IsDead || mirrorcaster.Data.Role is not MirrorcasterRole || target.Data == null || target.Data.IsDead || target.Data.Disconnected)
        {
            return;
        }

        Mirrors.Place(mirrorcaster.PlayerId, target.PlayerId, OptionGroupSingleton<MirrorcasterOptions>.Instance.MirrorDuration);
    }

    [MethodRpc((uint)EclipseRpc.MirrorClear, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcMirrorClear(PlayerControl mirrorcaster)
    {
        if (LobbyBehaviour.Instance || mirrorcaster == null || mirrorcaster.Data?.Role is not MirrorcasterRole)
        {
            return;
        }

        Mirrors.RemoveOf(mirrorcaster.PlayerId);
    }

    [MethodRpc((uint)EclipseRpc.MirrorHit, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcMirrorHit(PlayerControl source, PlayerControl mirrorcaster, PlayerControl target, bool wasAttack)
    {
        if (LobbyBehaviour.Instance || MeetingHud.Instance || source == null || mirrorcaster == null || target == null || source == target
            || mirrorcaster.Data?.Role is not MirrorcasterRole role || Mirrors.On(target.PlayerId) is not { } mirror || mirror.Mirrorcaster != mirrorcaster.PlayerId)
        {
            return;
        }

        Mirrors.Carried.Remove(target.PlayerId);
        role.UnleashesAvailable++;

        if (source.AmOwner && wasAttack)
        {
            ShieldGuard.ResetStoppedAttacker(source);
        }

        var options = OptionGroupSingleton<MirrorcasterOptions>.Instance;
        if (mirrorcaster.AmOwner)
        {
            CustomButtonSingleton<MirrorcasterMagicMirrorButton>.Instance.BreakByAttack();
            CustomButtonSingleton<MirrorcasterUnleashButton>.Instance.ResetCooldownAndOrEffect();
            Danger(true);
            Helpers.CreateAndShowNotification(Describe(options.AttackInformationGiven, source, target), Color.white, new Vector3(0f, 1f, -20f), spr: EclipseAssets.RoleIcon("Mirrorcaster")?.LoadAsset());
        }
        else if (options.WhoGetsNotification == MirrorWhoIsTold.MirrorcasterAndKiller && source.AmOwner)
        {
            Danger(false);
        }
    }

    [MethodRpc((uint)EclipseRpc.MirrorUnleash, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcMirrorUnleash(PlayerControl mirrorcaster)
    {
        if (LobbyBehaviour.Instance || mirrorcaster?.Data?.Role is not MirrorcasterRole { UnleashesAvailable: > 0 } role)
        {
            return;
        }

        role.UnleashesAvailable--;
    }

    private static void Danger(bool owner)
    {
        ScreenFlash.Flash(DangerColor);
        if (owner && HudManager.Instance != null)
        {
            HudManager.Instance.StartCoroutine(HudManager.Instance.PlayerCam.CoShakeScreen(0.4f, 3f));
        }
    }

    private static string Describe(MirrorAttackInfo info, PlayerControl source, PlayerControl target)
    {
        var attackerRole = source.Data.Role;
        var (faction, alignmentId) = RoleClassification.Classify(attackerRole);
        var text = MirrorcasterRules.MessageFor(info, attackerRole != null) switch
        {
            MirrorMessage.WithRole => MiraLocaleManager.Get("Eclipse.Role.Mirrorcaster.AttackedMessageWithType").Replace("<attackerRole>", attackerRole!.GetRoleName()),
            MirrorMessage.WithFaction => MiraLocaleManager.Get("Eclipse.Role.Mirrorcaster.AttackedMessageWithFaction").Replace("<faction>", FactionName(faction)),
            MirrorMessage.WithSubalignment => MiraLocaleManager.Get("Eclipse.Role.Mirrorcaster.AttackedMessageWithSubalignment").Replace("<subalignment>", AlignmentName(alignmentId, faction)),
            _ => MiraLocaleManager.Get("Eclipse.Role.Mirrorcaster.AttackedMessageWithoutType"),
        };
        return $"<b>{text.Replace("<player>", target.Data.PlayerName)}</b>";
    }

    private static string FactionName(Faction faction)
    {
        return MiraLocaleManager.Get(faction switch
        {
            Faction.Neutral => "MiraApi.RoleTeam.Neutral",
            Faction.Impostor => "MiraApi.RoleTeam.Impostor.Short",
            _ => "MiraApi.RoleTeam.Crewmate",
        });
    }

    // A vanilla role has no alignment: its faction is the best that can be said.
    private static string AlignmentName(string? alignmentId, Faction faction)
    {
        return alignmentId != null && AlignmentRegistry.TryGet(alignmentId, out var alignment) ? alignment.DisplayName : FactionName(faction);
    }
}
