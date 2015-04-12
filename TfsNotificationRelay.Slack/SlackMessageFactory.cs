namespace DevCore.TfsNotificationRelay.Slack
{
    using System.Linq;
    using Notifications;
    using System.Collections.Generic;

    public class SlackMessageFactory : ISlackMessageFactory
    {
        public IEnumerable<Message> GetMessages(INotification notification, SlackConfiguration slackConfiguration)
        {
            return ToSlackMessages((dynamic)notification, slackConfiguration);
        }

        private IEnumerable<Message> ToSlackMessages(INotification notification, SlackConfiguration slackConfiguration)
        {
            return
                slackConfiguration.Channels.Select(
                    channel => ToSlackMessage(notification, slackConfiguration, channel));
        }

        private Message ToSlackMessage(INotification notification, SlackConfiguration slackConfiguration, string channel)
        {
            var lines = notification.ToMessage(slackConfiguration.Bot, s => s);

            return SlackHelper.CreateSlackMessage(lines, slackConfiguration.Bot, channel, slackConfiguration.StandardColor);
        }

        private IEnumerable<Message> ToSlackMessages(IBuildCompletionNotification notification,
            SlackConfiguration slackConfiguration)
        {
            var messages = new List<Message>(slackConfiguration.Channels.Select(channel => ToSlackMessage(notification, slackConfiguration, channel)));

            if (!notification.IsSuccessful)
            {
                messages.AddRange(
                    slackConfiguration.NotableEventsChannels.Select(
                        channel => ToSlackMessage(notification, slackConfiguration, channel)));
            }

            return messages;
        }

        private Message ToSlackMessage(IBuildCompletionNotification notification, SlackConfiguration slackConfiguration, string channel)
        {
            var lines = notification.ToMessage(slackConfiguration.Bot, s => s);
            var color = notification.IsSuccessful ? slackConfiguration.SuccessColor : slackConfiguration.ErrorColor;

            return SlackHelper.CreateSlackMessage(lines, slackConfiguration.Bot, channel, color);
        }

        private IEnumerable<Message> ToSlackMessages(IWorkItemChangedNotification notification, SlackConfiguration slackConfiguration)
        {
            if (notification.IsAssignmentChanged || !NeedToNotifyChannelsAboutWorkItemStateChange(notification, slackConfiguration))
            {
                return new Message[] { };
            }

            return
                slackConfiguration.Channels.Select(
                    channel => ToSlackMessage(notification, slackConfiguration, channel));
        }

        private bool NeedToNotifyChannelsAboutWorkItemStateChange(IWorkItemChangedNotification notification, SlackConfiguration slackConfiguration)
        {
            return 
                WorkItemIsReturnedFromTesting(notification, slackConfiguration) || 
                WorkItemIsReadyForTesting(notification, slackConfiguration) ||
                WorkItemIsANewBugThatNeedsToBeNotifiedAbout(notification, slackConfiguration);
        }

        private bool WorkItemIsReadyForTesting(IWorkItemChangedNotification notification, SlackConfiguration slackConfiguration)
        {
            return notification.State == slackConfiguration.ReadyToTest;
        }

        private bool WorkItemIsReturnedFromTesting(IWorkItemChangedNotification notification, SlackConfiguration slackConfiguration)
        {
            return
                notification.State == slackConfiguration.InProgressState &&
                notification.OldState == slackConfiguration.TestingState;
        }

        private bool WorkItemIsANewBugThatNeedsToBeNotifiedAbout(IWorkItemChangedNotification notification, SlackConfiguration slackConfiguration)
        {
            return 
                notification.IsNew && 
                notification.WiType == slackConfiguration.BugWorkItemType &&
                slackConfiguration.SeveritiesToBeNotifiedAbout.Contains(notification.Severity);
        }

        private Message ToSlackMessage(IWorkItemChangedNotification notification, SlackConfiguration slackConfiguration, string channel)
        {
            var colour = WorkItemIsReadyForTesting(notification, slackConfiguration) ? slackConfiguration.SuccessColor : slackConfiguration.ErrorColor; 
            
            string header = notification.ToMessage(slackConfiguration.Bot, s => s).First();
            var fields = new[] { 
                new AttachmentField(slackConfiguration.Bot.Text.State, notification.State, true), 
                new AttachmentField(slackConfiguration.Bot.Text.AssignedTo, notification.AssignedTo, true) 
            };

            return SlackHelper.CreateSlackMessage(header, fields, slackConfiguration.Bot, channel, colour);
        }
    }
}