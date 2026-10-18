using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MiraAPI;
using MiraAPI.PluginLoading;
using MiraAPI.Translation;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;

namespace Eclipse;

/// <summary>
/// The Eclipse plugin: BepInEx loads it, Mira API scans it for roles, buttons and options.
/// </summary>
[BepInAutoPlugin("fault.eclipse", "Eclipse")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency(MiraApiPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class EclipsePlugin : BasePlugin, IMiraPlugin
{
    private Harmony Harmony { get; } = new(Id);

    string IMiraPlugin.OptionsTitleText => "Eclipse";

    ConfigFile IMiraPlugin.GetConfigFile()
    {
        return Config;
    }

    /// <inheritdoc />
    /// <remarks>BepInEx calls this ONCE, at start-up. If it throws, the mod does not load: read the log.</remarks>
    public override void Load()
    {
        MiraLocaleManager.Register(Id, "Eclipse");

        Harmony.PatchAll();

        Log.LogInfo($"{Name} {Version} cargado.");
    }
}
