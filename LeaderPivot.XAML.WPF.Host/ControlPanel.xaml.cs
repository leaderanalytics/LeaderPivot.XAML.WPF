using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeaderPivot.XAML.WPF.Host;

public partial class ControlPanel : UserControl, INotifyPropertyChanged
{
    public bool DisplayGrandTotalOption
    {
        get { return (bool)GetValue(DisplayGrandTotalOptionProperty); }
        set { SetValue(DisplayGrandTotalOptionProperty, value); }
    }

    public static readonly DependencyProperty DisplayGrandTotalOptionProperty =
        DependencyProperty.Register("DisplayGrandTotalOption", typeof(bool), typeof(ControlPanel), new PropertyMetadata(true));


    public bool DisplayDimensionButtons
    {
        get { return (bool)GetValue(DisplayDimensionButtonsProperty); }
        set { SetValue(DisplayDimensionButtonsProperty, value); }
    }

    public static readonly DependencyProperty DisplayDimensionButtonsProperty =
        DependencyProperty.Register("DisplayDimensionButtons", typeof(bool), typeof(ControlPanel), new PropertyMetadata(true));


    public bool DisplayMeasureSelectors
    {
        get { return (bool)GetValue(DisplayMeasureSelectorsProperty); }
        set { SetValue(DisplayMeasureSelectorsProperty, value); }
    }

    public static readonly DependencyProperty DisplayMeasureSelectorsProperty =
        DependencyProperty.Register("DisplayMeasureSelectors", typeof(bool), typeof(ControlPanel), new PropertyMetadata(true));


    public bool DisplayReloadDataButton
    {
        get { return (bool)GetValue(DisplayReloadDataButtonProperty); }
        set { SetValue(DisplayReloadDataButtonProperty, value); }
    }

    public static readonly DependencyProperty DisplayReloadDataButtonProperty =
        DependencyProperty.Register("DisplayReloadDataButton", typeof(bool), typeof(ControlPanel), new PropertyMetadata(false));



    public string SelectedPivotStyle
    {
        get { return (string)GetValue(SelectedPivotStyleProperty); }
        set { SetValue(SelectedPivotStyleProperty, value); }
    }

    public static readonly DependencyProperty SelectedPivotStyleProperty =
        DependencyProperty.Register("SelectedPivotStyle", typeof(string), typeof(ControlPanel), new PropertyMetadata(null));



    public ControlPanel()
    {
        InitializeComponent();
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
