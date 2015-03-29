namespace TfsNotificationRelay.Tests.SlackNotifierSpecification
{
    using DevCore.TfsNotificationRelay.Slack;
    using NUnit.Framework;

    [TestFixture]
    class SlackNotifierConfigurationFactorySpecification
    {
        private ISlackConfigurationFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new SlackConfigurationFactory();
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
            var slackBot = TestConfigurationHelper.LoadSlackBot(@"SlackNotifierSpecification\slackNotifierTestConfig.config");
            var config = _factory.GetConfiguration(slackBot);

            //when
            var actualChannels = config.Channels;

            //then
            Assert.That(actualChannels, Is.EquivalentTo(expectedChannels));
        }

        [Test]
        public void ShouldStoreBotElementThatWasUsedToInitializeIt()
        {
            //given
            var slackBot = TestConfigurationHelper.LoadSlackBot(@"SlackNotifierSpecification\slackNotifierTestConfig.config");

            //when
            var config = _factory.GetConfiguration(slackBot);

            //then
            Assert.That(config.Bot, Is.EqualTo(slackBot));
        }
    }
}
