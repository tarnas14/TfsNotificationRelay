namespace DevCore.TfsNotificationRelay.Slack
{
    using Notifications;
    using System.Collections.Generic;

    public interface ISlackMessageFactory
    {
        Message GetMessage(INotification notification, SlackConfiguration slackConfiguration, string channel);
        IEnumerable<Message> GetMessages(INotification notification, SlackConfiguration slackConfiguration);
    }
}