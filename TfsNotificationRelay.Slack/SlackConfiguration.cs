namespace DevCore.TfsNotificationRelay.Slack
{
    using System.Collections.Generic;
    using Configuration;

    public class SlackConfiguration
    {
        public IEnumerable<string> Channels { get; set; }
        public BotElement Bot { get; set; }
        public string WebhookUrl { get; set; }

        public string Username { get; set; }
        public string IconUrl { get; set; }
        public string StandardColor { get; set; }
        public string SuccessColor { get; set; }
        public string ErrorColor { get; set; }
        public string IconEmoji { get; set; }

        public Formatting TextFormatting { get; set; }

        public class Formatting
        {
            public string Id { get; set; }
            public string PushFormat { get; set; }
            public string Pushed { get; set; }
            public string ForcePushed { get; set; }
            public string Commit { get; set; }
            public string RefPointer { get; set; }
            public string Deleted { get; set; }
            public string CommitFormat { get; set; }
            public string LinesSupressedFormat { get; set; }
            public string DateTimeFormat { get; set; }
            public string TimeSpanFormat { get; set; }
            public string BuildFormat { get; set; }
            public string BuildQualityChangedFormat { get; set; }
            public string BuildQualityNotSet { get; set; }
            public string ProjectCreatedFormat { get; set; }
            public string ProjectDeletedFormat { get; set; }
            public string CheckinFormat { get; set; }
            public string ProjectLinkFormat { get; set; }
            public string ChangeCountAddFormat { get; set; }
            public string ChangeCountDeleteFormat { get; set; }
            public string ChangeCountEditFormat { get; set; }
            public string ChangeCountRenameFormat { get; set; }
            public string ChangeCountSourceRenameFormat { get; set; }
            public string ChangeCountUnknownFormat { get; set; }
            public string WorkItemchangedFormat { get; set; }
            public string Updated { get; set; }
            public string Created { get; set; }
            public string State { get; set; }
            public string AssignedTo { get; set; }
            public string PullRequestCreatedFormat { get; set; }
            public string PullRequestStatusUpdateFormat { get; set; }
            public string VoteApproved { get; set; }
            public string VoteRejected { get; set; }
            public string VoteRescinded { get; set; }
            public string Completed { get; set; }
            public string Abandoned { get; set; }
            public string Reactivated { get; set; }
        }
    }
}