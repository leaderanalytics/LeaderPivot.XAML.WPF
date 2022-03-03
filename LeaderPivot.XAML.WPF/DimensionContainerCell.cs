using LeaderAnalytics.LeaderPivot;
using System.Windows;

namespace LeaderPivot.XAML.WPF;

internal class DimensionContainerCell : Cell
{

    public List<Dimension> Dimensions
    {
        get { return (List<Dimension>)GetValue(DimensionsProperty); }
        set { SetValue(DimensionsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Dimensions.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DimensionsProperty =
        DependencyProperty.Register("Dimensions", typeof(List<Dimension>), typeof(DimensionContainerCell), new PropertyMetadata(null));


    public int MaxDimensions { get; set; }
    public bool IsRows { get; set; }

    static DimensionContainerCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DimensionContainerCell), new FrameworkPropertyMetadata(typeof(DimensionContainerCell)));
}
