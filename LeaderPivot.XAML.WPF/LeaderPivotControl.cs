/* 
 * Copyright 2021 Leader Analytics 
 * LeaderAnalytics.com
 * SamWheat.com
 * 
 * Please do not remove this header.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

public class LeaderPivotControl: ContentControl
{
    #region Properties
    public PivotViewBuilder ViewBuilder
    {
        get { return (PivotViewBuilder)GetValue(ViewBuilderProperty); }
        set { SetValue(ViewBuilderProperty, value); }
    }

    public static readonly DependencyProperty ViewBuilderProperty =
        DependencyProperty.Register("ViewBuilder", typeof(PivotViewBuilder), typeof(LeaderPivotControl), new PropertyMetadata(null, (s,e) => ((LeaderPivotControl)s).BuildGrid(null)));


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


    public bool UseResponsiveSizing
    {
        get { return (bool)GetValue(UseResponsiveSizingProperty); }
        set { SetValue(UseResponsiveSizingProperty, value); }
    }
    
    public static readonly DependencyProperty UseResponsiveSizingProperty =
        DependencyProperty.Register("UseResponsiveSizing", typeof(bool), typeof(LeaderPivotControl), new PropertyMetadata(false));


    public int CellPadding
    {
        get { return (int)GetValue(CellPaddingProperty); }
        set { SetValue(CellPaddingProperty, value); }
    }

    public static readonly DependencyProperty CellPaddingProperty =
        DependencyProperty.Register("CellPadding", typeof(int), typeof(LeaderPivotControl), new PropertyMetadata(4));


    public bool IsLoading
    {
        get { return (bool)GetValue(IsLoadingProperty); }
        set { SetValue(IsLoadingProperty, value); }
    }

    public static readonly DependencyProperty IsLoadingProperty =
        DependencyProperty.Register("IsLoading", typeof(bool), typeof(LeaderPivotControl), new FrameworkPropertyMetadata(false));

    #endregion

    #region Commands

    public ICommand ReloadDataCommand   
    {
        get { return (ICommand)GetValue(ReloadDataCommandProperty); }
        set { SetValue(ReloadDataCommandProperty, value); }
    }

    public static readonly DependencyProperty ReloadDataCommandProperty =
        DependencyProperty.Register("command", typeof(ICommand), typeof(LeaderPivotControl), new PropertyMetadata(null));


    public ICommand DimensionEventCommand
    {
        get { return (ICommand)GetValue(DimensionEventCommandProperty); }
        set { SetValue(DimensionEventCommandProperty, value); }
    }

    public static readonly DependencyProperty DimensionEventCommandProperty =
        DependencyProperty.Register("DimensionEventCommand", typeof(ICommand), typeof(LeaderPivotControl), new PropertyMetadata(null));




    #endregion

    private byte[,]? table;
    private Grid grid;
    private ICommand toggleNodeExpansionCommand;
    
    static LeaderPivotControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(LeaderPivotControl), new FrameworkPropertyMetadata(typeof(LeaderPivotControl)));

    public LeaderPivotControl()
    {
        ReloadDataCommand = new RelayCommand(() => BuildGrid(null));
        DimensionEventCommand = new RelayCommand<DimensionEventArgs>(DimensionEventCommandHandler);
        toggleNodeExpansionCommand = new RelayCommand<string>(x => BuildGrid(x));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        grid = (Grid) Template.FindName("PART_Grid", this);
    }
    
    public void BuildGrid(string? nodeID)
    {
        // Row span takes precidence over column span.
        // If a cell spans multiple rows, cells in the second and subsequent rows are pushed to the right - not down.
        // A cell is never pushed down to a lower row - it is pushed to the right.  Therefore a cells row index in the matrix is
        // always the same as it's row number in the grid.

        IsLoading = true;
        ViewBuilder.BuildMatrix(nodeID);
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
                    CellType.GroupHeader => new GroupHeaderCell { NodeID = mCell.NodeID, IsExpanded = mCell.IsExpanded, CanToggleExpansion = mCell.CanToggleExapansion, ToggleNodeExpansionCommand = toggleNodeExpansionCommand },
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

    public void DimensionEventCommandHandler(DimensionEventArgs dimensionEvent)
    { 
        if(dimensionEvent == null)
            throw new ArgumentNullException(nameof(dimensionEvent));
        if (dimensionEvent.Action == DimensionAction.NoOp)
            return;

        Dimension dimension = ViewBuilder.Dimensions.First(x => x.DisplayValue == dimensionEvent.DimensionID);
        
        var x = dimensionEvent.Action switch
        {
            DimensionAction.SortAscending => dimension.IsAscending = true,
            DimensionAction.SortDescending => dimension.IsAscending = false,
            DimensionAction.Hide => dimension.IsEnabled = false,
            DimensionAction.UnHide => dimension.IsEnabled = true,
            _ => throw new Exception($"DimensionAction not recognised: {dimensionEvent.Action}"),
        };
        BuildGrid(null);
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
