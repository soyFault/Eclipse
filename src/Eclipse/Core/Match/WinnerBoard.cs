using System.Collections.Generic;
using System.Linq;

namespace Eclipse.Core;

internal sealed class WinnerBoard<T>
{
    private readonly List<(byte PlayerId, T Detail)> _winners = [];

    public bool Ended { get; private set; }

    public bool HasPending => !Ended && _winners.Count > 0;

    public bool Declare(byte playerId, T detail)
    {
        if (_winners.Any(x => x.PlayerId == playerId))
        {
            return false;
        }

        _winners.Add((playerId, detail));
        return true;
    }

    public IReadOnlyList<byte> Ids => _winners.ConvertAll(x => x.PlayerId);

    public bool Contains(byte playerId)
    {
        return _winners.Any(x => x.PlayerId == playerId);
    }

    public T? Find(byte playerId)
    {
        foreach (var (id, detail) in _winners)
        {
            if (id == playerId)
            {
                return detail;
            }
        }

        return default;
    }

    public void MarkEnded()
    {
        Ended = true;
    }

    public void Clear()
    {
        _winners.Clear();
        Ended = false;
    }
}
