using Eclipse.Api.Factions;
using Eclipse.Colors;
using Eclipse.Core;

namespace Eclipse.Api.Alignments;

public static class BuiltInAlignments
{
    private const string Prefix = "Eclipse.Alignment.";

    // ---- Crewmate ----

    /// <summary>Gets crewmates that gather information.</summary>
    public static AlignmentDefinition CrewInvestigative { get; } = Create(AlignmentIds.CrewInvestigative, Faction.Crewmate, "CrewInvestigative", 0);

    /// <summary>Gets crewmates that can kill under conditions.</summary>
    public static AlignmentDefinition CrewKilling { get; } = Create(AlignmentIds.CrewKilling, Faction.Crewmate, "CrewKilling", 1);

    /// <summary>Gets crewmates that protect others.</summary>
    public static AlignmentDefinition CrewProtective { get; } = Create(AlignmentIds.CrewProtective, Faction.Crewmate, "CrewProtective", 2);

    /// <summary>Gets crewmates with strong influence over meetings or the match.</summary>
    public static AlignmentDefinition CrewPower { get; } = Create(AlignmentIds.CrewPower, Faction.Crewmate, "CrewPower", 3);

    /// <summary>Gets crewmates that assist the team with utility.</summary>
    public static AlignmentDefinition CrewSupport { get; } = Create(AlignmentIds.CrewSupport, Faction.Crewmate, "CrewSupport", 4);

    /// <summary>Gets crewmate roles that act after death through a special mechanic.</summary>
    public static AlignmentDefinition CrewAfterlife { get; } = Create(AlignmentIds.CrewAfterlife, Faction.Crewmate, "CrewAfterlife", 5);

    /// <summary>Gets crewmate ghost roles (assigned on death).</summary>
    public static AlignmentDefinition CrewGhost { get; } = Create(AlignmentIds.CrewGhost, Faction.Crewmate, "CrewGhost", 6);

    // ---- Impostor ----

    /// <summary>Gets impostors that hide kills or identities.</summary>
    public static AlignmentDefinition ImpConcealing { get; } = Create(AlignmentIds.ImpConcealing, Faction.Impostor, "ImpConcealing", 100);

    /// <summary>Gets impostors with enhanced killing abilities.</summary>
    public static AlignmentDefinition ImpKilling { get; } = Create(AlignmentIds.ImpKilling, Faction.Impostor, "ImpKilling", 101);

    /// <summary>Gets impostors with strong influence over the match.</summary>
    public static AlignmentDefinition ImpPower { get; } = Create(AlignmentIds.ImpPower, Faction.Impostor, "ImpPower", 102);

    /// <summary>Gets impostors that assist the team with utility.</summary>
    public static AlignmentDefinition ImpSupport { get; } = Create(AlignmentIds.ImpSupport, Faction.Impostor, "ImpSupport", 103);

    /// <summary>Gets impostor roles that act after death through a special mechanic.</summary>
    public static AlignmentDefinition ImpAfterlife { get; } = Create(AlignmentIds.ImpAfterlife, Faction.Impostor, "ImpAfterlife", 104);

    /// <summary>Gets impostor ghost roles (assigned on death).</summary>
    public static AlignmentDefinition ImpGhost { get; } = Create(AlignmentIds.ImpGhost, Faction.Impostor, "ImpGhost", 105);

    // ---- Neutral ----

    /// <summary>Gets neutrals that pose no threat and win by surviving or a simple goal.</summary>
    public static AlignmentDefinition NeutralBenign { get; } = Create(AlignmentIds.NeutralBenign, Faction.Neutral, "NeutralBenign", 200);

    /// <summary>Gets neutrals with a hostile or chaotic goal that does not require killing everyone.</summary>
    public static AlignmentDefinition NeutralEvil { get; } = Create(AlignmentIds.NeutralEvil, Faction.Neutral, "NeutralEvil", 201);

    /// <summary>Gets neutrals that must eliminate everyone to win.</summary>
    public static AlignmentDefinition NeutralKilling { get; } = Create(AlignmentIds.NeutralKilling, Faction.Neutral, "NeutralKilling", 202);

    /// <summary>Gets neutrals with an unconventional win condition.</summary>
    public static AlignmentDefinition NeutralOutlier { get; } = Create(AlignmentIds.NeutralOutlier, Faction.Neutral, "NeutralOutlier", 203);

    /// <summary>Gets neutral roles that act after death through a special mechanic.</summary>
    public static AlignmentDefinition NeutralAfterlife { get; } = Create(AlignmentIds.NeutralAfterlife, Faction.Neutral, "NeutralAfterlife", 204);

    /// <summary>Gets neutral ghost roles (assigned on death).</summary>
    public static AlignmentDefinition NeutralGhost { get; } = Create(AlignmentIds.NeutralGhost, Faction.Neutral, "NeutralGhost", 205);

    /// <summary>
    /// Registers every built-in alignment. Called once by the Eclipse plugin during <c>Load()</c>.
    /// </summary>
    public static void RegisterAll()
    {
        AlignmentRegistry.Register(CrewInvestigative);
        AlignmentRegistry.Register(CrewKilling);
        AlignmentRegistry.Register(CrewProtective);
        AlignmentRegistry.Register(CrewPower);
        AlignmentRegistry.Register(CrewSupport);
        AlignmentRegistry.Register(CrewAfterlife);
        AlignmentRegistry.Register(CrewGhost);

        AlignmentRegistry.Register(ImpConcealing);
        AlignmentRegistry.Register(ImpKilling);
        AlignmentRegistry.Register(ImpPower);
        AlignmentRegistry.Register(ImpSupport);
        AlignmentRegistry.Register(ImpAfterlife);
        AlignmentRegistry.Register(ImpGhost);

        AlignmentRegistry.Register(NeutralBenign);
        AlignmentRegistry.Register(NeutralEvil);
        AlignmentRegistry.Register(NeutralKilling);
        AlignmentRegistry.Register(NeutralOutlier);
        AlignmentRegistry.Register(NeutralAfterlife);
        AlignmentRegistry.Register(NeutralGhost);
    }

    private static AlignmentDefinition Create(string id, Faction faction, string localePart, int sortOrder)
    {
        return new AlignmentDefinition(id, faction, Prefix + localePart, EclipseColors.ForAlignment(id, faction), sortOrder);
    }
}
