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
        Measures = MeasuresT.OrderBy(x => x.Sequence).Select(x => new Selectable<Measure>(x, x.IsEnabled)).ToList();
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


public abstract class PivotViewBuilder : PropertyChangedBase
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

    private IList<Selectable<Measure>> _Measures;
    public IList<Selectable<Measure>> Measures
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
}