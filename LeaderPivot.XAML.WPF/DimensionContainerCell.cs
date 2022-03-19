using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
// https://jm2k69.github.io/2018/11/WPF-Solution-Drag-and-Drop-GongSolutions.html
// https://github.com/punker76/gong-wpf-dragdrop

namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

internal class DimensionContainerCell : Cell
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
