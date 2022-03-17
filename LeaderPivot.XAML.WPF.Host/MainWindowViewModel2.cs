using LeaderAnalytics.LeaderPivot;
using LeaderAnalytics.LeaderPivot.TestData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LeaderAnalytics.LeaderPivot.XAML.WPF;
using System.Windows.Input;

namespace LeaderPivot.XAML.WPF.Host;

internal class MainWindowViewModel2 : INotifyPropertyChanged
{
    private string _SelectedDropDownButtonItem;
    public string SelectedDropDownButtonItem
    {
        get => _SelectedDropDownButtonItem;
        set => SetProp(ref _SelectedDropDownButtonItem, value);
    }

    private PivotViewBuilder<SalesData> _ViewBuilder;
    public PivotViewBuilder<SalesData> ViewBuilder
    {
        get => _ViewBuilder;
        set => SetProp(ref _ViewBuilder, value);
    }
    private SalesDataService SalesDataService;


    private ICommand _DummyDropDownButtonCommand;
    public ICommand DummyDropDownButtonCommand
    {
        get => _DummyDropDownButtonCommand;
        set => SetProp(ref _DummyDropDownButtonCommand, value);
    }

    private ICommand _DummyListBoxCommand;
    public ICommand DummyListBoxCommand
    {
        get => _DummyListBoxCommand;
        set => SetProp(ref _DummyListBoxCommand, value);
    }


    public MainWindowViewModel2()
    {
        DummyDropDownButtonCommand = new RelayCommand(() =>
        {
            string z = SelectedDropDownButtonItem;
        });

        DummyListBoxCommand = new RelayCommand<string>((x) =>
        {
            string z = x;
        });

        SalesDataService = new SalesDataService();
        BuildMatrix();
    }

    private void BuildMatrix()
    {
        SelectedDropDownButtonItem = "World";
        List<Dimension<SalesData>> dimensions = SalesDataService.LoadDimensions();
        List<Measure<SalesData>> measures = SalesDataService.LoadMeasures();
        dimensions.First(x => x.DisplayValue == "City").IsEnabled = false;
        ViewBuilder = new PivotViewBuilder<SalesData>(dimensions, measures, LoadData, true);
    }

    public List<SalesData> LoadData()
    {
        List<SalesData> salesData = SalesDataService.GetSalesData();
        return salesData;
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
