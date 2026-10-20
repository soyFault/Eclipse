using System;
using System.Collections.Generic;

namespace Eclipse.Core;

internal sealed class IdRegistry<T>
    where T : class
{
    private readonly Dictionary<string, T> _byId = new(StringComparer.Ordinal);
    private readonly List<T> _inOrder = [];
    private readonly Func<T, string> _idOf;
    private readonly object _sync = new();

    public IdRegistry(Func<T, string> idOf)
    {
        _idOf = idOf ?? throw new ArgumentNullException(nameof(idOf));
    }

    public T Register(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var id = _idOf(item);
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("An item id cannot be null, empty or whitespace.", nameof(item));
        }

        lock (_sync)
        {
            if (_byId.TryGetValue(id, out var existing))
            {
                if (existing.Equals(item))
                {
                    return existing;
                }

                throw new InvalidOperationException($"An item with id '{id}' is already registered.");
            }

            _byId.Add(id, item);
            _inOrder.Add(item);
            return item;
        }
    }

    public bool TryGet(string id, out T item)
    {
        lock (_sync)
        {
            return _byId.TryGetValue(id, out item!);
        }
    }


    public IReadOnlyList<T> Snapshot()
    {
        lock (_sync)
        {
            return _inOrder.ToArray();
        }
    }
}
