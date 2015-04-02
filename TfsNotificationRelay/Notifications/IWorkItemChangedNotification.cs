namespace DevCore.TfsNotificationRelay.Notifications
{
    public interface IWorkItemChangedNotification : INotification
    {
        bool IsNew { get; set; }
        string UniqueName { get; set; }
        string DisplayName { get; set; }
        string WiUrl { get; set; }
        string WiType { get; set; }
        int WiId { get; set; }
        string WiTitle { get; set; }
        string ProjectName { get; set; }
        bool IsStateChanged { get; set; }
        bool IsAssignmentChanged { get; set; }
        string AssignedTo { get; set; }
        string State { get; set; }
        string UserName { get; }
        string TeamProjectCollection { get; set; }
        bool IsMatch(string collection, Configuration.EventRuleCollection eventRules);
    }
}