using System.Collections.Generic;
using System.Linq;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Modifiers;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.Modifiers;
using UnityEngine;

namespace Eclipse.Game;

internal static class Mirrors
{
    public sealed record Mirror(byte Mirrorcaster, double ExpiresAt);

    // Every game keeps its own copy of this book, kept in step by the messages (and ended by the clock, in all of them, when a mirror runs out).
    public static Dictionary<byte, Mirror> Carried { get; } = [];

    public static double Now => Time.timeAsDouble;

    public static void Place(byte mirrorcasterId, byte targetId, float seconds)
    {
        RemoveOf(mirrorcasterId);
        Carried[targetId] = new Mirror(mirrorcasterId, Now + seconds);
    }

    public static void RemoveOf(byte mirrorcasterId)
    {
        foreach (var target in Carried.Where(m => m.Value.Mirrorcaster == mirrorcasterId).Select(m => m.Key).ToList())
        {
            Carried.Remove(target);
        }
    }

    public static bool HasMirrorOf(byte mirrorcasterId)
    {
        return Carried.Values.Any(m => m.Mirrorcaster == mirrorcasterId && m.ExpiresAt > Now);
    }

    public static byte? ProtectedBy(byte mirrorcasterId)
    {
        foreach (var carried in Carried)
        {
            if (carried.Value.Mirrorcaster == mirrorcasterId && carried.Value.ExpiresAt > Now)
            {
                return carried.Key;
            }
        }

        return null;
    }

    public static Mirror? On(byte targetId)
    {
        return Carried.TryGetValue(targetId, out var mirror) && mirror.ExpiresAt > Now ? mirror : null;
    }

    // The mirror that stops source from attacking target right now, or null.
    public static Mirror? Stopping(PlayerControl source, PlayerControl target, bool ignoresDefense)
    {
        var mirror = On(target.PlayerId);
        var stops = MirrorcasterRules.Stops(
            mirror != null,
            source.PlayerId == target.PlayerId,
            ignoresDefense,
            source.HasModifier<VeteranAlertModifier>(),
            MeetingHud.Instance || ExileController.Instance);
        return stops ? mirror : null;
    }

    // Registered as a local tick: forgets the mirrors that ran out and keeps the drawings up to date.
    public static void Tick(float deltaTime)
    {
        foreach (var target in Carried.Where(m => m.Value.ExpiresAt <= Now).Select(m => m.Key).ToList())
        {
            Carried.Remove(target);
        }

        MirrorVisuals.Sync();
    }

    public static void Reset()
    {
        Carried.Clear();
        MirrorVisuals.Sync();
    }

    // The mark 〚〛 next to the name of a mirrored player, for the Mirrorcaster and the dead. Registered as a name mark provider.
    public static string? MirrorMark(PlayerControl viewer, PlayerControl target)
    {
        if (On(target.PlayerId) is not { } mirror
            || !MirrorcasterRules.ShowsMirror(viewer.PlayerId == mirror.Mirrorcaster && viewer.Data.Role is MirrorcasterRole, viewer.Data.IsDead))
        {
            return null;
        }

        return $" <color=#{ColorUtility.ToHtmlStringRGB(EclipseColors.Mirrorcaster)}>〚〛</color>";
    }
}
