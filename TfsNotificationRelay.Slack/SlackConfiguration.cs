namespace DevCore.TfsNotificationRelay.Slack
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;

    public class SlackConfiguration
    {
        public SlackConfiguration(BotElement slackBot)
        {
            Bot = slackBot;
            ReadSlackBot(slackBot);
        }

        private void ReadSlackBot(BotElement slackBot)
        {
            Channels = slackBot.GetSetting(ChannelsSetting)
                .Split(',')
                .Select(channel => channel.Trim());

            AllNotificationsShouldGoToAllChannels = Channels.Any();
        }

        public const string ChannelsSetting = "channels";

        public bool AllNotificationsShouldGoToAllChannels { get; private set; }

        public IEnumerable<string> Channels { get; private set; }

        public BotElement Bot { get; private set; }
    }
}