namespace DevCore.TfsNotificationRelay.Configuration
{
    public interface INotificationTextFormatting
    {
        string Id { get; }
        string PushFormat { get; }
        string Pushed { get; }
        string ForcePushed { get; }
        string Commit { get; }
        string RefPointer { get; }
        string Deleted { get; }
        string CommitFormat { get; }
        string LinesSupressedFormat { get; }
        string DateTimeFormat { get; }
        string TimeSpanFormat { get; }
        string BuildFormat { get; }
        string BuildQualityChangedFormat { get; }
        string BuildQualityNotSet { get; }
        string ProjectCreatedFormat { get; }
        string ProjectDeletedFormat { get; }
        string CheckinFormat { get; }
        string ProjectLinkFormat { get; }
        string ChangeCountAddFormat { get; }
        string ChangeCountDeleteFormat { get; }
        string ChangeCountEditFormat { get; }
        string ChangeCountRenameFormat { get; }
        string ChangeCountSourceRenameFormat { get; }
        string ChangeCountUnknownFormat { get; }
        string WorkItemchangedFormat { get; }
        string Updated { get; }
        string Created { get; }
        string State { get; }
        string AssignedTo { get; }
        string PullRequestCreatedFormat { get; }
        string PullRequestStatusUpdateFormat { get; }
        string PullRequestReviewerVoteFormat { get; }
        string VoteApproved { get; }
        string VoteRejected { get; }
        string VoteRescinded { get; }
        string Completed { get; }
        string Abandoned { get; }
        string Reactivated { get; }
    }
}