namespace TfsNotificationRelay.Tests.SlackNotifierSpecification
{
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
            var slackBot = TestConfigurationHelper.LoadSlackBot(@"SlackNotifierSpecification\legacyNotification.config");

            //when
            var config = new SlackConfiguration(slackBot);

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
            var slackBot = TestConfigurationHelper.LoadSlackBot(@"SlackNotifierSpecification\legacyNotification.config");
            var config = new SlackConfiguration(slackBot);

            //when
            var actualChannels = config.Channels;

            //then
            Assert.That(actualChannels, Is.EquivalentTo(expectedChannels));
        }

        [Test]
        public void ShouldStoreBotElementThatWasUsedToInitializeIt()
        {
            //given
            var slackBot = TestConfigurationHelper.LoadSlackBot(@"SlackNotifierSpecification\legacyNotification.config");

            //when
            var config = new SlackConfiguration(slackBot);

            //then
            Assert.That(config.Bot, Is.EqualTo(slackBot));
        }
    }
}
