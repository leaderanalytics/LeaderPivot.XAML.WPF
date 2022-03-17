using System.ComponentModel;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

public enum DimensionAction
{
    [Description("Sort Ascending")]
    SortAscending,
    [Description("Sort Descending")]
    SortDescending,
    [Description("Hide")]
    Hide,
    [Description("UnHide")]
    UnHide
}