using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

internal class DimensionButton : ContentControl
{
    public Dimension Dimension
    {
        get { return (Dimension)GetValue(DimensionProperty); }
        set { SetValue(DimensionProperty, value); }
    }

    public static readonly DependencyProperty DimensionProperty =
        DependencyProperty.Register("Dimension", typeof(Dimension), typeof(DimensionButton), new PropertyMetadata(null));

    static DimensionButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DimensionButton), new FrameworkPropertyMetadata(typeof(DimensionButton)));
}
