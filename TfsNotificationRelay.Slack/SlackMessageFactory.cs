namespace DevCore.TfsNotificationRelay.Slack
{
    using System.Linq;
    using Notifications;

    public class SlackMessageFactory : ISlackMessageFactory
    {
        public Message GetMessage(INotification notification, SlackConfiguration slackConfiguration, string channel)
        {
            return ToSlackMessage((dynamic) notification, slackConfiguration, channel);
        }

        private Message ToSlackMessage(INotification notification, SlackConfiguration slackConfiguration, string channel)
        {
            var lines = notification.ToMessage(slackConfiguration.NotificationTextFormatting, s => s);

            return SlackHelper.CreateSlackMessage(lines, slackConfiguration, channel, slackConfiguration.StandardColor);
        }

        private Message ToSlackMessage(IBuildCompletionNotification notification, SlackConfiguration slackConfiguration, string channel)
        {
            var lines = notification.ToMessage(slackConfiguration.NotificationTextFormatting, s => s);
            var color = notification.IsSuccessful ? slackConfiguration.SuccessColor : slackConfiguration.ErrorColor;

            return SlackHelper.CreateSlackMessage(lines, slackConfiguration, channel, color);
        }

        private Message ToSlackMessage(IWorkItemChangedNotification notification, SlackConfiguration slackConfiguration, string channel)
        {
            string header = notification.ToMessage(slackConfiguration.NotificationTextFormatting, s => s).First();
            var fields = new[] { 
                new AttachmentField(slackConfiguration.NotificationTextFormatting.State, notification.State, true), 
                new AttachmentField(slackConfiguration.NotificationTextFormatting.AssignedTo, notification.AssignedTo, true) 
            };

            return SlackHelper.CreateSlackMessage(header, fields, slackConfiguration, channel, slackConfiguration.StandardColor);
        }
    }
}