// Portions derived from earlier GPLv3 work; see ATTRIBUTIONS.md.
using Eclipse.Api.Alignments;
using Eclipse.Api.Factions;
using Eclipse.Core;
using UnityEngine;

namespace Eclipse.Colors;

internal static class EclipseColors
{
    // ---- Alignments ----
    public static Color CrewInvestigative { get; } = FactionColors.Crewmate;

    public static Color CrewKilling { get; } = FactionColors.Crewmate;

    public static Color CrewProtective { get; } = FactionColors.Crewmate;

    public static Color CrewPower { get; } = FactionColors.Crewmate;

    public static Color CrewSupport { get; } = FactionColors.Crewmate;

    public static Color CrewAfterlife { get; } = FactionColors.Crewmate;

    public static Color CrewGhost { get; } = FactionColors.Crewmate;

    public static Color ImpConcealing { get; } = FactionColors.Impostor;

    public static Color ImpKilling { get; } = FactionColors.Impostor;

    public static Color ImpPower { get; } = FactionColors.Impostor;

    public static Color ImpSupport { get; } = FactionColors.Impostor;

    public static Color ImpAfterlife { get; } = FactionColors.Impostor;

    public static Color ImpGhost { get; } = FactionColors.Impostor;

    public static Color NeutralBenign { get; } = FactionColors.Neutral;

    public static Color NeutralEvil { get; } = FactionColors.Neutral;

    public static Color NeutralKilling { get; } = FactionColors.Neutral;

    public static Color NeutralOutlier { get; } = FactionColors.Neutral;

    public static Color NeutralAfterlife { get; } = FactionColors.Neutral;

    public static Color NeutralGhost { get; } = FactionColors.Neutral;

    // ---- Roles ----
    public static Color Sheriff { get; } = new Color32(0xFA, 0xD2, 0x07, 0xFF);

    public static Color Medic { get; } = new Color32(0x3D, 0xD9, 0x7A, 0xFF);

    public static Color Engineer { get; } = new Color32(0xFF, 0xA6, 0x0A, 0xFF);

    public static Color Aurial { get; } = new Color32(0xB3, 0x4D, 0x99, 0xFF);

    public static Color Forensic { get; } = new Color32(0x4D, 0x4D, 0xFF, 0xFF);

    public static Color Investigator { get; } = new Color32(0x00, 0xB3, 0xB3, 0xFF);

    public static Color Lookout { get; } = new Color32(0x33, 0xFF, 0x66, 0xFF);

    public static Color Medium { get; } = new Color32(0xA6, 0x80, 0xFF, 0xFF);

    public static Color Seer { get; } = new Color32(0xFF, 0xCC, 0x80, 0xFF);

    public static Color Spy { get; } = new Color32(0xCC, 0xA3, 0xCC, 0xFF);

    public static Color Trapper { get; } = new Color32(0xA6, 0xD1, 0xB3, 0xFF);

    public static Color Deputy { get; } = new Color32(0xFF, 0xCC, 0x00, 0xFF);

    public static Color Hunter { get; } = new Color32(0x29, 0xAB, 0x87, 0xFF);

    public static Color Officer { get; } = new Color32(0x37, 0x65, 0xDB, 0xFF);

    public static Color Vigilante { get; } = new Color32(0xFF, 0xFF, 0x99, 0xFF);

    public static Color Altruist { get; } = new Color32(0x66, 0x00, 0x00, 0xFF);

    public static Color Cleric { get; } = new Color32(0x00, 0xFF, 0xB3, 0xFF);

    public static Color Mirrorcaster { get; } = new Color32(0x90, 0xA2, 0xC3, 0xFF);

    public static Color Oracle { get; } = new Color32(0xBF, 0x00, 0xBF, 0xFF);

    public static Color Warden { get; } = new Color32(0x99, 0x00, 0xFF, 0xFF);

    public static Color Jailor { get; } = new Color32(0xA6, 0xA6, 0xA6, 0xFF);

    public static Color Monarch { get; } = new Color32(0xEA, 0x53, 0x5B, 0xFF);

    public static Color Prosecutor { get; } = new Color32(0xB3, 0x80, 0x00, 0xFF);

    public static Color Swapper { get; } = new Color32(0x66, 0xE6, 0x66, 0xFF);

    public static Color TimeLord { get; } = new Color32(0x87, 0x89, 0xD3, 0xFF);

    public static Color Imitator { get; } = new Color32(0xB3, 0xD9, 0x4D, 0xFF);

    public static Color Sentry { get; } = new Color32(0x64, 0x96, 0xC8, 0xFF);

    public static Color Spectre { get; } = new Color32(102, 41, 97, 255);

    public static Color Haunter { get; } = new Color32(0xD4, 0xD4, 0xD4, 0xFF);

    public static Color HaunterRevealed { get; } = new Color32(150, 50, 50, 255);

    public static Color Plumber { get; } = new Color32(0xCC, 0x77, 0x00, 0xFF);

    public static Color Transporter { get; } = new Color32(0x00, 0xED, 0xFF, 0xFF);

    public static Color Veteran { get; } = new Color32(0x99, 0x80, 0x40, 0xFF);

    public static Color ImpSoft { get; } = new Color32(214, 64, 66, 255);

    public static Color Plaguebearer { get; } = new Color32(230, 255, 179, 255);

    public static Color Pestilence { get; } = new Color32(77, 77, 77, 255);

    public static Color Vampire { get; } = new Color32(163, 41, 41, 255);

    public static Color Amnesiac { get; } = new Color32(128, 179, 255, 255);

    public static Color Blackmailer { get; } = FactionColors.Impostor;

    public static Color Escapist { get; } = FactionColors.Impostor;

    public static Color Warlock { get; } = FactionColors.Impostor;

    public static Color Bomber { get; } = FactionColors.Impostor;

    public static Color Janitor { get; } = FactionColors.Impostor;

    public static Color Morphling { get; } = FactionColors.Impostor;

    public static Color Swooper { get; } = FactionColors.Impostor;

    public static Color Venerer { get; } = FactionColors.Impostor;

    public static Color Glitch { get; } = Color.green;

    public static Color Undertaker { get; } = FactionColors.Impostor;

    public static Color Ambusher { get; } = FactionColors.Impostor;

    public static Color Miner { get; } = FactionColors.Impostor;

    public static Color Ambassador { get; } = FactionColors.Impostor;

    public static Color Traitor { get; } = FactionColors.Impostor;

    public static Color Spellslinger { get; } = FactionColors.Impostor;

    public static Color Herbalist { get; } = FactionColors.Impostor;

    public static Color Hypnotist { get; } = FactionColors.Impostor;

    public static Color Puppeteer { get; } = FactionColors.Impostor;

    public static Color Parasite { get; } = FactionColors.Impostor;

    public static Color HypnosisMark { get; } = new Color32(0xD5, 0x3F, 0x42, 0xFF);

    public static Color Mystic { get; } = new Color32(0x4D, 0x99, 0xE6, 0xFF);

    public static Color Sonar { get; } = new Color32(0x4E, 0xCF, 0x88, 0xFF);

    public static Color Scavenger { get; } = FactionColors.Impostor;

    public static Color Sniper { get; } = new Color32(0xDC, 0x28, 0x28, 0xFF);

    public static Color Executioner { get; } = new Color32(0x63, 0x3B, 0x1F, 0xFF);

    public static Color Snitch { get; } = new Color32(0xD4, 0xB0, 0x38, 0xFF);

    public static Color Politician { get; } = new Color32(0x66, 0x00, 0x99, 0xFF);

    public static Color Mayor { get; } = new Color32(0x70, 0x4F, 0xA8, 0xFF);

    public static Color Doomsayer { get; } = new Color32(0x00, 0xFF, 0x80, 0xFF);

    public static Color Jester { get; } = new Color32(0xF5, 0xA6, 0xCB, 0xFF);

    public static Color Juggernaut { get; } = new Color32(0x8C, 0x00, 0x4D, 0xFF);

    public static Color Werewolf { get; } = new Color32(0xA8, 0x66, 0x29, 0xFF);

    public static Color Mercenary { get; } = new Color32(140, 102, 153, 255);

    public static Color Fairy { get; } = new Color32(0xB3, 0xFF, 0xFF, 0xFF);

    public static Color Survivor { get; } = new Color32(0xFF, 0xE6, 0x4D, 0xFF);

    public static Color Arsonist { get; } = new Color32(0xFF, 0x4D, 0x00, 0xFF);

    public static Color Amalgam { get; } = new Color32(0xFF, 0x84, 0xCD, 0xFF);

    public static Color ForAlignment(string alignmentId, Faction faction)
    {
        return alignmentId switch
        {
            AlignmentIds.CrewInvestigative => CrewInvestigative,
            AlignmentIds.CrewKilling => CrewKilling,
            AlignmentIds.CrewProtective => CrewProtective,
            AlignmentIds.CrewPower => CrewPower,
            AlignmentIds.CrewSupport => CrewSupport,
            AlignmentIds.CrewAfterlife => CrewAfterlife,
            AlignmentIds.CrewGhost => CrewGhost,
            AlignmentIds.ImpConcealing => ImpConcealing,
            AlignmentIds.ImpKilling => ImpKilling,
            AlignmentIds.ImpPower => ImpPower,
            AlignmentIds.ImpSupport => ImpSupport,
            AlignmentIds.ImpAfterlife => ImpAfterlife,
            AlignmentIds.ImpGhost => ImpGhost,
            AlignmentIds.NeutralBenign => NeutralBenign,
            AlignmentIds.NeutralEvil => NeutralEvil,
            AlignmentIds.NeutralKilling => NeutralKilling,
            AlignmentIds.NeutralOutlier => NeutralOutlier,
            AlignmentIds.NeutralAfterlife => NeutralAfterlife,
            AlignmentIds.NeutralGhost => NeutralGhost,
            _ => FactionColors.Of(faction),
        };
    }
}
