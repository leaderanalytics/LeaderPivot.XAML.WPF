﻿using LeaderAnalytics.LeaderPivot;
using System.Windows;

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
    static DimensionContainerCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DimensionContainerCell), new FrameworkPropertyMetadata(typeof(DimensionContainerCell)));
}