﻿namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

public class DimensionButton : DropDownButton, ICommandSource
{
    public ICommand CheckboxCheckedCommand
    {
        get { return (ICommand)GetValue(CheckboxCheckedCommandProperty); }
        set { SetValue(CheckboxCheckedCommandProperty, value); }
    }

    public static readonly DependencyProperty CheckboxCheckedCommandProperty =
        DependencyProperty.Register("CheckboxCheckedCommand", typeof(ICommand), typeof(DimensionButton), new PropertyMetadata(null));


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


    public Dimension Dimension
    {
        get { return (Dimension)GetValue(DimensionProperty); }
        set { SetValue(DimensionProperty, value); }
    }

    public static readonly DependencyProperty DimensionProperty =
        DependencyProperty.Register("Dimension", typeof(Dimension), typeof(DimensionButton), new PropertyMetadata(null));


    static DimensionButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DimensionButton), new FrameworkPropertyMetadata(typeof(DimensionButton)));

    public DimensionButton() => CheckboxCheckedCommand = new RelayCommand<DimensionAction>(x => SelectedItem = x);
}
