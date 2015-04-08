namespace TfsNotificationRelay.Tests.SlackNotifierSpecification.SlackMessageFactorySpecification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DevCore.TfsNotificationRelay.Configuration;
    using DevCore.TfsNotificationRelay.Notifications;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    class SlackMessageFromBuildCompletionNotification : SlackMessageElementsForEveryNotification<IBuildCompletionNotification>
    {
        protected override void ReturnFromNotification(params string[] notificationLines)
        {
            A.CallTo(() => Notification.ToMessage(A<BotElement>.Ignored, A<Func<string, string>>.Ignored))
                .Returns(new List<string>(notificationLines));
        }

        [Test]
        public void ShouldUseErrorColorForNotificationAboutUnsuccessfullBuild()
        {
            //given
            A.CallTo(() => Notification.IsSuccessful).Returns(false);

            //when
            var message = SlackMessageFactory.GetMessage(Notification, SlackConfiguration, "channel");

            //then
            var firstAttachment = message.Attachments.Single();

            Assert.That(firstAttachment.Color, Is.EqualTo(SlackConfiguration.ErrorColor));
        }

        [Test]
        public void ShouldUseSuccessColorForNotificationAboutSuccessfullBuild()
        {
            //given
            A.CallTo(() => Notification.IsSuccessful).Returns(true);

            //when
            var message = SlackMessageFactory.GetMessage(Notification, SlackConfiguration, "channel");

            //then
            var firstAttachment = message.Attachments.Single();

            Assert.That(firstAttachment.Color, Is.EqualTo(SlackConfiguration.SuccessColor));
        }

        [Test]
        public void ShouldGenerateMessagesForBothNormalChannelsAndNotableEventChannelsOnUnsuccessfullBuild()
        {
            //given
            A.CallTo(() => Notification.IsSuccessful).Returns(false);
            var expectedChannels = SlackConfiguration.Channels.Concat(SlackConfiguration.NotableEventsChannels);

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var actualChannels = messages.Select(message => message.Channel);
            Assert.That(actualChannels, Is.EquivalentTo(expectedChannels));
        }

        [Test]
        public void ShouldGenerateMessagesOnlyForNormalChannelsOnSuccessfullBuild()
        {
            //given
            A.CallTo(() => Notification.IsSuccessful).Returns(true);

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var actualChannels = messages.Select(message => message.Channel);
            Assert.That(actualChannels, Is.EquivalentTo(SlackConfiguration.Channels));
        }
    }
}
