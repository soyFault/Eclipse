using Eclipse.Ui;
using MiraAPI.Hud;
using MiraAPI.PluginLoading;
using UnityEngine;

namespace Eclipse.Buttons;

[MiraIgnore]
internal abstract class EclipseActionButton : CustomActionButton
{
    private SpriteRenderer? _commsIcon;
    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        _commsIcon = ButtonCommsIcon.Find(Button);
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        ButtonCommsIcon.Hide(_commsIcon);
    }
}

[MiraIgnore]
internal abstract class EclipseActionButton<T> : CustomActionButton<T>
    where T : MonoBehaviour
{
    private SpriteRenderer? _commsIcon;


    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        _commsIcon = ButtonCommsIcon.Find(Button);
    }


    protected override void FixedUpdate(PlayerControl playerControl)
    {
        ButtonCommsIcon.Hide(_commsIcon);
    }
}
