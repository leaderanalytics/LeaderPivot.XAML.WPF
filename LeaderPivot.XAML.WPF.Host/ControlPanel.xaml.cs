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


    public bool UseResponsiveSizing
    {
        get { return (bool)GetValue(UseResponsiveSizingProperty); }
        set { SetValue(UseResponsiveSizingProperty, value); }
    }

    public static readonly DependencyProperty UseResponsiveSizingProperty =
        DependencyProperty.Register("UseResponsiveSizing", typeof(bool), typeof(ControlPanel), new PropertyMetadata(false));


    public int CellPadding
    {
        get { return (int)GetValue(CellPaddingProperty); }
        set { SetValue(CellPaddingProperty, value); }
    }

    public static readonly DependencyProperty CellPaddingProperty =
        DependencyProperty.Register("CellPadding", typeof(int), typeof(ControlPanel), new PropertyMetadata(4, (s, e) => ((ControlPanel)s).RaisePropertyChanged("CellPaddingString")));


    public int PivotControlFontSize
    {
        get { return (int)GetValue(PivotControlFontSizeProperty); }
        set { SetValue(PivotControlFontSizeProperty, value); }
    }

    public static readonly DependencyProperty PivotControlFontSizeProperty =
        DependencyProperty.Register("PivotControlFontSize", typeof(int), typeof(ControlPanel), new PropertyMetadata(11, (s,e) => ((ControlPanel)s).RaisePropertyChanged("FontSizeString")));


    public string CellPaddingString => $"Cell Padding ({CellPadding})";
    public string FontSizeString => $"Font Size ({PivotControlFontSize})";


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
