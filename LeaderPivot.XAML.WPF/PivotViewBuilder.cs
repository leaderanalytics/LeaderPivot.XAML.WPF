using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using LeaderAnalytics.LeaderPivot;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

public class PivotViewBuilder<T> : PivotViewBuilder
{
    private IEnumerable<Dimension<T>> DimensionsT;
    private IEnumerable<Measure<T>> MeasuresT;
    private Func<Task<IEnumerable<T>>> LoadData;
    private MatrixBuilder<T> MatrixBuilder;

    public PivotViewBuilder(IEnumerable<Dimension<T>> dimensions, IEnumerable<Measure<T>> measures, Func<IEnumerable<T>> loadData, bool displayGrandTotals = true)
        : this(dimensions, measures, () => Task.FromResult(loadData()), displayGrandTotals)
    {
        
    }

    public PivotViewBuilder(IEnumerable<Dimension<T>> dimensions, IEnumerable<Measure<T>> measures, Func<Task<IEnumerable<T>>> loadData, bool displayGrandTotals = true)
    {
        DimensionsT = dimensions;
        MeasuresT = measures;
        Dimensions = DimensionsT.ToList<Dimension>();
        Measures = MeasuresT.OrderBy(x => x.Sequence).ToList<Measure>();
        DisplayGrandTotals = displayGrandTotals;
        LoadData = loadData;
        NodeBuilder<T> nodeBuilder = new NodeBuilder<T>();
        Validator<T> validator = new Validator<T>();
        MatrixBuilder = new MatrixBuilder<T>(nodeBuilder, validator);
    }
    
    public override async Task BuildMatrix(string? nodeID = null)
    {
        if (string.IsNullOrEmpty(nodeID))
        {
            RowDimensions = Dimensions.Where(x => x.IsEnabled && x.IsRow).OrderBy(x => x.Sequence).ToList();
            ColumnDimensions = Dimensions.Where(x => x.IsEnabled && !x.IsRow).OrderBy(x => x.Sequence).ToList();
            HiddenDimensions = Dimensions.Where(x => !x.IsEnabled).OrderBy(x => x.Sequence).ToList();
            Matrix = MatrixBuilder.BuildMatrix(await LoadData(), DimensionsT, MeasuresT, DisplayGrandTotals);
        }
        else
            Matrix = MatrixBuilder.ToggleNodeExpansion(nodeID);
    }
}


public abstract class PivotViewBuilder : INotifyPropertyChanged
{
    private bool _DisplayGrandTotals;
    public bool DisplayGrandTotals
    {
        get => _DisplayGrandTotals;
        set => SetProp(ref _DisplayGrandTotals, value);
    }

    private IList<Dimension> _Dimensions;
    public IList<Dimension> Dimensions
    {
        get => _Dimensions;
        protected set => SetProp(ref _Dimensions, value);
    }

    private IList<Dimension> _RowDimensions;
    public IList<Dimension> RowDimensions
    {
        get => _RowDimensions;
        protected set => SetProp(ref _RowDimensions, value);
    }

    private IList<Dimension> _ColumnDimensions;
    public IList<Dimension> ColumnDimensions
    {
        get => _ColumnDimensions;
        protected set => SetProp(ref _ColumnDimensions, value);
    }

    private IList<Dimension> _HiddenDimensions;
    public IList<Dimension> HiddenDimensions
    {
        get => _HiddenDimensions;
        protected set 
        {
            SetProp(ref _HiddenDimensions, value);
            RaisePropertyChanged(nameof(HiddenDimensionsVisibility));
        }
    }

    private IList<Measure> _Measures;
    public IList<Measure> Measures
    {
        get => _Measures;
        protected set => SetProp(ref _Measures, value);
    }

    private LeaderAnalytics.LeaderPivot.Matrix _Matrix;
    public LeaderAnalytics.LeaderPivot.Matrix? Matrix
    {
        get => _Matrix;
        protected set => SetProp(ref _Matrix, value);
    }

    public Visibility HiddenDimensionsVisibility => (HiddenDimensions?.Any() ?? false) ? Visibility.Visible : Visibility.Collapsed;

    public abstract Task BuildMatrix(string? nodeID);

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