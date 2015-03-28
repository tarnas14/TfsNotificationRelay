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
        [Test]
        public void ShouldRecogniseLegacyNotificationSchemeWhenChannelsSettingIsNotEmpty()
        {
            //given
            var configuration = ConfigurationManager.GetSection("tfsNotificationRelay") as TfsNotificationRelaySection;
            var slackBot = configuration.Bots.First(bot => bot.Id == "slack");

            //when
            var config = new SlackConfiguration(slackBot);

            //then
            Assert.That(config.AllNotificationsShouldGoToAllChannels, Is.True);
        }
    }
}
