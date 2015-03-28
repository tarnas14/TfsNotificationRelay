namespace DevCore.TfsNotificationRelay.Slack
{
    using Configuration;

    public class SlackConfiguration
    {
        private readonly BotElement _slackBot;

        public SlackConfiguration(BotElement slackBot)
        {
            _slackBot = slackBot;
        }

        public const string ChannelsSetting = "channels";

        public bool AllNotificationsShouldGoToAllChannels
        {
            get { return !string.IsNullOrEmpty(_slackBot.GetSetting("channels")); }
        }
    }
}