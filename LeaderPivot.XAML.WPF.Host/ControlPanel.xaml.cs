using LeaderAnalytics.LeaderPivot.XAML.WPF;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors.ColorManipulation;
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
using System.Drawing;
using System.Windows.Controls.Primitives;

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
    

    public static readonly DependencyProperty UseResponsiveSizingProperty =
        DependencyProperty.Register("UseResponsiveSizing", typeof(bool), typeof(ControlPanel), new PropertyMetadata(false));


    public bool UseDarkTheme
    {
        get { return (bool)GetValue(UseDarkThemeProperty); }
        set { SetValue(UseDarkThemeProperty, value); }
    }

    public static readonly DependencyProperty UseDarkThemeProperty =
        DependencyProperty.Register("UseDarkTheme", typeof(bool), typeof(ControlPanel), new PropertyMetadata(false, UseDarkThemeChanged));


    public int CellPadding
    {
        get { return (int)GetValue(CellPaddingProperty); }
        set { SetValue(CellPaddingProperty, value); }
    }

    public static readonly DependencyProperty CellPaddingProperty =
        DependencyProperty.Register("CellPadding", typeof(int), typeof(ControlPanel), new PropertyMetadata(4, (s, e) => ((ControlPanel)s).RaisePropertyChanged(nameof(CellPaddingString))));


    public int PivotControlFontSize
    {
        get { return (int)GetValue(PivotControlFontSizeProperty); }
        set { SetValue(PivotControlFontSizeProperty, value); }
    }

    public static readonly DependencyProperty PivotControlFontSizeProperty =
        DependencyProperty.Register("PivotControlFontSize", typeof(int), typeof(ControlPanel), new PropertyMetadata(11, (s,e) => ((ControlPanel)s).RaisePropertyChanged(nameof(FontSizeString))));


    private ICommand _TogglePanelVisibilityCommand;
    public ICommand TogglePanelVisibilityCommand
    { 
        get => _TogglePanelVisibilityCommand;
        set => SetProp(ref _TogglePanelVisibilityCommand, value);
    }

    private ICommand _ShowColorPickerPopupCommand;
    public ICommand ShowColorPickerPopupCommand
    {
        get => _ShowColorPickerPopupCommand;
        set => SetProp(ref _ShowColorPickerPopupCommand, value);
    }

    private ICommand _HideColorPickerPopupCommand;
    public ICommand HideColorPickerPopupCommand
    {
        get => _HideColorPickerPopupCommand;
        set => SetProp(ref _HideColorPickerPopupCommand, value);
    }

    private Visibility _PanelVisibility;
    public Visibility PanelVisibility
    {
        get => _PanelVisibility;
        set => SetProp(ref _PanelVisibility, value);
    }

    private System.Windows.Media.Color _PrimaryColor;
    public System.Windows.Media.Color PrimaryColor
    {
        get => _PrimaryColor;
        set
        {
            SetProp(ref _PrimaryColor, value);
            ColorsChanged();
        }
    }

    private System.Windows.Media.Color _SecondaryColor;
    public System.Windows.Media.Color SecondaryColor
    {
        get => _SecondaryColor;
        set
        {
            SetProp(ref _SecondaryColor, value);
            ColorsChanged();
        }
    }

    private bool _IsColorPickerPopupOpen;
    public bool IsColorPickerPopupOpen
    {
        get => _IsColorPickerPopupOpen;
        set => SetProp( ref _IsColorPickerPopupOpen, value);
    }

    private ICommand _SelectedThemeChangedCommand;
    public ICommand SelectedThemeChangedCommand
    {
        get => _SelectedThemeChangedCommand;
        set => SetProp(ref _SelectedThemeChangedCommand, value);
    }

    public string CellPaddingString => $"Cell Padding ({CellPadding})";
    public string FontSizeString => $"Font Size ({PivotControlFontSize})";
    

    public ControlPanel()
    {
        InitializeComponent();
        PanelVisibility = Visibility.Collapsed;
        TogglePanelVisibilityCommand = new RelayCommand(() => PanelVisibility = PanelVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        ShowColorPickerPopupCommand = new RelayCommand(() => IsColorPickerPopupOpen = true);
        HideColorPickerPopupCommand = new RelayCommand(() => IsColorPickerPopupOpen = false);
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        PrimaryColor = theme.PrimaryMid.Color;
        SecondaryColor = theme.SecondaryMid.Color;
        SelectedThemeChangedCommand = new RelayCommand<SelectionChangedEventArgs>((x) => SelectedThemeChangedCommandHandler(x));
        SetResourceDictionary("Primary", true);
    }

    public static void UseDarkThemeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        ControlPanel panel = sender as ControlPanel;
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        theme.SetBaseTheme(panel.UseDarkTheme ? Theme.Dark : Theme.Light);
        paletteHelper.SetTheme(theme);
    }

    public void ColorsChanged()
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        theme.PrimaryLight = new ColorPair(PrimaryColor.Lighten());
        theme.PrimaryMid = new ColorPair(PrimaryColor);
        theme.PrimaryDark = new ColorPair(PrimaryColor.Darken());
        
        theme.SecondaryLight = new ColorPair(SecondaryColor.Lighten());
        theme.SecondaryMid = new ColorPair(SecondaryColor);
        theme.SecondaryDark = new ColorPair(SecondaryColor.Darken());
        paletteHelper.SetTheme(theme);
    }

    private void SelectedThemeChangedCommandHandler(SelectionChangedEventArgs e)
    {
        string remove = ((ComboBoxItem)e.RemovedItems[0]).Content.ToString();
        string add = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();
        SetResourceDictionary(remove, false);
        SetResourceDictionary(add, true);
    }

    private void SetResourceDictionary(string themeName, bool add)
    {
        Uri uri = new Uri($"/LeaderPivot.XAML.WPF.Host;component/Themes/{themeName}.xaml", UriKind.Relative);

        if (add)
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
        else
            Application.Current.Resources.MergedDictionaries.Remove(Application.Current.Resources.MergedDictionaries.First(x => x.Source == uri));
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
