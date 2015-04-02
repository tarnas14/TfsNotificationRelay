namespace DevCore.TfsNotificationRelay.Notifications
{
    using System;
    using Microsoft.TeamFoundation.Build.Server;

    public interface IBuildCompletionNotification : INotification
    {
        string ProjectName { get; set; }
        string BuildDefinition { get; set; }
        string DropLocation { get; set; }
        BuildStatus BuildStatus { get; set; }
        string BuildUrl { get; set; }
        string BuildNumber { get; set; }
        BuildReason BuildReason { get; set; }
        string RequestedFor { get; set; }
        string RequestedForDisplayName { get; set; }
        DateTime StartTime { get; set; }
        DateTime FinishTime { get; set; }
        string UserName { get; }
        string DisplayName { get; }
        bool IsSuccessful { get; }
        string TeamProjectCollection { get; set; }
        bool IsMatch(string collection, Configuration.EventRuleCollection eventRules);
    }
}