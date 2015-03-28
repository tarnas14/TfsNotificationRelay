namespace DevCore.TfsNotificationRelay.Slack
{
    using Configuration;
    using Notifications;

    public interface ISlackMessageFactory
    {
        Message GetMessage(INotification notification, BotElement botElement, string channel);
    }
}