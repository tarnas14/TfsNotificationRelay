namespace DevCore.TfsNotificationRelay.Slack
{
    using System.Collections.Generic;

    public class SlackConfiguration
    {
        public IEnumerable<string> Channels { get; set; }
        public string WebhookUrl { get; set; }

        public string Username { get; set; }
        public string IconUrl { get; set; }
        public string StandardColor { get; set; }
        public string SuccessColor { get; set; }
        public string ErrorColor { get; set; }
        public string IconEmoji { get; set; }

        public NotificationTextFormatting NotificationTextFormatting { get; set; }
    }
}