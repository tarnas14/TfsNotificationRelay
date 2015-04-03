namespace TfsNotificationRelay.Tests.SlackNotifierSpecification
{
    using System;
    using System.Collections.Generic;
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
        private ISlackClient _slackClient;
        private ISlackMessageFactory _slackMessageFactory;
        private ISlackConfigurationFactory _slackConfigurationFactory;

        [SetUp]
        public void Setup()
        {
            _slackClient = A.Fake<ISlackClient>();

            _slackMessageFactory = A.Fake<ISlackMessageFactory>();
            A.CallTo(
                () =>
                    _slackMessageFactory.GetMessage(A<INotification>.Ignored, A<SlackConfiguration>.Ignored, A<String>.Ignored))
                .ReturnsLazily((INotification notification, SlackConfiguration slackConfig, string channel) => new Message { Channel = channel });

            _slackConfigurationFactory = A.Fake<ISlackConfigurationFactory>();

            _slackNotifier = new SlackNotifier(_slackClient, _slackMessageFactory, _slackConfigurationFactory);
        }

        [Test]
        public void ShouldNotifyAllChannelsAboutAGenericNotification()
        {
            ShouldNotifyAllChannelsAboutNotification(A.Fake<INotification>());
        }

        [Test]
        public void ShouldNotifyAllChannelsAboutBuildCompletionNotification()
        {
            ShouldNotifyAllChannelsAboutNotification(A.Fake<IBuildCompletionNotification>());
        }

        [Test]
        public async void ShouldNotifyNormalAndNotableEventsChannelsAboutFailedBuild()
        {
            //given
            var slackConfiguration = new SlackConfiguration
            {
                Channels =  new[]
                {
                    "#normal", "channels"
                },
                NotableEventsChannels = new[]
                {
                    "#notable", "events"
                }
            };
            A.CallTo(() => _slackConfigurationFactory.GetConfiguration(A<BotElement>.Ignored))
                .Returns(slackConfiguration);

            var failedBuildNotification = A.Fake<IBuildCompletionNotification>();
            A.CallTo(() => failedBuildNotification.IsSuccessful).Returns(false);

            //when
            await _slackNotifier.NotifyAsync(A.Fake<TeamFoundationRequestContext>(), failedBuildNotification, A.Fake<BotElement>());

            //then
            ShouldHaveNotifiedChannels(slackConfiguration.Channels);
            ShouldHaveNotifiedChannels(slackConfiguration.NotableEventsChannels);
        }

        [Test]
        public async void ShouldNotNotifyNotableEventsChannelsAboutSuccessfulBuild()
        {
            //given
            var slackConfiguration = new SlackConfiguration
            {
                NotableEventsChannels = new[]
                {
                    "#notable", "events"
                }
            };
            A.CallTo(() => _slackConfigurationFactory.GetConfiguration(A<BotElement>.Ignored))
                .Returns(slackConfiguration);

            var failedBuildNotification = A.Fake<IBuildCompletionNotification>();
            A.CallTo(() => failedBuildNotification.IsSuccessful).Returns(true);

            //when
            await _slackNotifier.NotifyAsync(A.Fake<TeamFoundationRequestContext>(), failedBuildNotification, A.Fake<BotElement>());

            //then
            ShouldNotHaveNotifiedChannels(slackConfiguration.NotableEventsChannels);
        }

        private async void ShouldNotifyAllChannelsAboutNotification(INotification notification)
        {
            //given
            var slackConfiguration = new SlackConfiguration
            {
                Channels = new[]
                {
                    "#general",
                    "b"
                }
            };
            A.CallTo(() => _slackConfigurationFactory.GetConfiguration(A<BotElement>.Ignored))
                .Returns(slackConfiguration);

            //when;
            await _slackNotifier.NotifyAsync(A.Fake<TeamFoundationRequestContext>(), notification, A.Fake<BotElement>());

            //then
            ShouldHaveNotifiedChannels(slackConfiguration.Channels);
        }

        private void ShouldHaveNotifiedChannels(IEnumerable<string> channels)
        {
            foreach (var channel in channels)
            {
                A.CallTo(() => _slackClient.SendMessageAsync(A<Message>.That.Matches(msg => msg.Channel == channel), A<string>.Ignored))
                    .MustHaveHappened(Repeated.Exactly.Once);
            }
        }

        private void ShouldNotHaveNotifiedChannels(IEnumerable<string> channels)
        {
            foreach (var channel in channels)
            {
                A.CallTo(() => _slackClient.SendMessageAsync(A<Message>.That.Matches(msg => msg.Channel == channel), A<string>.Ignored))
                    .MustNotHaveHappened();
            }
        }
    }
}
