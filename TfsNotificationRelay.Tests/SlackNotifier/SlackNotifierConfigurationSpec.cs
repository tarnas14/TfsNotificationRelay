namespace TfsNotificationRelay.Tests.SlackNotifier
{
    using System.Configuration;
    using System.Linq;
    using DevCore.TfsNotificationRelay.Configuration;
    using DevCore.TfsNotificationRelay.Slack;
    using NUnit.Framework;

    [TestFixture]
    class SlackNotifierConfigurationSpec
    {
        private SlackConfiguration GetSlackBotFromConfigFile(string configFile)
        {
            var fileMap = new ExeConfigurationFileMap {ExeConfigFilename = configFile};
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            var section = config.GetSection("tfsNotificationRelay") as TfsNotificationRelaySection;

            var slackBot = section.Bots.First(bot => bot.Id == "slack");
            return new SlackConfiguration(slackBot);
        }

        [Test]
        public void ShouldRecogniseLegacyNotificationSchemeWhenChannelsSettingIsNotEmpty()
        {
            //when
            var config = GetSlackBotFromConfigFile(@"SlackNotifier\legacyNotification.config");

            //then
            Assert.That(config.AllNotificationsShouldGoToAllChannels, Is.True);
        }

        [Test]
        public void ShouldRetrieveChannelListFromConfiguration()
        {
            //given
            var expectedChannels = new[]
            {
                "#general",
                "#b"
            };
            var config = GetSlackBotFromConfigFile(@"SlackNotifier\legacyNotification.config");

            //when
            var actualChannels = config.Channels;

            //then
            Assert.That(actualChannels, Is.EquivalentTo(expectedChannels));
        }
    }
}
