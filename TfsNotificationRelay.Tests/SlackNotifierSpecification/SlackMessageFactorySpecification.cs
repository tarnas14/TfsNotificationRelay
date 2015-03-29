namespace TfsNotificationRelay.Tests.SlackNotifierSpecification
{
    using System;
    using System.Collections.Generic;
    using DevCore.TfsNotificationRelay.Configuration;
    using DevCore.TfsNotificationRelay.Notifications;
    using DevCore.TfsNotificationRelay.Slack;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class SlackMessageFactorySpecification
    {
        private SlackConfiguration _slackConfiguration;

        [SetUp]
        public void Setup()
        {
            var slackBot =
                TestConfigurationHelper.LoadSlackBot(@"SlackNotifierSpecification\slackNotifierTestConfig.config");
            _slackConfiguration = new SlackConfigurationFactory().GetConfiguration(slackBot);
        }

        [Test]
        public void ShouldFillMessageFromGenericNotificationWithSpecifiedChannel()
        {
            //given
            const string expectedChannel = "expectedChannel";

            var genericNotification = NotificationBuilder.GetGenericNotification("some", "lines");

            //when
            var message = new SlackMessageFactory().GetMessage(genericNotification, _slackConfiguration, expectedChannel);

            //then
            Assert.That(message.Channel, Is.EqualTo(expectedChannel));
        }

        [Test]
        public void ShouldFillMessageFromBuildCompletionNotificationWithSpecifiedChannel()
        {
            //given
            const string expectedChannel = "expectedChannel";

            var buildCompletionNotification =
                NotificationBuilder.GetBuildCompletedNotification("some", "lines");

            //when
            var message = new SlackMessageFactory().GetMessage(buildCompletionNotification, _slackConfiguration, expectedChannel);

            //then
            Assert.That(message.Channel, Is.EqualTo(expectedChannel));
        }

        [Test]
        public void ShouldFillMessageFromWorkItemChangedNotificationWithSpecifiedChannel()
        {
            //given
            const string expectedChannel = "expectedChannel";

            var workItemChangedNotification = NotificationBuilder.GetWorkItemChangeNotification("some", "lines");

            //when
            var message = new SlackMessageFactory().GetMessage(workItemChangedNotification, _slackConfiguration, expectedChannel);

            //then
            Assert.That(message.Channel, Is.EqualTo(expectedChannel));
        }
    }

    static class NotificationBuilder
    {
        public static IBuildCompletionNotification GetBuildCompletedNotification(params string[] lines)
        {
            var notification = A.Fake<IBuildCompletionNotification>();
            var messageLines = new List<string>(lines);

            A.CallTo(() => notification.ToMessage(A<BotElement>.Ignored, A<Func<string, string>>.Ignored))
                .Returns(messageLines);

            return notification;
        }

        public static INotification GetGenericNotification(params string[] lines)
        {
            var notification = A.Fake<INotification>();
            var messageLines = new List<string>(lines);

            A.CallTo(() => notification.ToMessage(A<BotElement>.Ignored, A<Func<string, string>>.Ignored))
                .Returns(messageLines);

            return notification;
        }

        public static IWorkItemChangedNotification GetWorkItemChangeNotification(params string[] lines)
        {
            var notification = A.Fake<IWorkItemChangedNotification>();
            var messageLines = new List<string>(lines);

            A.CallTo(() => notification.ToMessage(A<BotElement>.Ignored, A<Func<string, string>>.Ignored))
                .Returns(messageLines);

            return notification;
        }
    }
}