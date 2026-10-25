using System.Collections.Generic;
using System.Linq;

namespace Eclipse.Core;

internal enum TargetKind : byte
{
    Executioner = 0,
    Fairy = 1,
}

internal sealed class TargetLedger
{
    private readonly Dictionary<(TargetKind Kind, byte Holder), byte> _targets = [];

    public int Count => _targets.Count;

    public void Set(TargetKind kind, byte holderId, byte targetId)
    {
        _targets[(kind, holderId)] = targetId;
    }

    public byte? TargetOf(TargetKind kind, byte holderId)
    {
        return _targets.TryGetValue((kind, holderId), out var target) ? target : null;
    }

    public bool Has(TargetKind kind, byte holderId)
    {
        return _targets.ContainsKey((kind, holderId));
    }

    public bool IsTarget(TargetKind kind, byte playerId)
    {
        return _targets.Any(x => x.Key.Kind == kind && x.Value == playerId);
    }

    public IReadOnlyList<byte> HoldersOf(TargetKind kind, byte targetId)
    {
        return _targets.Where(x => x.Key.Kind == kind && x.Value == targetId).Select(x => x.Key.Holder).ToList();
    }

    public bool HasAsTarget(byte holderId, byte targetId)
    {
        return _targets.Any(x => x.Key.Holder == holderId && x.Value == targetId);
    }

    public bool Clear(TargetKind kind, byte holderId)
    {
        return _targets.Remove((kind, holderId));
    }

    public int ClearHolder(byte holderId)
    {
        var keys = _targets.Keys.Where(k => k.Holder == holderId).ToList();
        foreach (var key in keys)
        {
            _targets.Remove(key);
        }

        return keys.Count;
    }

    public int Transfer(byte fromId, byte toId)
    {
        var moved = _targets.Where(x => x.Key.Holder == fromId).ToList();
        foreach (var entry in moved)
        {
            _targets.Remove(entry.Key);
            _targets[(entry.Key.Kind, toId)] = entry.Value;
        }

        return moved.Count;
    }

    public void Clear()
    {
        _targets.Clear();
    }
}
