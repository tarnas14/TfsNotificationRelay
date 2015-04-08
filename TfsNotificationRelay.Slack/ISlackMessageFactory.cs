namespace DevCore.TfsNotificationRelay.Slack
{
    using Notifications;
    using System.Collections.Generic;

    public interface ISlackMessageFactory
    {
        IEnumerable<Message> GetMessages(INotification notification, SlackConfiguration slackConfiguration);
    }
}