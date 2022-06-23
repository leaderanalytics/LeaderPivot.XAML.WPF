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
using System.Windows.Controls;

namespace LeaderPivot.XAML.WPF.Host;

internal class MainWindowViewModel : INotifyPropertyChanged
{
    private PivotViewBuilder<SalesData> _ViewBuilder;
    public PivotViewBuilder<SalesData> ViewBuilder
    {
        get => _ViewBuilder;
        set => SetProp(ref _ViewBuilder, value);
    }

    private bool _IsBusy;
    public bool IsBusy
    {
        get => _IsBusy;
        set => SetProp(ref _IsBusy, value);
    }

    private SalesDataService SalesDataService;
    

    public MainWindowViewModel()
    {
        IsBusy = true;
        SalesDataService = new SalesDataService();
        BuildMatrix();
    }

    private void BuildMatrix()
    {
        List<Dimension<SalesData>> dimensions = SalesDataService.LoadDimensions();
        List<Measure<SalesData>> measures = SalesDataService.LoadMeasures();
        dimensions.First(x => x.DisplayValue == "City").IsEnabled = false;
        ViewBuilder = new PivotViewBuilder<SalesData>(dimensions, measures, LoadDataAsync, true);
    }

    public async Task<IEnumerable<SalesData>> LoadDataAsync()
    {
        List<SalesData> salesData = SalesDataService.GetSalesData();
        return await Task.FromResult(salesData);
        //await Task.Delay(1);
        //return salesData;
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
