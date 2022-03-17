using LeaderAnalytics.LeaderPivot;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

internal abstract class Cell : ContentControl, INotifyPropertyChanged
{
    public string? DataLabel { get; set; }
    public int RowSpan { get; set; } = 1;
    public int ColSpan { get; set; } = 1;

    static Cell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(Cell), new FrameworkPropertyMetadata(typeof(Cell)));

    #region INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler? PropertyChanged;
    public void RaisePropertyChanged([CallerMemberNameAttribute] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public void SetProp<T>(ref T prop, T value, [CallerMemberNameAttribute] string propertyName = "")
    {
        if (!Object.Equals(prop, value))
        {
            prop = value;
            RaisePropertyChanged(propertyName);
        }
    }
    #endregion
}

internal abstract class BaseTotalCell : Cell 
{

}

internal class GroupHeaderCell : Cell
{
    public bool IsExpanded
    {
        get { return (bool)GetValue(IsExpandedProperty); }
        set { SetValue(IsExpandedProperty, value); }
    }

    public static readonly DependencyProperty IsExpandedProperty =
        DependencyProperty.Register("IsExpanded", typeof(bool), typeof(GroupHeaderCell), new PropertyMetadata(true));


    public bool CanToggleExpansion
    {
        get { return (bool)GetValue(CanToggleExpansionProperty); }
        set { SetValue(CanToggleExpansionProperty, value); }
    }
    
    public static readonly DependencyProperty CanToggleExpansionProperty =
        DependencyProperty.Register("CanToggleExpansion", typeof(bool), typeof(GroupHeaderCell), new PropertyMetadata(true));


    public string NodeID
    {
        get { return (string)GetValue(NodeIDProperty); }
        set { SetValue(NodeIDProperty, value); }
    }

    public static readonly DependencyProperty NodeIDProperty =
        DependencyProperty.Register("NodeID", typeof(string), typeof(GroupHeaderCell), new PropertyMetadata(null));


    public ICommand ToggleNodeExpansionCommand
    {
        get { return (ICommand)GetValue(ToggleNodeExpansionCommandProperty); }
        set { SetValue(ToggleNodeExpansionCommandProperty, value); }
    }

    public static readonly DependencyProperty ToggleNodeExpansionCommandProperty =
        DependencyProperty.Register("ToggleNodeExpansionCommand", typeof(ICommand), typeof(GroupHeaderCell), new PropertyMetadata(null));


    static GroupHeaderCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupHeaderCell), new FrameworkPropertyMetadata(typeof(GroupHeaderCell)));
    
}

internal class GrandTotalHeaderCell : BaseTotalCell 
{
    static GrandTotalHeaderCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(GrandTotalHeaderCell), new FrameworkPropertyMetadata(typeof(GrandTotalHeaderCell)));
}

internal class GrandTotalCell : BaseTotalCell
{
    static GrandTotalCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(GrandTotalCell), new FrameworkPropertyMetadata(typeof(GrandTotalCell)));
}

internal class MeasureCell : Cell 
{
    static MeasureCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureCell), new FrameworkPropertyMetadata(typeof(MeasureCell)));
}

internal class MeasureLabelCell : Cell 
{
    static MeasureLabelCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureLabelCell), new FrameworkPropertyMetadata(typeof(MeasureLabelCell)));
}

internal class MeasureTotalLabelCell : BaseTotalCell
{
    static MeasureTotalLabelCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureTotalLabelCell), new FrameworkPropertyMetadata(typeof(MeasureTotalLabelCell)));
}

internal class TotalCell : BaseTotalCell 
{
    static TotalCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TotalCell), new FrameworkPropertyMetadata(typeof(TotalCell)));
}

internal class TotalHeaderCell : BaseTotalCell 
{
    static TotalHeaderCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TotalHeaderCell), new FrameworkPropertyMetadata(typeof(TotalHeaderCell)));
}

internal class MeasureContainerCell : Cell
{
    static MeasureContainerCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureContainerCell), new FrameworkPropertyMetadata(typeof(MeasureContainerCell)));
}
