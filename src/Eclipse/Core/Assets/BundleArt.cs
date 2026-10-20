namespace Eclipse.Core;

internal static class BundleArt
{
    public const string IconsFolder = "Icons";
    public const string ButtonsFolder = "Buttons";
    public const string ModifiersFolder = "Modifiers";
    public const string SpritesFolder = "Sprites";
    public const string FreeplayFolder = "Freeplay";
    public const string PrefabsFolder = "PreFabs";
    public static string Stem(string assetPath)
    {
        var slash = assetPath.LastIndexOfAny(['/', '\\']);
        var file = slash >= 0 ? assetPath[(slash + 1)..] : assetPath;
        var dot = file.LastIndexOf('.');
        var stem = dot > 0 ? file[..dot] : file;
        return stem.ToLowerInvariant();
    }

    public static string Folder(string assetPath)
    {
        var parts = assetPath.Split(['/', '\\']);
        return parts.Length >= 2 ? parts[^2].ToLowerInvariant() : string.Empty;
    }

    public static string Key(string folder, string name)
    {
        return $"{folder}/{name}".ToLowerInvariant();
    }


}
