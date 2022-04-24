using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

public class DropDownButton : ContentControl, INotifyPropertyChanged
{
    #region BindableProperties
    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(DropDownButton), new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true});


    public ICommand SelectionChangedCommand
    {
        get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
        set { SetValue(SelectionChangedCommandProperty, value); }
    }

    public static readonly DependencyProperty SelectionChangedCommandProperty =
        DependencyProperty.Register("SelectionChangedCommand", typeof(ICommand), typeof(DropDownButton), new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true});


    public IEnumerable ItemsSource
    {
        get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DropDownButton), new PropertyMetadata(null));


    public object SelectedItem
    {
        get { return (object)GetValue(SelectedItemProperty); }
        set { SetValue(SelectedItemProperty, value); }
    }

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(object), typeof(DropDownButton), new FrameworkPropertyMetadata {BindsTwoWayByDefault = true });


    public DataTemplate ItemTemplate
    {
        get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    public static readonly DependencyProperty ItemTemplateProperty =
        DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(DropDownButton), new PropertyMetadata(null));


    public Style ButtonStyle  
    {
        get { return (Style)GetValue(ButtonStyleProperty); }
        set { SetValue(ButtonStyleProperty, value); }
    }

    public static readonly DependencyProperty ButtonStyleProperty =
        DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(DropDownButton), new PropertyMetadata(null));


    public Style ListBoxStyle
    {
        get { return (Style)GetValue(ListBoxStyleProperty); }
        set { SetValue(ListBoxStyleProperty, value); }
    }

    public static readonly DependencyProperty ListBoxStyleProperty =
        DependencyProperty.Register("ListBoxStyle", typeof(Style), typeof(DropDownButton), new PropertyMetadata(null));



    public Style ListBoxItemStyle
    {
        get { return (Style)GetValue(ListBoxItemStyleProperty); }
        set { SetValue(ListBoxItemStyleProperty, value); }
    }

    public static readonly DependencyProperty ListBoxItemStyleProperty =
        DependencyProperty.Register("ListBoxItemStyle", typeof(Style), typeof(DropDownButton), new PropertyMetadata(null));



    public Style PopupStyle
    {
        get { return (Style)GetValue(PopupStyleProperty); }
        set { SetValue(PopupStyleProperty, value); }
    }

    public static readonly DependencyProperty PopupStyleProperty =
        DependencyProperty.Register("PopupStyle", typeof(Style), typeof(DropDownButton), new PropertyMetadata(null));


    public Thickness PopupPadding
    {
        get { return (Thickness)GetValue(PopupPaddingProperty); }
        set { SetValue(PopupPaddingProperty, value); }
    }

    public static readonly DependencyProperty PopupPaddingProperty =
        DependencyProperty.Register("PopupPadding", typeof(Thickness), typeof(DropDownButton), new PropertyMetadata(new Thickness()));



    #endregion

    private bool _IsDropDownOpen;
    public bool IsDropDownOpen
    {
        get => _IsDropDownOpen;
        set => SetProp(ref _IsDropDownOpen, value);
    }

    private ICommand _ToggleDropDownCommand;
    public ICommand ToggleDropDownCommand
    {
        get => _ToggleDropDownCommand;
        set => SetProp(ref _ToggleDropDownCommand, value);
    }

    private ICommand _MouseLeaveCommand;
    public ICommand MouseLeaveCommand
    {
        get => _MouseLeaveCommand;
        set => SetProp(ref _MouseLeaveCommand,value);
    }



    

    // Using a DependencyProperty as the backing store for JunkCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty JunkCommandProperty =
        DependencyProperty.Register("JunkCommand", typeof(ICommand), typeof(DropDownButton), new PropertyMetadata(null));



    public DropDownButton()
    {
        Loaded += DropDownButton_Loaded;
        ToggleDropDownCommand = new RelayCommand(() => IsDropDownOpen = !IsDropDownOpen);
        MouseLeaveCommand = new RelayCommand(() => IsDropDownOpen = false);
    }

    private void DropDownButton_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= DropDownButton_Loaded;
        ListBox lb = GetTemplateChild("PART_ListBox") as ListBox;
        
        //if (lb == null)
        //    lb = LogicalTreeHelper.FindLogicalNode(this, "PART_ListBox") as ListBox;
        
        if(lb != null)
            lb.SelectionChanged += (s, e) => SelectionChanged(s, e);
    }

    public event EventHandler SelectionChanged;

    static DropDownButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));

    
    
    

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
