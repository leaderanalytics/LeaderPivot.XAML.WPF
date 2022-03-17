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

internal class DimensionButton : ContentControl, INotifyPropertyChanged
{
    

    public static readonly DependencyProperty DimensionEventCommandProperty =
        DependencyProperty.Register("DimensionEventCommand", typeof(ICommand), typeof(DimensionButton), new PropertyMetadata(null));


    public ICommand SelectionChangedCommand
    {
        get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
        set { SetValue(SelectionChangedCommandProperty, value); }
    }

    public static readonly DependencyProperty SelectionChangedCommandProperty =
        DependencyProperty.Register("SelectionChangedCommand", typeof(ICommand), typeof(DimensionButton), new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true });


    public Dimension Dimension
    {
        get { return (Dimension)GetValue(DimensionProperty); }
        set { SetValue(DimensionProperty, value); }
    }

    public static readonly DependencyProperty DimensionProperty =
        DependencyProperty.Register("Dimension", typeof(Dimension), typeof(DimensionButton), new PropertyMetadata(null));


    public object SelectedItem
    {
        get { return (object)GetValue(SelectedItemProperty); }
        set { SetValue(SelectedItemProperty, value); }
    }

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(object), typeof(DimensionButton), new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true });


    public bool SortAscending
    {
        get { return (bool)GetValue(SortAscendingProperty); }
        set { SetValue(SortAscendingProperty, value); }
    }

    public static readonly DependencyProperty SortAscendingProperty =
        DependencyProperty.Register("SortAscending", typeof(bool), typeof(DimensionButton), new FrameworkPropertyMetadata(true) { BindsTwoWayByDefault=true });


    public bool SortDescending
    {
        get { return (bool)GetValue(SortDescendingProperty); }
        set { SetValue(SortDescendingProperty, value); }
    }

    public static readonly DependencyProperty SortDescendingProperty =
        DependencyProperty.Register("SortDescending", typeof(bool), typeof(DimensionButton), new FrameworkPropertyMetadata(false, SortDescendingPropertyChanged) { BindsTwoWayByDefault = true});



    static DimensionButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DimensionButton), new FrameworkPropertyMetadata(typeof(DimensionButton)));

    public DimensionButton()
    {
        SelectionChangedCommand = new RelayCommand(SelectionChangedCommandHandler);
    }

    public void SelectionChangedCommandHandler()
    {
        bool sortAscending = SortAscending;
        bool sortDescending = SortDescending;
        object selectedItem = SelectedItem;
    }

    public static void SortDescendingPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        int x = 1;
    }


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
