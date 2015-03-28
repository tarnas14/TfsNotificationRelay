namespace DevCore.TfsNotificationRelay.Slack
{
    using System.Linq;
    using Configuration;

    public class SlackConfigurationFactory : ISlackConfigurationFactory
    {
        public const string ChannelsSetting = "channels";

        public SlackConfiguration GetConfiguration(BotElement slackBot)
        {
            var channels = slackBot.GetSetting(ChannelsSetting)
                                .Split(',')
                                .Select(channel => channel.Trim());
            return new SlackConfiguration
            {
                Channels = channels,
                AllNotificationsShouldGoToAllChannels = channels.Any(),
                Bot = slackBot
            };
        }
    }
}