namespace DevCore.TfsNotificationRelay.Slack
{
    using System.Collections.Generic;
    using Configuration;

    public class SlackConfiguration
    {
        public IEnumerable<string> Channels { get; set; }

        public BotElement Bot { get; set; }
    }
}