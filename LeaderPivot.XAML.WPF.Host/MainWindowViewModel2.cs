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

namespace LeaderPivot.XAML.WPF.Host;

internal class MainWindowViewModel2 : INotifyPropertyChanged
{
    private PivotViewBuilder<SalesData> _ViewBuilder;
    public PivotViewBuilder<SalesData> ViewBuilder
    {
        get => _ViewBuilder;
        set => SetProp(ref _ViewBuilder, value);
    }
    private SalesDataService SalesDataService;

    public MainWindowViewModel2()
    {
        SalesDataService = new SalesDataService();
        BuildMatrix();
    }

    private void BuildMatrix()
    {
        List<Dimension<SalesData>> dimensions = SalesDataService.LoadDimensions();
        List<Measure<SalesData>> measures = SalesDataService.LoadMeasures();
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
