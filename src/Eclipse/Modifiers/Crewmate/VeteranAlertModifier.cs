using Eclipse.Assets;
using Eclipse.Core;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Killing;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using UnityEngine;

namespace Eclipse.Modifiers;

internal sealed class VeteranAlertModifier : TimedModifier
{
    // Where the shield sits in the player's cosmetics layer, and how long it takes to fade out at the end of the alert.
    private const float ShieldY = -0.05f;
    private const float ShieldZ = 1.5f;
    private const float FadeSeconds = 1f;

    // Orbit of the emblem around the player (local units of the prefab). Same as the Medic's shield.
    private const float OrbitRadiusX = 1.0f;
    private const float OrbitRadiusY = 0.5f;
    private const float OrbitCenterY = -0.1f;
    private const float OrbitSeconds = 3f;
    private const float EmblemScale = 0.6f;
    private const float DepthScale = 0.18f;

    private GameObject? _holder;
    private SpriteRenderer[] _renderers = [];
    private Color[] _baseColors = [];
    private Transform? _emblem;
    private float _alpha = 1f;

    public override string IdPrefix => "Eclipse.Modifier";

    public override string IdPart => "VeteranAlert";

    public override float Duration => OptionGroupSingleton<VeteranOptions>.Instance.AlertDuration;

    public override bool HideOnUi => true;

    public override void OnActivate()
    {
        var prefab = EclipseBundle.Find<GameObject>(BundleArt.PrefabsFolder, "VeteranAlert");
        if (prefab == null || Player == null)
        {
            return;
        }

        var shield = Object.Instantiate(prefab, Player.transform);
        shield.name = "EclipseVeteranAlert";
        var playerScale = Player.transform.localScale;
        shield.transform.localScale = new Vector3(prefab.transform.localScale.x / playerScale.x, prefab.transform.localScale.y / playerScale.y, 1f);

        _holder = new GameObject("A_VeteranAlert");
        _holder.transform.SetParent(Player.cosmetics.transform, false);
        _holder.transform.localPosition = new Vector3(0f, ShieldY, ShieldZ);
        shield.transform.SetParent(_holder.transform, true);

        _renderers = shield.GetComponentsInChildren<SpriteRenderer>(true);
        _baseColors = new Color[_renderers.Length];
        for (var i = 0; i < _renderers.Length; i++)
        {
            _baseColors[i] = _renderers[i].color;
        }

        // The aura animates itself with the game's SpriteAnim. The emblem is moved by FixedUpdate below.
        _emblem = shield.transform.Find("VeteranAlertOrbit/Emblem");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_holder == null || Player == null || MeetingHud.Instance)
        {
            return;
        }

        var local = PlayerControl.LocalPlayer;
        var visible = local != null
                      && local.Data != null
                      && !Player.inVent
                      && VeteranAlertRules.ShowsShield(OptionGroupSingleton<VeteranOptions>.Instance.EveryoneSeesShield, Player.AmOwner, local.Data.IsDead);
        _holder.SetActive(visible);

        if (_emblem != null)
        {
            var angle = Time.time * (2f * Mathf.PI / OrbitSeconds);
            var sin = Mathf.Sin(angle);
            var scale = EmblemScale * (1f - DepthScale * sin);
            _emblem.localPosition = new Vector3(OrbitRadiusX * Mathf.Cos(angle), OrbitCenterY + OrbitRadiusY * sin, sin > 0f ? 0.00006f : -0.00005f);
            _emblem.localScale = new Vector3(scale, scale, 1f);
        }

        var flip = Player.cosmetics.FlipX;
        foreach (var renderer in _renderers)
        {
            renderer.flipX = flip;
        }

        if (TimerActive)
        {
            SetAlpha(VeteranAlertRules.FadeAlpha(TimeRemaining, FadeSeconds));
        }
    }

    public override void OnTimerComplete()
    {
        SetAlpha(0f);
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();

        if (_holder != null)
        {
            Object.Destroy(_holder);
        }

        _holder = null;
        _renderers = [];
        _baseColors = [];
        _emblem = null;

        // The alert is spent when it ends, not when it starts: until then the Veteran still counts as able to alert for the match rules.
        if (Player != null && Player.Data?.Role is VeteranRole veteran)
        {
            veteran.Alerts--;
        }
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent?.RemoveModifier(this);
    }

    public override void OnMeetingStart()
    {
        ModifierComponent?.RemoveModifier(this);
    }

    // Only touches the renderers when the value really changes: nothing is done during the rest of the alert.
    private void SetAlpha(float alpha)
    {
        if (Mathf.Approximately(alpha, _alpha))
        {
            return;
        }

        _alpha = alpha;
        for (var i = 0; i < _renderers.Length; i++)
        {
            var color = _baseColors[i];
            color.a *= alpha;
            _renderers[i].color = color;
        }
    }
}
