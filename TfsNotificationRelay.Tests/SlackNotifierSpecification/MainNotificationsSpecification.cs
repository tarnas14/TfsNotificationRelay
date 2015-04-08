namespace TfsNotificationRelay.Tests.SlackNotifierSpecification
{
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

            _slackConfigurationFactory = A.Fake<ISlackConfigurationFactory>();

            _slackNotifier = new SlackNotifier(_slackClient, _slackMessageFactory, _slackConfigurationFactory);
        }

        [Test]
        public async void ShouldSendAllMessagesGotFromMessageFactory()
        {
            //given
            var messages = new List<Message>
            {
                new Message(),
                new Message(),
                new Message()
            };
            A.CallTo(() => _slackMessageFactory.GetMessages(A<INotification>.Ignored, A<SlackConfiguration>.Ignored))
                .Returns(messages);

            //when
            await _slackNotifier.NotifyAsync(A.Fake<TeamFoundationRequestContext>(), A.Fake<INotification>(), A.Fake<BotElement>());

            //then
            foreach (var message in messages)
            {
                A.CallTo(() => _slackClient.SendMessageAsync(message, A<string>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
            }
        }

        [Test]
        public async void ShouldSendAllMessagesToTheWebhookSpecifiedInConfiguration()
        {
            //given
            var messages = new List<Message>
            {
                new Message(),
                new Message(),
                new Message()
            };
            A.CallTo(() => _slackMessageFactory.GetMessages(A<INotification>.Ignored, A<SlackConfiguration>.Ignored))
                .Returns(messages);

            const string webhookUrl = "webhook";
            A.CallTo(() => _slackConfigurationFactory.GetConfiguration(A<BotElement>.Ignored))
                .Returns(new SlackConfiguration {WebhookUrl = webhookUrl});

            //when
            await _slackNotifier.NotifyAsync(A.Fake<TeamFoundationRequestContext>(), A.Fake<INotification>(), A.Fake<BotElement>());

            //then
            A.CallTo(() => _slackClient.SendMessageAsync(A<Message>.Ignored, webhookUrl)).MustHaveHappened(Repeated.Exactly.Times(messages.Count));
        }
    }
}
