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
            A.CallTo(() => Notification.IsStateChanged).Returns(true);
            A.CallTo(() => Notification.State).Returns(SlackConfiguration.InProgressState);
            A.CallTo(() => Notification.OldState).Returns(SlackConfiguration.TestingState);

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var actualChannels = messages.Select(message => message.Channel);

            Assert.That(actualChannels, Is.EquivalentTo(SlackConfiguration.Channels));
        }

        [Test]
        public void ShouldSendRedColouredNotificationAboutTasksComingBrackFromTesting()
        {
            //given
            A.CallTo(() => Notification.IsStateChanged).Returns(true);
            A.CallTo(() => Notification.State).Returns(SlackConfiguration.InProgressState);
            A.CallTo(() => Notification.OldState).Returns(SlackConfiguration.TestingState);

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var colours = messages.Select(message => message.Attachments.First().Color);

            Assert.That(colours.All(colour => colour == SlackConfiguration.ErrorColor));
        }

        [Test]
        public void ShouldNotifyNormalChannelsWhenTaskIsReadyToTest()
        {
            //given
            A.CallTo(() => Notification.IsStateChanged).Returns(true);
            A.CallTo(() => Notification.State).Returns(SlackConfiguration.ReadyToTest);

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var actualChannels = messages.Select(message => message.Channel);

            Assert.That(actualChannels, Is.EquivalentTo(SlackConfiguration.Channels));
        }

        [Test]
        public void ShouldSendGreenColourNotificationAboutTasksReadyToTest()
        {
            //given
            A.CallTo(() => Notification.IsStateChanged).Returns(true);
            A.CallTo(() => Notification.State).Returns(SlackConfiguration.ReadyToTest);

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var colours = messages.Select(message => message.Attachments.First().Color);

            Assert.That(colours.All(colour => colour == SlackConfiguration.SuccessColor));
        }

        [Test]
        public void ShouldNotNotifyAnyChannelsAboutNewBugsOfSeveritiesOutsideConfiguration()
        {
            //given
            SlackConfiguration.SeveritiesToBeNotifiedAbout = new string[] {};

            A.CallTo(() => Notification.IsNew).Returns(true);
            A.CallTo(() => Notification.WiType).Returns(SlackConfiguration.BugWorkItemType);
            A.CallTo(() => Notification.Severity).Returns("some severity");

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            Assert.That(messages, Is.Empty);
        }

        [Test]
        public void ShouldSendNotificationsToNormalChannelWhenNewBugOfSpecifiedSeverityIsCreated()
        {
            //given
            SlackConfiguration.SeveritiesToBeNotifiedAbout = new[] {"testSeverity"};

            A.CallTo(() => Notification.IsNew).Returns(true);
            A.CallTo(() => Notification.WiType).Returns(SlackConfiguration.BugWorkItemType);
            A.CallTo(() => Notification.Severity).Returns(SlackConfiguration.SeveritiesToBeNotifiedAbout.First());

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var actualChannels = messages.Select(message => message.Channel);

            Assert.That(actualChannels, Is.EquivalentTo(SlackConfiguration.Channels));
        }

        [Test]
        public void ShouldSendRedColouredNotificationAboutNewBugsOfConfiguredSeverity()
        {
            //given
            SlackConfiguration.SeveritiesToBeNotifiedAbout = new[] { "testSeverity" };

            A.CallTo(() => Notification.IsNew).Returns(true);
            A.CallTo(() => Notification.WiType).Returns(SlackConfiguration.BugWorkItemType);
            A.CallTo(() => Notification.Severity).Returns(SlackConfiguration.SeveritiesToBeNotifiedAbout.First());

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var colours = messages.Select(message => message.Attachments.First().Color);

            Assert.That(colours.All(colour => colour == SlackConfiguration.ErrorColor));
        }
    }
}
