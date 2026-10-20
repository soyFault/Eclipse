using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using Eclipse.Assets;
using Eclipse.Api.Alignments;
using HarmonyLib;
using MiraAPI;
using MiraAPI.PluginLoading;
using MiraAPI.Translation;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;

namespace Eclipse;

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

    public override void Load()
    {
        MiraLocaleManager.Register(Id, "Eclipse");
        BuiltInAlignments.RegisterAll();
        Harmony.PatchAll();
        Log.LogInfo($"{Name} {Version} cargado.");
        EclipseBundle.Report();
    }
}
