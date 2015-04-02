namespace DevCore.TfsNotificationRelay.Slack
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;

    public class SlackConfigurationFactory : ISlackConfigurationFactory
    {
        public SlackConfiguration GetConfiguration(BotElement slackBot)
        {
            var webhookUrl = slackBot.GetSetting("webhookUrl");
            var channels = GetChannels(slackBot.GetSetting("channels"));
            var username = slackBot.GetSetting("username");
            var iconEmoji = slackBot.GetSetting("iconEmoji");
            var iconUrl = slackBot.GetSetting("iconUrl");
            var standardColor = slackBot.GetSetting("standardColor");
            var successColor = slackBot.GetSetting("successColor");
            var errorColor = slackBot.GetSetting("errorColor");

            return new SlackConfiguration
            {
                WebhookUrl = webhookUrl,
                Channels = channels,
                Username = username,
                IconEmoji = iconEmoji,
                IconUrl = iconUrl,
                StandardColor = standardColor,
                SuccessColor = successColor,
                ErrorColor = errorColor,
                NotificationTextFormatting = new NotificationTextFormatting
                {
                    PushFormat = slackBot.Text.PushFormat,
                    Pushed = slackBot.Text.Pushed,
                    ForcePushed = slackBot.Text.ForcePushed,
                    Commit = slackBot.Text.Commit,
                    RefPointer = slackBot.Text.RefPointer,
                    Deleted = slackBot.Text.Deleted,
                    CommitFormat = slackBot.Text.CommitFormat,
                    LinesSupressedFormat = slackBot.Text.LinesSupressedFormat,
                    DateTimeFormat = slackBot.Text.DateTimeFormat,
                    TimeSpanFormat = slackBot.Text.TimeSpanFormat,
                    BuildFormat = slackBot.Text.BuildFormat,
                    BuildQualityChangedFormat = slackBot.Text.BuildQualityChangedFormat,
                    BuildQualityNotSet = slackBot.Text.BuildQualityNotSet,
                    ProjectCreatedFormat = slackBot.Text.ProjectCreatedFormat,
                    ProjectDeletedFormat = slackBot.Text.ProjectDeletedFormat,
                    CheckinFormat = slackBot.Text.CheckinFormat,
                    ProjectLinkFormat = slackBot.Text.ProjectLinkFormat,
                    ChangeCountAddFormat = slackBot.Text.ChangeCountAddFormat,
                    ChangeCountDeleteFormat = slackBot.Text.ChangeCountDeleteFormat,
                    ChangeCountEditFormat = slackBot.Text.ChangeCountEditFormat,
                    ChangeCountRenameFormat = slackBot.Text.ChangeCountRenameFormat,
                    ChangeCountSourceRenameFormat = slackBot.Text.ChangeCountSourceRenameFormat,
                    ChangeCountUnknownFormat = slackBot.Text.ChangeCountUnknownFormat,
                    WorkItemchangedFormat = slackBot.Text.WorkItemchangedFormat,
                    Updated = slackBot.Text.Updated,
                    Created = slackBot.Text.Created,
                    State = slackBot.Text.State,
                    AssignedTo = slackBot.Text.AssignedTo,
                    PullRequestCreatedFormat = slackBot.Text.PullRequestCreatedFormat,
                    PullRequestStatusUpdateFormat = slackBot.Text.PullRequestStatusUpdateFormat,
                    PullRequestReviewerVoteFormat = slackBot.Text.PullRequestReviewerVoteFormat,
                    VoteApproved = slackBot.Text.VoteApproved,
                    VoteRejected = slackBot.Text.VoteRejected,
                    VoteRescinded = slackBot.Text.VoteRescinded,
                    Completed = slackBot.Text.Completed,
                    Abandoned = slackBot.Text.Abandoned,
                    Reactivated = slackBot.Text.Reactivated
                }
            };
        }

        private IEnumerable<string> GetChannels(string channelsSetting)
        {
            return channelsSetting.Split(',').Select(channel => channel.Trim());
        }
    }
}