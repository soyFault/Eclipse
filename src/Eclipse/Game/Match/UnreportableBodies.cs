using System.Collections;
using MiraAPI.Utilities;
using Reactor.Utilities;
using UnityEngine;

namespace Eclipse.Game;

internal static class UnreportableBodies
{

    public static void Start(byte victimId)
    {
        Coroutines.Start(CoMakeBodyUnreportable(victimId));
    }

    private static IEnumerator CoMakeBodyUnreportable(byte victimId)
    {
        for (var waited = 0f; waited < 5f; waited += 0.1f)
        {
            var body = Helpers.GetBodyById(victimId);
            if (body != null)
            {
                body.gameObject.layer = LayerMask.NameToLayer("Ship");
                body.Reported = true;
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
