using System;
using System.Collections.Concurrent;
using Reactor.Utilities;

namespace Eclipse.Ui;

internal static class CosmeticGuard
{
    private static readonly ConcurrentDictionary<string, bool> Reported = new(StringComparer.Ordinal);

    public static void Run(string name, Action action)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            if (Reported.TryAdd(name, true))
            {
                Logger<EclipsePlugin>.Error($"Cosmetic effect '{name}' failed and was skipped (reported once): {exception}");
            }
        }
    }
}
