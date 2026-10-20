using Eclipse.Core;
using Eclipse.Modifiers;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.Modifiers;
using MiraAPI.Roles;

namespace Eclipse.Game;

internal static class ShieldGuard
{

    public const float BlockedCooldown = 5f;

    public static void ResetStoppedAttacker(PlayerControl attacker)
    {
        if (!attacker.AmOwner)
        {
            return;
        }

        attacker.isKilling = false;
        attacker.SetKillTimer(BlockedCooldown);
    }

    public static bool Blocks(PlayerControl attacker, PlayerControl target, bool ignoresDefense)
    {

        var shielded = target.TryGetModifier<MedicShieldModifier>(out var shield) && !shield.HasNoMedic;
        return ShieldRules.Blocks(shielded, attacker.PlayerId, target.PlayerId, ignoresDefense, MeetingHud.Instance || ExileController.Instance);
    }

    public static PlayerControl? MedicOf(PlayerControl target)
    {
        if (!target.TryGetModifier<MedicShieldModifier>(out var shield) || shield.CurrentMedicId is not { } medicId)
        {
            return null;
        }

        foreach (var role in CustomRoleUtils.GetActiveRolesOfType<MedicRole>())
        {
            if (role.Player != null && role.Player.PlayerId == medicId)
            {
                return role.Player;
            }
        }

        return null;
    }
}
