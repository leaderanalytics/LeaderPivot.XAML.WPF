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

public class DimensionButton : DropDownButton, ICommandSource
{
    public Dimension Dimension
    {
        get { return (Dimension)GetValue(DimensionProperty); }
        set { SetValue(DimensionProperty, value); }
    }



    public object CommandParameter
    {
        get { return (object)GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register("CommandParameter", typeof(object), typeof(DimensionButton), new PropertyMetadata(null));


    public IInputElement CommandTarget
    {
        get { return (IInputElement)GetValue(CommandTargetProperty); }
        set { SetValue(CommandTargetProperty, value); }
    }

    public static readonly DependencyProperty CommandTargetProperty =
        DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(DimensionButton), new PropertyMetadata(null));
    

    public static readonly DependencyProperty DimensionProperty =
        DependencyProperty.Register("Dimension", typeof(Dimension), typeof(DimensionButton), new PropertyMetadata(null));


    static DimensionButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DimensionButton), new FrameworkPropertyMetadata(typeof(DimensionButton)));
}
