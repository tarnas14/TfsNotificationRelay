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
        public void ShouldFillMessageWithSpecifiedChannel()
        {
            //given
            const string expectedChannel = "expectedChannel";

            var notification = A.Fake<INotification>();
            A.CallTo(() => notification.ToMessage(A<BotElement>.Ignored, A<Func<string, string>>.Ignored))
                .Returns(new List<string> {"some", "lines"});

            //when
            var message = new SlackMessageFactory().GetMessage(notification, _slackConfiguration, expectedChannel);

            //then
            Assert.That(message.Channel, Is.EqualTo(expectedChannel));
        }
    }
}