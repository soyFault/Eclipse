using System;

namespace Eclipse.Core;

internal readonly struct NameParts : IEquatable<NameParts>
{
    private readonly string? _prefix;
    private readonly string? _suffix;
    public NameParts(string prefix, string suffix)
    {
        _prefix = prefix;
        _suffix = suffix;
    }

    public string Prefix => _prefix ?? string.Empty;

    public string Suffix => _suffix ?? string.Empty;

    public bool IsEmpty => Prefix.Length == 0 && Suffix.Length == 0;

    public bool Equals(NameParts other)
    {
        return Prefix == other.Prefix && Suffix == other.Suffix;
    }

    public override bool Equals(object? obj)
    {
        return obj is NameParts other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Prefix, Suffix);
    }

    public static bool operator ==(NameParts left, NameParts right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NameParts left, NameParts right)
    {
        return !left.Equals(right);
    }
}

internal static class NameDecoration
{
    public static string Reconcile(string shown, NameParts current, NameParts wanted)
    {
        if (wanted != current)
        {
            shown = TextMark.RemovePrefix(TextMark.Remove(shown, current.Suffix), current.Prefix);
        }

        return TextMark.ApplyPrefix(TextMark.Apply(shown, wanted.Suffix), wanted.Prefix);
    }
}
