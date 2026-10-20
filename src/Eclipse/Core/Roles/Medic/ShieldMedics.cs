using System.Collections.Generic;

namespace Eclipse.Core;

internal sealed class ShieldMedics
{
    private readonly List<byte> _all = [];

    public byte? Current { get; private set; }

    public bool IsEmpty => _all.Count == 0;

    public bool Contains(byte medic) => _all.Contains(medic);

    public IReadOnlyList<byte> All => _all;

    public void Add(byte medic)
    {
        if (!_all.Contains(medic))
        {
            _all.Add(medic);
        }

        Current = medic;
    }

    public void Remove(byte medic)
    {
        _all.Remove(medic);
        if (_all.Count == 0)
        {
            Current = null;
        }
        else if (Current == medic)
        {
            Current = _all[0];
        }
    }

    public void ShiftNext(byte after)
    {
        if (_all.Count < 2)
        {
            return;
        }

        var index = _all.IndexOf(after);
        Current = _all[(index + 1) % _all.Count];
    }
}
