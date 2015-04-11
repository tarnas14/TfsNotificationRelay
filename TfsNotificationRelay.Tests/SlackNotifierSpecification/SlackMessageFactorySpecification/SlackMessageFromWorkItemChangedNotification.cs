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
    class SlackMessageFromWorkItemChangedNotification : SlackMessageElementsForEveryNotification<IWorkItemChangedNotification>
    {
        protected override void ReturnFromNotification(params string[] notificationLines)
        {
            A.CallTo(() => Notification.ToMessage(A<BotElement>.Ignored, A<Func<string, string>>.Ignored))
                .Returns(new List<string>(notificationLines));
        }

        [Test]
        public void ShouldNotNotifyAnyChannelsAboutAssignmentChange()
        {
            //given
            A.CallTo(() => Notification.IsAssignmentChanged).Returns(true);

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            Assert.That(messages, Is.Empty);
        }

        [Test]
        public void ShouldNotNotifyAnyChannelsAboutRandomStateChanges()
        {
            //given
            SlackConfiguration.TestingState = "testingState";
            SlackConfiguration.InProgressState = "inProgress";

            A.CallTo(() => Notification.IsStateChanged).Returns(true);
            A.CallTo(() => Notification.State).Returns("some state not specified in config");
            A.CallTo(() => Notification.OldState).Returns("some other state not in config");

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            Assert.That(messages, Is.Empty);
        }

        [Test]
        public void ShouldNotifyNormalChannelsWhenTaskComesBackFromTestingToInProgress()
        {
            //given
            SlackConfiguration.TestingState = "testingState";
            SlackConfiguration.InProgressState = "inProgress";

            A.CallTo(() => Notification.IsStateChanged).Returns(true);
            A.CallTo(() => Notification.State).Returns(SlackConfiguration.InProgressState);
            A.CallTo(() => Notification.OldState).Returns(SlackConfiguration.TestingState);

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var actualChannels = messages.Select(message => message.Channel);

            Assert.That(actualChannels, Is.EquivalentTo(SlackConfiguration.Channels));
        }
    }
}
