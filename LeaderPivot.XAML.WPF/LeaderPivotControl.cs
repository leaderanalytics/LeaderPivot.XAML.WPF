using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LeaderAnalytics.LeaderPivot;

namespace LeaderPivot.XAML.WPF;
/// <summary>
/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
///
/// Step 1a) Using this custom control in a XAML file that exists in the current project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:LeaderPivot.XAML.WPF"
///
///
/// Step 1b) Using this custom control in a XAML file that exists in a different project.
/// Add this XmlNamespace attribute to the root element of the markup file where it is 
/// to be used:
///
///     xmlns:MyNamespace="clr-namespace:LeaderPivot.XAML.WPF;assembly=LeaderPivot.XAML.WPF"
///
/// You will also need to add a project reference from the project where the XAML file lives
/// to this project and Rebuild to avoid compilation errors:
///
///     Right click on the target project in the Solution Explorer and
///     "Add Reference"->"Projects"->[Select this project]
///
///
/// Step 2)
/// Go ahead and use your control in the XAML file.
///
///     <MyNamespace:CustomControl1/>
///
/// </summary>
public class LeaderPivotControl: ContentControl
{
    public bool DisplayGrandTotals
    {
        get { return (bool)GetValue(DisplayGrandTotalsProperty); }
        set { SetValue(DisplayGrandTotalsProperty, value); }
    }

    public static readonly DependencyProperty DisplayGrandTotalsProperty =
        DependencyProperty.Register("DisplayGrandTotals", typeof(bool), typeof(LeaderPivotControl), new PropertyMetadata(false));


    public PivotViewBuilder ViewBuilder
    {
        get { return (PivotViewBuilder)GetValue(ViewBuilderProperty); }
        set { SetValue(ViewBuilderProperty, value); }
    }

    public static readonly DependencyProperty ViewBuilderProperty =
        DependencyProperty.Register("ViewBuilder", typeof(PivotViewBuilder), typeof(LeaderPivotControl), new PropertyMetadata(null,ViewBuilderPropertyChanged));


    private byte[,]? table;
    private Grid grid;

    
    static LeaderPivotControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LeaderPivotControl), new FrameworkPropertyMetadata(typeof(LeaderPivotControl)));
    }

    public LeaderPivotControl()
    {
    
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        grid = (Grid) Template.FindName("PART_Grid", this);
    }

    private void LeaderPivotControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    public static void ViewBuilderPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    { 
        ((LeaderPivotControl)sender).BuildGrid();
    }

    public void BuildGrid()
    {
        int rowCount = ViewBuilder.Matrix.Rows.Count;
        int columnCount = ViewBuilder.Matrix.Rows[0].Cells.Sum(x => x.ColSpan);

        table = new byte[rowCount, columnCount];

        for (int j = 0; j < columnCount; j++)
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        for (int i = 0; i < rowCount; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            int columnIndex = 0;

            for (int j = 0; j < ViewBuilder.Matrix.Rows[i].Cells.Count; j++)
            {
                MatrixCell? mCell = ViewBuilder.Matrix.Rows[i].Cells[j];

                Cell cell = mCell.CellType switch
                {
                    CellType.Measure => new MeasureCell(mCell),
                    CellType.Total => new TotalCell(mCell),
                    CellType.GrandTotal => new GrandTotalCell(mCell),
                    CellType.GroupHeader => new GroupHeaderCell(mCell),
                    CellType.TotalHeader => new TotalHeaderCell(mCell),
                    CellType.GrandTotalHeader => new GrandTotalHeaderCell(mCell),
                    CellType.MeasureTotalLabel => new MeasureTotalLabelCell(mCell),
                    CellType.MeasureLabel when i == 0 && j == 0 => new MeasureContainerCell(mCell),
                    CellType.MeasureLabel when i == 0 && j == 1 => new DimensionContainerCell(mCell),
                    CellType.MeasureLabel => new MeasureLabelCell(mCell),
                    _ => throw new NotImplementedException()
                };

                columnIndex = IncrementCol(i, columnIndex, cell);
                Grid.SetRow(cell, i);
                Grid.SetRowSpan(cell, cell.RowSpan);
                Grid.SetColumn(cell, columnIndex);
                Grid.SetColumnSpan(cell, cell.ColSpan);
                grid.Children.Add(cell);
            }
        }
    }

    private int IncrementCol(int rowIndex, int startingCol, Cell cell)
    {
        int colIndex = startingCol;

        while (table[rowIndex, colIndex] == 1)
            colIndex++;

        for (int r = rowIndex; r < rowIndex + cell.RowSpan; r++)
            for (int c = colIndex; c < colIndex + cell.ColSpan; c++)
                table[r, c] = 1;

        return colIndex;
    }
}
