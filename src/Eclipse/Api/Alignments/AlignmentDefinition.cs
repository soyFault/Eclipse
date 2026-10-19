using Eclipse.Api.Factions;
using MiraAPI.Roles;
using MiraAPI.Translation;
using UnityEngine;

namespace Eclipse.Api.Alignments;

public sealed record AlignmentDefinition(
    string Id,
    Faction Faction,
    string LocaleKey,
    Color Color,
    int SortOrder = 0)
{
    public string DisplayName => MiraLocaleManager.Get(LocaleKey);

    public RoleOptionsGroup OptionsGroup => new(LocaleKey + ".Group", Color, SortOrder);
}
