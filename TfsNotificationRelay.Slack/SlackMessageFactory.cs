namespace DevCore.TfsNotificationRelay.Slack
{
    using System.Linq;
    using Configuration;
    using Notifications;
    using System.Collections.Generic;

    public class SlackMessageFactory : ISlackMessageFactory
    {
        private Message ToSlackMessage(INotification notification, BotElement bot, string channel)
        {
            var lines = notification.ToMessage(bot, s => s);

            return SlackHelper.CreateSlackMessage(lines, bot, channel, bot.GetSetting("standardColor"));
        }

        private Message ToSlackMessage(IBuildCompletionNotification notification, BotElement bot, string channel)
        {
            var lines = notification.ToMessage(bot, s => s);
            var color = notification.IsSuccessful ? bot.GetSetting("successColor") : bot.GetSetting("errorColor");

            return SlackHelper.CreateSlackMessage(lines, bot, channel, color);
        }

        private Message ToSlackMessage(IWorkItemChangedNotification notification, BotElement bot, string channel)
        {
            string header = notification.ToMessage(bot, s => s).First();
            var fields = new[] { 
                new AttachmentField(bot.Text.State, notification.State, true), 
                new AttachmentField(bot.Text.AssignedTo, notification.AssignedTo, true) 
            };

            return SlackHelper.CreateSlackMessage(header, fields, bot, channel, bot.GetSetting("standardColor"));
        }

        public IEnumerable<Message> GetMessages(INotification notification, SlackConfiguration slackConfiguration)
        {
            return ToSlackMessages((dynamic)notification, slackConfiguration);
        }

        private IEnumerable<Message> ToSlackMessages(IWorkItemChangedNotification notification, SlackConfiguration slackConfiguration)
        {
            if (notification.IsAssignmentChanged || !WorkItemIsReturnedFromTesting(notification, slackConfiguration))
            {
                return new Message[] { };
            }

            return
                slackConfiguration.Channels.Select(
                    channel => ToSlackMessage(notification, slackConfiguration.Bot, channel));
        }

        private bool WorkItemIsReturnedFromTesting(IWorkItemChangedNotification notification, SlackConfiguration slackConfiguration)
        {
            return
                notification.State == slackConfiguration.InProgressState &&
                notification.OldState == slackConfiguration.TestingState;
        }

        private IEnumerable<Message> ToSlackMessages(INotification notification, SlackConfiguration slackConfiguration)
        {
            return
                slackConfiguration.Channels.Select(
                    channel => ToSlackMessage(notification, slackConfiguration.Bot, channel));
        }

        private IEnumerable<Message> ToSlackMessages(IBuildCompletionNotification notification,
            SlackConfiguration slackConfiguration)
        {
            var messages = new List<Message>(slackConfiguration.Channels.Select(channel => ToSlackMessage(notification, slackConfiguration.Bot, channel)));

            if (!notification.IsSuccessful)
            {
                messages.AddRange(
                    slackConfiguration.NotableEventsChannels.Select(
                        channel => ToSlackMessage(notification, slackConfiguration.Bot, channel)));
            }

            return messages;
        }
    }
}