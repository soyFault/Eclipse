using Eclipse.Ui;
using MiraAPI.Hud;
using MiraAPI.PluginLoading;
using UnityEngine;

namespace Eclipse.Buttons;

[MiraIgnore]
internal abstract class EclipseActionButton : CustomActionButton
{
    private SpriteRenderer? _commsIcon;
    private SpriteRenderer? _roundLock;
    private SpriteRenderer? _commsLock;

    protected virtual bool IsRoundLocked => false;

    protected virtual bool IsCommsLocked => false;
    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        _commsIcon = ButtonCommsIcon.Find(Button);
        _roundLock = ButtonRoundLock.Create(Button);
        _commsLock = ButtonCommsLock.Create(Button, _commsIcon);
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        ButtonCommsIcon.Hide(_commsIcon);
        ButtonRoundLock.Set(_roundLock, IsRoundLocked);
        ButtonCommsLock.Set(_commsLock, IsCommsLocked);
    }
}

[MiraIgnore]
internal abstract class EclipseActionButton<T> : CustomActionButton<T>
    where T : MonoBehaviour
{
    private SpriteRenderer? _commsIcon;
    private SpriteRenderer? _roundLock;
    private SpriteRenderer? _commsLock;

    protected virtual bool IsRoundLocked => false;

    protected virtual bool IsCommsLocked => false;


    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        _commsIcon = ButtonCommsIcon.Find(Button);
        _roundLock = ButtonRoundLock.Create(Button);
        _commsLock = ButtonCommsLock.Create(Button, _commsIcon);
    }


    protected override void FixedUpdate(PlayerControl playerControl)
    {
        ButtonCommsIcon.Hide(_commsIcon);
        ButtonRoundLock.Set(_roundLock, IsRoundLocked);
        ButtonCommsLock.Set(_commsLock, IsCommsLocked && !IsRoundLocked);
    }
}
