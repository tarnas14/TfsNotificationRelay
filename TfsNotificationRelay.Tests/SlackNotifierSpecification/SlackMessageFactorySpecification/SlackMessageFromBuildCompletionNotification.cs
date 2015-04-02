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
            A.CallTo(() => Notification.ToMessage(A<INotificationTextFormatting>.Ignored, A<Func<string, string>>.Ignored))
                .Returns(new List<string>(notificationLines));
        }

        [Test]
        public void ShouldUserErrorColorForNotificationAboutUnsuccessfullBuild()
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
        public void ShouldUserSuccessColorForNotificationAboutSuccessfullBuild()
        {
            //given
            A.CallTo(() => Notification.IsSuccessful).Returns(true);

            //when
            var message = SlackMessageFactory.GetMessage(Notification, SlackConfiguration, "channel");

            //then
            var firstAttachment = message.Attachments.Single();

            Assert.That(firstAttachment.Color, Is.EqualTo(SlackConfiguration.SuccessColor));
        }
    }
}
