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
using Microsoft.Toolkit.Mvvm.Input;

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

    public PivotViewBuilder ViewBuilder
    {
        get { return (PivotViewBuilder)GetValue(ViewBuilderProperty); }
        set { SetValue(ViewBuilderProperty, value); }
    }

    public static readonly DependencyProperty ViewBuilderProperty =
        DependencyProperty.Register("ViewBuilder", typeof(PivotViewBuilder), typeof(LeaderPivotControl), new PropertyMetadata(null, ViewBuilderPropertyChanged));


    public bool DisplayGrandTotalOption
    {
        get { return (bool)GetValue(DisplayGrandTotalOptionProperty); }
        set { SetValue(DisplayGrandTotalOptionProperty, value); }
    }

    public static readonly DependencyProperty DisplayGrandTotalOptionProperty =
        DependencyProperty.Register("DisplayGrandTotalOption", typeof(bool), typeof(LeaderPivotControl), new PropertyMetadata(true));


    public bool DisplayDimensionButtons
    {
        get { return (bool)GetValue(DisplayDimensionButtonsProperty); }
        set { SetValue(DisplayDimensionButtonsProperty, value); }
    }

    public static readonly DependencyProperty DisplayDimensionButtonsProperty =
        DependencyProperty.Register("DisplayDimensionButtons", typeof(bool), typeof(LeaderPivotControl), new PropertyMetadata(true));


    public bool DisplayMeasureSelectors
    {
        get { return (bool)GetValue(DisplayMeasureSelectorsProperty); }
        set { SetValue(DisplayMeasureSelectorsProperty, value); }
    }

    public static readonly DependencyProperty DisplayMeasureSelectorsProperty =
        DependencyProperty.Register("DisplayMeasureSelectors", typeof(bool), typeof(LeaderPivotControl), new PropertyMetadata(true));


    public bool DisplayReloadDataButton
    {
        get { return (bool)GetValue(DisplayReloadDataButtonProperty); }
        set { SetValue(DisplayReloadDataButtonProperty, value); }
    }

    public static readonly DependencyProperty DisplayReloadDataButtonProperty =
        DependencyProperty.Register("DisplayReloadDataButton", typeof(bool), typeof(LeaderPivotControl), new FrameworkPropertyMetadata(false));



    public bool IsLoading
    {
        get { return (bool)GetValue(IsLoadingProperty); }
        set { SetValue(IsLoadingProperty, value); }
    }

    public static readonly DependencyProperty IsLoadingProperty =
        DependencyProperty.Register("IsLoading", typeof(bool), typeof(LeaderPivotControl), new FrameworkPropertyMetadata(false));




    public ICommand ReloadDataCommand   
    {
        get { return (ICommand)GetValue(ReloadDataCommandProperty); }
        set { SetValue(ReloadDataCommandProperty, value); }
    }

    public static readonly DependencyProperty ReloadDataCommandProperty =
        DependencyProperty.Register("command", typeof(ICommand), typeof(LeaderPivotControl), new PropertyMetadata(null));


    public ICommand MeasureCheckedChangedCommand
    {
        get { return (ICommand)GetValue(MeasureCheckedChangedProperty); }
        set { SetValue(MeasureCheckedChangedProperty, value); }
    }

    public static readonly DependencyProperty MeasureCheckedChangedProperty =
        DependencyProperty.Register("MeasureCheckedChanged", typeof(ICommand), typeof(LeaderPivotControl), new PropertyMetadata(null));


    private byte[,]? table;
    private Grid grid;

    
    static LeaderPivotControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(LeaderPivotControl), new FrameworkPropertyMetadata(typeof(LeaderPivotControl)));

    public LeaderPivotControl()
    {
        ReloadDataCommand = new RelayCommand(BuildGrid);
        MeasureCheckedChangedCommand = new RelayCommand<Measure>(MeasureCheckedChanged);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        grid = (Grid) Template.FindName("PART_Grid", this);
    }

    public static void ViewBuilderPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    { 
        ((LeaderPivotControl)sender).BuildGrid();
    }

    public void MeasureCheckedChanged(Measure measure)
    { 
        BuildGrid();
    }


    public void BuildGrid()
    {
        IsLoading = true;
        ViewBuilder.BuildMatrix();
        grid.Children.Clear();
        grid.RowDefinitions.Clear();
        grid.ColumnDefinitions.Clear();
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
                    CellType.Measure => new MeasureCell(),
                    CellType.Total => new TotalCell(),
                    CellType.GrandTotal => new GrandTotalCell(),
                    CellType.GroupHeader => new GroupHeaderCell(),
                    CellType.TotalHeader => new TotalHeaderCell(),
                    CellType.GrandTotalHeader => new GrandTotalHeaderCell(),
                    CellType.MeasureTotalLabel => new MeasureTotalLabelCell(),
                    CellType.MeasureLabel when i == 0 && j == 0 => new MeasureContainerCell() ,
                    CellType.MeasureLabel when i == 0 && j == 1 => new DimensionContainerCell {  Dimensions = ViewBuilder.ColumnDimensions, IsRows = false, Padding = new Thickness(6,0,0,0)},
                    CellType.MeasureLabel => new MeasureLabelCell(),
                    _ => throw new NotImplementedException()
                };

                cell.RowSpan = mCell.RowSpan;
                cell.ColSpan = mCell.ColSpan;
                cell.Content = mCell.Value?.ToString(); 
                columnIndex = IncrementCol(i, columnIndex, cell);
                Grid.SetRow(cell, i);
                Grid.SetRowSpan(cell, cell.RowSpan);
                Grid.SetColumn(cell, columnIndex);
                Grid.SetColumnSpan(cell, cell.ColSpan);
                grid.Children.Add(cell);
            }
        }
        IsLoading = false;
    }

    private int IncrementCol(int rowIndex, int colIndex, Cell cell)
    {
        while (table[rowIndex, colIndex] == 1)
            colIndex++;

        for (int r = rowIndex; r < rowIndex + cell.RowSpan; r++)
            for (int c = colIndex; c < colIndex + cell.ColSpan; c++)
                table[r, c] = 1;

        return colIndex;
    }
}
