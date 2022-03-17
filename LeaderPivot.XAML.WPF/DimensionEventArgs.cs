using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

public class DimensionEventArgs  
{
    //  Dimension.DisplayName is guaranteed to be unique and is also the ID
    public string DimensionID { get; set; }
    public DimensionAction Action { get; set; }
}
