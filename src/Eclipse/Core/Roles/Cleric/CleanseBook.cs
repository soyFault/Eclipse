using System.Collections.Generic;

namespace Eclipse.Core;

internal sealed record CleanseEntry(byte TargetId, IReadOnlyList<string> Effects);

internal sealed class CleanseBook
{
    private readonly List<CleanseEntry> _entries = [];

    public int Count => _entries.Count;

    public void Add(byte targetId, IReadOnlyList<string> effects)
    {
        _entries.RemoveAll(e => e.TargetId == targetId);
        _entries.Add(new CleanseEntry(targetId, effects));
    }

    public IReadOnlyList<CleanseEntry> Drain()
    {
        var all = _entries.ToArray();
        _entries.Clear();
        return all;
    }

    public void Clear()
    {
        _entries.Clear();
    }
}
