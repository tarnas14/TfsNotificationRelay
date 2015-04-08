namespace TfsNotificationRelay.Tests.SlackNotifierSpecification.SlackMessageFactorySpecification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DevCore.TfsNotificationRelay.Configuration;
    using DevCore.TfsNotificationRelay.Notifications;
    using DevCore.TfsNotificationRelay.Slack;
    using FakeItEasy;
    using NUnit.Framework;

    abstract class SlackMessageElementsForEveryNotification<T> where T : INotification
    {
        protected SlackConfiguration SlackConfiguration;
        protected T Notification;
        protected SlackMessageFactory SlackMessageFactory;

        [SetUp]
        public void Setup()
        {
            SlackMessageFactory = new SlackMessageFactory();
            var slackBot =
                TestConfigurationHelper.LoadSlackBot(@"SlackNotifierSpecification\slackNotifierTestConfig.config");
            SlackConfiguration = new SlackConfigurationFactory().GetConfiguration(slackBot);

            Notification = A.Fake<T>();
            ReturnFromNotification("some", "lines");
        }

        protected virtual void ReturnFromNotification(params string[] notificationLines)
        {
            A.CallTo(() => Notification.ToMessage(A<BotElement>.Ignored, A<Func<string, string>>.Ignored))
                .Returns(new List<string>(notificationLines));
        }

        [Test]
        public void ShouldAddUsernameToEveryMessage()
        {
            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            Assert.That(messages.All(message => message.Username == SlackConfiguration.Username));
        }

        [Test]
        public void ShouldTreatFirstLineOfTheNotificationAsFallbackAndPretextForAttachments()
        {
            //given
            ReturnFromNotification("fallback", "other", "lines");
            const string expectedFallback = "fallback";

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var actualFallbacks = messages.Select(message => message.Attachments.First().Fallback);
            var actualPretexts = messages.Select(message => message.Attachments.First().Pretext);

            Assert.That(actualFallbacks.All(fallback => fallback == expectedFallback));
            Assert.That(actualPretexts.All(pretext => pretext == expectedFallback));
        }
    }
}
