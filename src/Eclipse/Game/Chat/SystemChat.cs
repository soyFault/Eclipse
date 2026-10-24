using Eclipse.Core;
using UnityEngine;

namespace Eclipse.Game;

internal static class SystemChat
{
    private static readonly Color TitleColor = new(0.545f, 0.992f, 0.992f);

    public static void Add(string title, string message)
    {
        var chat = HudManager.InstanceExists ? HudManager.Instance.Chat : null;
        if (chat == null)
        {
            return;
        }

        var bubble = chat.GetPooledBubble();
        bubble.transform.SetParent(chat.scroller.Inner);
        bubble.transform.localScale = Vector3.one;
        bubble.SetLeft();

        bubble.NameText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(TitleColor)}>{title}</color>";
        bubble.NameText.color = Color.white;
        bubble.NameText.ForceMeshUpdate(true, true);
        bubble.votedMark.enabled = false;
        bubble.Xmark.enabled = false;
        bubble.TextArea.text = "<line-height=1em>" + ChatContrast.LiftColors(message);
        bubble.TextArea.color = Color.white;
        bubble.TextArea.ForceMeshUpdate(true, true);
        bubble.Background.color = new Color(0.07f, 0.07f, 0.1f, 1f);
        bubble.Background.size = new Vector2(5.52f, 0.2f + bubble.NameText.GetNotDumbRenderedHeight() + bubble.TextArea.GetNotDumbRenderedHeight());
        bubble.MaskArea.size = bubble.Background.size - new Vector2(0, 0.03f);

        bubble.NameText.transform.localPosition = new Vector3(-0.2f, 0.358f, 0);
        bubble.Background.transform.localPosition = new Vector3(2.735f, -1.5801f, 0);
        bubble.Background.transform.localScale = new Vector3(1.115f, 1, 1);
        bubble.MaskArea.transform.localPosition = new Vector3(2.725f, -1.5601f, 0.1f);
        bubble.MaskArea.transform.localScale = new Vector3(1.12f, 1, 1);
        bubble.NameText.rectTransform.sizeDelta = new Vector2(6, bubble.NameText.rectTransform.sizeDelta.y);
        bubble.Player.transform.localScale = Vector3.zero;
        bubble.ColorBlindName.transform.localScale = Vector3.zero;

        bubble.AlignChildren();
        var textPosition = bubble.TextArea.transform.localPosition;
        bubble.TextArea.transform.localPosition = new Vector3(bubble.NameText.transform.localPosition.x, textPosition.y, textPosition.z);
        chat.AlignAllBubbles();
        if (!chat.IsOpenOrOpening && chat.notificationRoutine == null)
        {
            chat.notificationRoutine = chat.StartCoroutine(chat.BounceDot());
        }
    }
}
