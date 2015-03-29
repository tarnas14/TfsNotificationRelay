namespace TfsNotificationRelay.Tests.SlackNotifierSpecification
{
    using System;
    using DevCore.TfsNotificationRelay.Configuration;
    using DevCore.TfsNotificationRelay.Notifications;
    using DevCore.TfsNotificationRelay.Slack;
    using FakeItEasy;
    using Microsoft.TeamFoundation.Framework.Server;
    using NUnit.Framework;

    [TestFixture]
    class MainNotificationsSpecification
    {
        private SlackNotifier _slackNotifier;
        private BotElement _legacyBotElement;
        private ISlackClient _slackClient;
        private ISlackMessageFactory _slackMessageFactory;
        private ISlackConfigurationFactory _slackConfigurationFactory;

        [SetUp]
        public void Setup()
        {
            _legacyBotElement = TestConfigurationHelper.LoadSlackBot(@"SlackNotifierSpecification\slackNotifierTestConfig.config");
            _slackClient = A.Fake<ISlackClient>();

            _slackMessageFactory = A.Fake<ISlackMessageFactory>();
            A.CallTo(
                () =>
                    _slackMessageFactory.GetMessage(A<INotification>.Ignored, A<SlackConfiguration>.Ignored, A<String>.Ignored))
                .ReturnsLazily((INotification notification, SlackConfiguration slackConfiguration, string channel) => new Message { Channel = channel });

            _slackConfigurationFactory = A.Fake<ISlackConfigurationFactory>();

            _slackNotifier = new SlackNotifier(_slackClient, _slackMessageFactory, _slackConfigurationFactory);
        }

        [Test]
        public async void ShouldNotifyAllChannelsAboutTheNotification()
        {
            //given
            var slackConfiguration = new SlackConfiguration
            {
                Channels = new[]
                {
                    "#general",
                    "#b"
                }
            };
            A.CallTo(() => _slackConfigurationFactory.GetConfiguration(A<BotElement>.Ignored))
                .Returns(slackConfiguration);

            //when;
            await _slackNotifier.NotifyAsync(A.Fake<TeamFoundationRequestContext>(), A.Fake<INotification>(), _legacyBotElement);

            //then
            A.CallTo(() => _slackClient.SendMessageAsync(A<Message>.That.Matches(msg => msg.Channel == "#general"), A<string>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _slackClient.SendMessageAsync(A<Message>.That.Matches(msg => msg.Channel == "#b"), A<string>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
