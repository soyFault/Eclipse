using System.Collections.Generic;
using System.Linq;

namespace Eclipse.Core;

internal enum ProtectionKind : byte
{
    Fortify = 0,
    Barrier = 1,
}

internal readonly record struct Protection(ProtectionKind Kind, byte ProtectorId, byte TargetId, double ExpiresAt);

internal sealed class ProtectionLedger
{
    private readonly List<Protection> _items = [];

    public int Count => _items.Count;

    public void Add(ProtectionKind kind, byte protectorId, byte targetId, double expiresAt = double.PositiveInfinity)
    {
        _items.RemoveAll(p => p.Kind == kind && p.ProtectorId == protectorId && p.TargetId == targetId);
        _items.Add(new Protection(kind, protectorId, targetId, expiresAt));
    }

    public void Set(ProtectionKind kind, byte protectorId, byte targetId, double expiresAt = double.PositiveInfinity)
    {
        _items.RemoveAll(p => p.Kind == kind && p.ProtectorId == protectorId);
        _items.Add(new Protection(kind, protectorId, targetId, expiresAt));
    }

    public Protection? BlockerOf(byte targetId, double now)
    {
        foreach (var item in _items)
        {
            if (item.TargetId == targetId && item.ExpiresAt > now)
            {
                return item;
            }
        }

        return null;
    }

    public bool IsProtected(byte targetId, double now)
    {
        return BlockerOf(targetId, now) != null;
    }

    public bool Has(ProtectionKind kind, byte targetId, double now)
    {
        return _items.Any(p => p.Kind == kind && p.TargetId == targetId && p.ExpiresAt > now);
    }

    public IReadOnlyList<byte> TargetsOf(ProtectionKind kind, byte protectorId, double now)
    {
        return _items.Where(p => p.Kind == kind && p.ProtectorId == protectorId && p.ExpiresAt > now).Select(p => p.TargetId).ToList();
    }

    public IReadOnlyList<Protection> Active(ProtectionKind kind, double now)
    {
        return _items.Where(p => p.Kind == kind && p.ExpiresAt > now).ToList();
    }

    public int RemoveTarget(byte targetId)
    {
        return _items.RemoveAll(p => p.TargetId == targetId);
    }

    public int RemoveProtector(byte protectorId, ProtectionKind? onlyKind = null)
    {
        return _items.RemoveAll(p => p.ProtectorId == protectorId && (onlyKind == null || p.Kind == onlyKind));
    }

    public int RemoveKind(ProtectionKind kind)
    {
        return _items.RemoveAll(p => p.Kind == kind);
    }

    public int Prune(double now)
    {
        return _items.RemoveAll(p => p.ExpiresAt <= now);
    }

    public void Clear()
    {
        _items.Clear();
    }
}
