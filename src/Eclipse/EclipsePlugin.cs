using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using Eclipse.Assets;
using Eclipse.Core;
using Eclipse.Api.Alignments;
using Eclipse.Game;
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
        LocalTick.Register(Mirrors.Tick);
        NameDecorations.Register(Mirrors.MirrorMark);
        NameDecorations.Register(FirstRoundShield.Mark);
        MatchReset.Register(Targets.Reset);
        LocalTick.Register(Protections.Tick);
        RoleLabels.RegisterReveal(SnitchEvents.SnitchRoleIsPublic);
        RoleLabels.RegisterReveal(SleuthEvents.SleuthSees);
        NameColors.Register(SnitchEvents.NameColor);
        LocalTick.Register(SnitchEvents.Tick);
        LocalTick.Register(Footprints.Tick);
        LocalTick.Register(NameSightTweak.Tick);
        ChatCommandSetup.RegisterAll();
        Log.LogInfo($"{Name} {Version} cargado.");
        EclipseBundle.Report();
    }
}
