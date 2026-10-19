using System;
using System.Collections.Generic;
using System.Linq;
using Eclipse.Core;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace Eclipse.Assets;

internal static class EclipseBundle
{
    public const string Name = "eclipse-assets";

    private static readonly Dictionary<string, string> Paths = [];

    private static AssetBundle? _bundle;
    private static bool _tried;
    private static AssetBundle? Main
    {
        get
        {
            if (!_tried)
            {
                _tried = true;
                Load();
            }

            return _bundle;
        }
    }

    public static void Report()
    {
        if (Main == null)
        {
            return;
        }

        var keys = Paths.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList();
        Logger<EclipsePlugin>.Info($"Bundle: {keys.Count} asset(s) found by folder and name [{string.Join(", ", keys)}].");
    }

    public static bool Has(string folder, string name)
    {
        return Main != null && Paths.ContainsKey(BundleArt.Key(folder, name));
    }

    public static T? Find<T>(string folder, string name)
        where T : UnityEngine.Object
    {
        var bundle = Main;
        if (bundle == null || !Paths.TryGetValue(BundleArt.Key(folder, name), out var path))
        {
            return null;
        }

        try
        {
            var asset = bundle.LoadAsset<T>(path);
            if (asset == null)
            {
                Logger<EclipsePlugin>.Warning($"Bundle: '{path}' exists but is not a {typeof(T).Name}.");
                return null;
            }

            asset.DontUnload();
            return asset;
        }
        catch (Exception exception)
        {
            Logger<EclipsePlugin>.Error($"Bundle: '{path}' could not be loaded: {exception.Message}");
            return null;
        }
    }

    private static void Load()
    {
        try
        {
            _bundle = AssetBundleManager.Load(typeof(EclipseBundle).Assembly, Name);
        }
        catch (AssetBundleNotFoundException)
        {
            var embedded = typeof(EclipseBundle).Assembly.GetManifestResourceNames().Where(n => n.Contains("bundle", StringComparison.OrdinalIgnoreCase)).ToList();
            Logger<EclipsePlugin>.Warning(
                embedded.Count > 0
                    ? $"Bundle: [{string.Join(", ", embedded)}] is embedded but the game could not open it. Look for a Unity error just before this line in LogOutput.log. Check that the AssetBundle module is enabled in Packages/manifest.json and that Unity is 2022.3.44f1."
                    : "Bundle: no bundle is embedded. The .bundle files must be in Resources/ BEFORE compiling.");
            return;
        }
        catch (Exception exception)
        {
            Logger<EclipsePlugin>.Error($"Bundle: '{Name}' could not be loaded: {exception}");
            return;
        }

        foreach (var path in _bundle.GetAllAssetNames())
        {
            var stem = BundleArt.Stem(path);
            var folder = BundleArt.Folder(path);
            if (stem.Length != 0 && folder.Length != 0)
            {
                Paths.TryAdd(BundleArt.Key(folder, stem), path);
            }
        }
    }
}
