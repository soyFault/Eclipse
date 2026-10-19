using System;
using System.Collections.Generic;
using System.Linq;
using Eclipse.Core;

namespace Eclipse.Api.Alignments;

public static class AlignmentRegistry
{
    private static readonly IdRegistry<AlignmentDefinition> Definitions = new(x => x.Id);

    public static IReadOnlyList<AlignmentDefinition> All
    {
        get
        {
            return Definitions.Snapshot().OrderBy(x => x.SortOrder).ToList();
        }
    }
    public static AlignmentDefinition Register(AlignmentDefinition definition)
    {
        return Definitions.Register(definition);
    }

    public static bool TryGet(string id, out AlignmentDefinition definition)
    {
        return Definitions.TryGet(id, out definition);
    }
}
