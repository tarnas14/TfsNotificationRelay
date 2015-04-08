namespace TfsNotificationRelay.Tests.SlackNotifierSpecification.SlackMessageFactorySpecification
{
    using System.Linq;
    using DevCore.TfsNotificationRelay.Notifications;
    using NUnit.Framework;

    [TestFixture]
    class SlackMessageFromGenericNotification : SlackMessageElementsForEveryNotification<INotification>
    {
        [Test]
        public void ShouldUseStandardColorForGenericNotifications()
        {
            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);

            //then
            var attachments = messages.SelectMany(message => message.Attachments);
            Assert.That(attachments.All(attachment => attachment.Color == SlackConfiguration.StandardColor));
        }

        [Test]
        public void ShouldAddNotificationLinesAsShortAttachmentFields()
        {
            //given
            ReturnFromNotification("some", "notification", "lines");
            var expectedFieldValues = new[] {"notification", "lines"};

            //when
            var messages = SlackMessageFactory.GetMessages(Notification, SlackConfiguration);
            
            //then
            foreach(var message in messages)
            {
                var attachmentFields = message.Attachments.First().Fields;
                var actualFieldValues = attachmentFields.Select(field => field.Value);

                Assert.That(actualFieldValues, Is.EquivalentTo(expectedFieldValues));
                Assert.That(!attachmentFields.Any(field => field.IsShort));
            }
        }
    }
}