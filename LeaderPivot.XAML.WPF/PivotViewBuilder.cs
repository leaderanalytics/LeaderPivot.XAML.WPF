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
namespace LeaderPivot.XAML.WPF;

public class PivotViewBuilder<T> : PivotViewBuilder
{
    private IEnumerable<Dimension<T>> Dimensions;
    private IEnumerable<Measure<T>> MeasuresT;
    private Func<IEnumerable<T>> LoadData;
    private MatrixBuilder<T> MatrixBuilder;

    public PivotViewBuilder(IEnumerable<Dimension<T>> dimensions, IEnumerable<Measure<T>> measures, Func<IEnumerable<T>> loadData, bool displayGrandTotals = true)
    {
        DisplayGrandTotals = displayGrandTotals;
        Dimensions = dimensions;
        MeasuresT = measures;
        LoadData = loadData;
        RowDimensions = dimensions.Where(x => x.IsRow).OrderBy(x => x.Sequence).ToList<Dimension>();
        ColumnDimensions = dimensions.Where(x => !x.IsRow).OrderBy(x => x.Sequence).ToList<Dimension>();
        Measures = MeasuresT.OrderBy(x => x.Sequence).ToList<Measure>();
        NodeBuilder<T> nodeBuilder = new NodeBuilder<T>();
        Validator<T> validator = new Validator<T>();
        MatrixBuilder = new MatrixBuilder<T>(nodeBuilder, validator);
    }

    public override void BuildMatrix() => Matrix = MatrixBuilder.BuildMatrix(LoadData(), Dimensions, MeasuresT, DisplayGrandTotals);
}


public abstract class PivotViewBuilder : INotifyPropertyChanged
{
    private bool _DisplayGrandTotals;
    public bool DisplayGrandTotals
    {
        get => _DisplayGrandTotals;
        protected set => SetProp(ref _DisplayGrandTotals, value);
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

    public abstract void BuildMatrix();

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