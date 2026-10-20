using System.Linq;
using Eclipse.Assets;
using Eclipse.Core;
using Eclipse.Options.Roles.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using MiraAPI.Modifiers;
using MiraAPI.Translation;

using UnityEngine;

namespace Eclipse.Modifiers;

internal sealed class MedicShieldModifier : BaseModifier
{

    private const float BubbleY = -0.05f;
    private const float BubbleZ = 1.5f;

    private readonly ShieldMedics _medics = new();
    private GameObject? _bubble;
    private SpriteRenderer[] _bubbleRenderers = [];
    private Transform? _shield;

    public MedicShieldModifier(PlayerControl medic)
    {
        _medics.Add(medic.PlayerId);
    }

    public override string IdPrefix => "Eclipse.Modifier";

    public override string IdPart => "MedicShield";

    public override LoadableAsset<Sprite>? ModifierIcon => EclipseAssets.ModifierIcon(IdPart);

    public byte? CurrentMedicId => _medics.Current;

    public bool HasNoMedic => _medics.IsEmpty;

    private static bool ShieldedKnows => OptionGroupSingleton<MedicOptions>.Instance.ShieldedKnows;

    public override bool HideOnUi => !ShieldedKnows;

    public override string GetDescription()
    {
        return MiraLocaleManager.Get("Eclipse.Modifier.MedicShield.Description");
    }

    public void AddMedic(PlayerControl medic)
    {
        _medics.Add(medic.PlayerId);
    }

    public void RemoveMedic(PlayerControl medic)
    {
        _medics.Remove(medic.PlayerId);
    }

    public void ShiftNextMedic(PlayerControl after)
    {
        _medics.ShiftNext(after.PlayerId);
    }

    public override void OnActivate()
    {
        var prefab = EclipseBundle.Find<GameObject>(BundleArt.PrefabsFolder, "MedicShield");
        if (prefab == null || Player == null)
        {
            return;
        }
        _bubble = Object.Instantiate(prefab, Player.transform);
        _bubble.name = "EclipseMedicShield";
        var playerScale = Player.transform.localScale;
        _bubble.transform.localScale = new Vector3(prefab.transform.localScale.x / playerScale.x, prefab.transform.localScale.y / playerScale.y, 1f);
        var holder = new GameObject("A_MedicShield");
        holder.transform.SetParent(Player.cosmetics.transform, false);
        holder.transform.localPosition = new Vector3(0f, BubbleY, BubbleZ);
        _bubble.transform.SetParent(holder.transform, true);
        _bubbleRenderers = _bubble.GetComponentsInChildren<SpriteRenderer>(true);
        _bubble.SetActive(ShowsBubble());
    }

    /// <inheritdoc />
    public override void OnDeactivate()
    {
        if (_bubble != null)
        {
            Object.Destroy(_bubble.transform.parent != null && _bubble.transform.parent.name == "A_MedicShield" ? _bubble.transform.parent.gameObject : _bubble);
        }

        _bubble = null;
        _bubbleRenderers = [];
    }

    private bool ShowsBubble()
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null || Player == null || local.Data == null)
        {
            return false;
        }

        return local.Data.IsDead || (Player.AmOwner && ShieldedKnows) || _medics.Contains(local.PlayerId);
    }

    /// <inheritdoc />
    public override void FixedUpdate()
    {
        if (_medics.IsEmpty)
        {
            ModifierComponent?.RemoveModifier(this);
            return;
        }

        if (_bubble != null && !MeetingHud.Instance)
        {

            if (_shield != null)
            {
                var angle = Time.time * (2f * Mathf.PI / 3f);
                _shield.localPosition = new Vector3(1.45f * Mathf.Cos(angle), -0.1f + 0.5f * Mathf.Sin(angle), Mathf.Sin(angle) > 0f ? 0.00006f : -0.00005f);
                var scale = 0.6f * (1f - 0.18f * Mathf.Sin(angle));
                _shield.localScale = new Vector3(scale, scale, 1f);
            }

            _bubble.SetActive(ShowsBubble());
            var flip = Player.cosmetics.FlipX;
            foreach (var renderer in _bubbleRenderers)
            {
                renderer.flipX = flip;
            }
        }
    }
}
