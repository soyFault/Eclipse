using System.Collections.Generic;
using System.Linq;

namespace Eclipse.Core;

internal sealed record KillRecord(byte KillerId, byte VictimId, double Time);

internal sealed class KillLog
{
    private readonly List<KillRecord> _records = [];

    public int Count => _records.Count;

    public IReadOnlyList<KillRecord> All => _records;

    public void Add(byte killerId, byte victimId, double time)
    {
        _records.Add(new KillRecord(killerId, victimId, time));
    }

    public KillRecord? LatestOf(byte victimId)
    {
        return _records.Where(r => r.VictimId == victimId).OrderByDescending(r => r.Time).FirstOrDefault();
    }

    public bool HasKilled(byte killerId)
    {
        return _records.Any(r => r.KillerId == killerId && r.VictimId != killerId);
    }

    public void Clear()
    {
        _records.Clear();
    }
}
