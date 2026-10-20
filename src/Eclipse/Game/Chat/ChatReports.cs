using Eclipse.Core;
using UnityEngine;

namespace Eclipse.Game;

internal static class ChatReports
{
    public static void Add(PlayerControl sender, string title, Color titleColor, string body, bool? onLeft = null)
    {
        var chat = HudManager.Instance != null ? HudManager.Instance.Chat : null;
        if (chat == null)
        {
            return;
        }

        chat.AddChat(sender, body, false);
        FaultGuard.Run("A chat report's title", () =>
        {
            var bubbles = chat.chatBubblePool.activeChildren;
            if (bubbles == null || bubbles.Count == 0)
            {
                return;
            }
            var bubble = bubbles[bubbles.Count - 1].TryCast<ChatBubble>();
            if (bubble != null && bubble.NameText != null)
            {
                bubble.NameText.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(titleColor)}>{title}</color>";
            }

            if (bubble != null && onLeft == true)
            {
                bubble.SetLeft();
            }
            else if (bubble != null && onLeft == false)
            {
                bubble.SetRight();
            }
        });
    }
}
