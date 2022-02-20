using LeaderAnalytics.LeaderPivot.TestData;
using LeaderAnalytics.LeaderPivot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LeaderPivot.XAML.WPF.Host;

internal class MainWindowViewModel : INotifyPropertyChanged
{
    private Grid? _Grid;
    public Grid? Grid
    {
        get => _Grid;
        set => SetProp(ref _Grid, value);
    }
    private byte[,] table;
    

    public MainWindowViewModel()
    {
        Grid = new Grid();
        Grid.Background = new SolidColorBrush(Colors.Cyan);
        Grid.ShowGridLines = true;
        BuildMatrix();

    }


    private void BuildMatrix()
    {
        // Row span takes precidence over column span.
        // In other words if a cell takes multiple rows, cells in the second and subsequent rows are pushed to the right - not down.
        // A cell is never pushed down to a lower row - it is pushed to the right.  Therefore it's row index in the matrix is always the same as it's row number in the grid.

        SalesDataService salesDataService = new SalesDataService();
        List<SalesData> salesData = salesDataService.GetSalesData();
        List<Dimension<SalesData>> dimensions = salesDataService.LoadDimensions();
        List<Measure<SalesData>> measures = salesDataService.LoadMeasures();
        bool displayGrandTotals = true;
        NodeBuilder<SalesData> nodeBuilder = new NodeBuilder<SalesData>();
        Validator<SalesData> validator = new Validator<SalesData>();
        MatrixBuilder<SalesData> matrixBuilder = new MatrixBuilder<SalesData>(nodeBuilder, validator);
        LeaderAnalytics.LeaderPivot.Matrix matrix = matrixBuilder.BuildMatrix(salesData, dimensions, measures, displayGrandTotals);
        int rowCount = matrix.Rows.Count;
        int columnCount = matrix.Rows[0].Cells.Sum(x => x.ColSpan);
        table = new byte[rowCount,columnCount];

        for (int j = 0; j < columnCount; j++)
            Grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        for (int i = 0; i < rowCount; i++)
        {
            Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            int columnIndex = 0;

            for (int j = 0; j < matrix.Rows[i].Cells.Count; j++)
            {
                MatrixCell cell = matrix.Rows[i].Cells[j];
                TextBox textBlock = new TextBox();
                textBlock.Text = cell.Value?.ToString();
                Grid.SetRow(textBlock, i);
                Grid.SetRowSpan(textBlock, cell.RowSpan);
                columnIndex = IncrementCol(i, columnIndex, cell);
                Grid.SetColumn(textBlock, columnIndex);
                Grid.SetColumnSpan(textBlock, cell.ColSpan);
                Grid.Children.Add(textBlock);
            }
        }
    }

    

    private int IncrementCol(int rowIndex, int startingCol, MatrixCell cell)
    {
        int colIndex = startingCol;

        while(table[rowIndex,colIndex] == 1)
            colIndex++;

        for (int r = rowIndex; r < rowIndex + cell.RowSpan; r++)
            for(int c = colIndex; c < colIndex + cell.ColSpan; c++ )
            table[r,c] = 1;

        return colIndex;
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
