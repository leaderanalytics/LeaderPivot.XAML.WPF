using LeaderAnalytics.LeaderPivot;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

public abstract class BaseCell : ContentControl, INotifyPropertyChanged
{
    public int RowSpan { get; set; } = 1;
    public int ColSpan { get; set; } = 1;

    static BaseCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseCell), new FrameworkPropertyMetadata(typeof(BaseCell)));

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

public abstract class BaseTotalCell : BaseCell 
{

}

public class DimensionContainerCell : BaseCell
{
    public IList<Dimension> Dimensions
    {
        get { return (IList<Dimension>)GetValue(DimensionsProperty); }
        set { SetValue(DimensionsProperty, value); }
    }

    public static readonly DependencyProperty DimensionsProperty =
        DependencyProperty.Register("Dimensions", typeof(IList<Dimension>), typeof(DimensionContainerCell), new PropertyMetadata(null));

    public bool IsRows
    {
        get { return (bool)GetValue(IsRowsProperty); }
        set { SetValue(IsRowsProperty, value); }
    }

    public static readonly DependencyProperty IsRowsProperty =
        DependencyProperty.Register("IsRows", typeof(bool), typeof(DimensionContainerCell), new PropertyMetadata(false));

    public int MaxDimensions { get; set; }

    public DimensionContainerCell()
    {

    }

    static DimensionContainerCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DimensionContainerCell), new FrameworkPropertyMetadata(typeof(DimensionContainerCell)));

}

public class GroupHeaderCell : BaseCell
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

public class GrandTotalHeaderCell : BaseTotalCell 
{
    static GrandTotalHeaderCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(GrandTotalHeaderCell), new FrameworkPropertyMetadata(typeof(GrandTotalHeaderCell)));
}

public class GrandTotalCell : BaseTotalCell
{
    static GrandTotalCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(GrandTotalCell), new FrameworkPropertyMetadata(typeof(GrandTotalCell)));
}

public class MeasureCell : BaseCell 
{
    static MeasureCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureCell), new FrameworkPropertyMetadata(typeof(MeasureCell)));
}

public class MeasureContainerCell : BaseCell
{
    public Style ReloadButtonStyle
    {
        get { return (Style)GetValue(ReloadButtonStyleProperty); }
        set { SetValue(ReloadButtonStyleProperty, value); }
    }

    public static readonly DependencyProperty ReloadButtonStyleProperty =
        DependencyProperty.Register("ReloadButtonStyle", typeof(Style), typeof(MeasureContainerCell), new PropertyMetadata(null));

    public Style MeasureCheckBoxStyle
    {
        get { return (Style)GetValue(MeasureCheckBoxStyleProperty); }
        set { SetValue(MeasureCheckBoxStyleProperty, value); }
    }

    public static readonly DependencyProperty MeasureCheckBoxStyleProperty =
        DependencyProperty.Register("MeasureCheckBoxStyle", typeof(Style), typeof(MeasureContainerCell), new PropertyMetadata(null));


    public Style HiddenDimensionsListBoxStyle
    {
        get { return (Style)GetValue(HiddenDimensionsListBoxStyleProperty); }
        set { SetValue(HiddenDimensionsListBoxStyleProperty, value); }
    }

    public static readonly DependencyProperty HiddenDimensionsListBoxStyleProperty =
        DependencyProperty.Register("HiddenDimensionsListBoxStyle", typeof(Style), typeof(MeasureContainerCell), new PropertyMetadata(null));

    static MeasureContainerCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureContainerCell), new FrameworkPropertyMetadata(typeof(MeasureContainerCell)));
}

public class MeasureLabelCell : BaseCell 
{
    static MeasureLabelCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureLabelCell), new FrameworkPropertyMetadata(typeof(MeasureLabelCell)));
}

public class MeasureTotalLabelCell : BaseTotalCell
{
    static MeasureTotalLabelCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureTotalLabelCell), new FrameworkPropertyMetadata(typeof(MeasureTotalLabelCell)));
}

public class TotalCell : BaseTotalCell 
{
    static TotalCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TotalCell), new FrameworkPropertyMetadata(typeof(TotalCell)));
}

public class TotalHeaderCell : BaseTotalCell 
{
    static TotalHeaderCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TotalHeaderCell), new FrameworkPropertyMetadata(typeof(TotalHeaderCell)));
}
