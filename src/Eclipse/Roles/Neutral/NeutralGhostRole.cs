using System.Text;
using AmongUs.GameOptions;
using Eclipse.Api.Alignments;
using Eclipse.Api.Factions;
using Eclipse.Api.Roles;
using Eclipse.Colors;
using Eclipse.Game;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Translation;
using UnityEngine;

namespace Eclipse.Roles.Neutral;

internal class NeutralGhostRole : RoleBehaviour, IEclipseRole
{
    private Minigame _hauntMenu = null!;

    public string IdPrefix => "Eclipse.Role";

    public string IdPart => "NeutralGhost";

    [HideFromIl2Cpp]
    public AlignmentDefinition Alignment => BuiltInAlignments.NeutralGhost;

    // A dead neutral shows the name, the color and the description of the role they had.
    public virtual string RoleName => (RoleWhenAlive() as ICustomRole)?.RoleName ?? MiraLocaleManager.Get("Eclipse.Role.NeutralGhost");

    public virtual string RoleDescription => (RoleWhenAlive() as ICustomRole)?.RoleDescription ?? string.Empty;

    public virtual string RoleLongDescription => (RoleWhenAlive() as ICustomRole)?.RoleLongDescription ?? string.Empty;

    public virtual Color RoleColor => (RoleWhenAlive() as ICustomRole)?.RoleColor ?? FactionColors.Neutral;

    public CustomRoleConfiguration Configuration => new(this)
    {
        TasksCountForProgress = false,
        HideSettings = true,
        MaxRoleCount = 0,
        DefaultRoleCount = 0,
        DefaultChance = 0,
        CanModifyChance = false,
    };

    public override bool IsDead => true;

    public override bool IsAffectedByComms => false;

    // The ghost has the same haunt ability as the game's own crewmate ghost.
    public void Awake()
    {
        var crewGhost = RoleManager.Instance.GetRole(RoleTypes.CrewmateGhost).Cast<CrewmateGhostRole>();
        _hauntMenu = crewGhost.HauntMenu;
        Ability = crewGhost.Ability;
    }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        NeutralTaskHeader.Show(playerControl, null, false);
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        NeutralTaskHeader.Clear(targetPlayer);
    }

    // The default hint talks about finishing tasks.
    public override void AppendTaskHint(Il2CppSystem.Text.StringBuilder taskStringBuilder)
    {
    }

    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }

        var console = usable.TryCast<Console>();
        return console == null || console.AllowImpostor;
    }

    // The ability button of a ghost opens the haunt menu: the game's own code for it works only for its own ghost roles, so it is repeated here.
    public override void UseAbility()
    {
        if (HudManager.Instance.Chat.IsOpenOrOpening)
        {
            return;
        }

        if (Minigame.Instance)
        {
            if (Minigame.Instance.TryCast<HauntMenuMinigame>())
            {
                Minigame.Instance.Close();
            }

            return;
        }

        var minigame = Instantiate(_hauntMenu, HudManager.Instance.AbilityButton.transform, false);
        minigame.transform.SetLocalZ(-5f);
        minigame.Begin(null);
        HudManager.Instance.AbilityButton.SetDisabled();
    }

    // The win is decided by the role they had.
    public override bool DidWin(GameOverReason gameOverReason)
    {
        var role = RoleWhenAlive();
        return role != null && role.DidWin(gameOverReason);
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var text = CustomRoleUtils.CreateForRole(this);
        var key = Player != null && NeutralTaskHeader.HasWon(Player.PlayerId) ? "Eclipse.Role.NeutralGhost.TabWon" : "Eclipse.Role.NeutralGhost.TabDead";
        text.AppendLine($"\n<b>{MiraLocaleManager.Get(key)}</b>");
        return text;
    }

    [HideFromIl2Cpp]
    private RoleBehaviour? RoleWhenAlive()
    {
        var data = Player == null ? null : Player.Data;
        return data != null && data.RoleWhenAlive.HasValue ? RoleManager.Instance.GetRole(data.RoleWhenAlive.Value) : null;
    }
}
