/* 
 * Copyright 2021 Leader Analytics 
 * LeaderAnalytics.com
 * SamWheat.com
 * 
 * Please do not remove this header.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using LeaderAnalytics.LeaderPivot;
using MaterialDesignThemes.Wpf;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

public class LeaderPivotControl: ContentControl, IDropTarget, IDragSource
{
    #region Properties
    public PivotViewBuilder ViewBuilder
    {
        get { return (PivotViewBuilder)GetValue(ViewBuilderProperty); }
        set { SetValue(ViewBuilderProperty, value); }
    }

    public static readonly DependencyProperty ViewBuilderProperty =
        DependencyProperty.Register("ViewBuilder", typeof(PivotViewBuilder), typeof(LeaderPivotControl), new PropertyMetadata(null, async (s,e) => await ((LeaderPivotControl)s).BuildGrid(null)));


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

    // 
    //public bool UseResponsiveSizing
    //{
    //    get { return (bool)GetValue(UseResponsiveSizingProperty); }
    //    set { SetValue(UseResponsiveSizingProperty, value); }
    //}
    
    //public static readonly DependencyProperty UseResponsiveSizingProperty =
    //    DependencyProperty.Register("UseResponsiveSizing", typeof(bool), typeof(LeaderPivotControl), new PropertyMetadata(false));


    public int CellPadding
    {
        get { return (int)GetValue(CellPaddingProperty); }
        set { SetValue(CellPaddingProperty, value); }
    }

    public static readonly DependencyProperty CellPaddingProperty =
        DependencyProperty.Register("CellPadding", typeof(int), typeof(LeaderPivotControl), new PropertyMetadata(4));


    public bool IsBusy
    {
        get { return (bool)GetValue(IsBusyProperty); }
        set { SetValue(IsBusyProperty, value); }
    }

    public static readonly DependencyProperty IsBusyProperty =
        DependencyProperty.Register("IsBusy", typeof(bool), typeof(LeaderPivotControl), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true});

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
        ReloadDataCommand = new AsyncRelayCommand(() => BuildGrid(null));
        DimensionEventCommand = new AsyncRelayCommand<DimensionEventArgs>(DimensionEventCommandHandler);
        toggleNodeExpansionCommand = new AsyncRelayCommand<string>(x => BuildGrid(x));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        grid = (Grid) Template.FindName("PART_Grid", this);
    }
    
    public async Task BuildGrid(string? nodeID)
    {
        // Row span takes precidence over column span.
        // If a cell spans multiple rows, cells in the second and subsequent rows are pushed to the right - not down.
        // A cell is never pushed down to a lower row - it is pushed to the right.  Therefore a cells row index in the matrix is
        // always the same as it's row number in the grid.
        IsBusy = true;
        await ViewBuilder.BuildMatrix(nodeID);
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

                BaseCell cell = mCell.CellType switch
                {
                    CellType.Measure => new MeasureCell(),
                    CellType.Total => new TotalCell(),
                    CellType.GrandTotal => new GrandTotalCell(),
                    CellType.GroupHeader => new GroupHeaderCell { NodeID = mCell.NodeID, IsExpanded = mCell.IsExpanded, CanToggleExpansion = mCell.CanToggleExapansion, ToggleNodeExpansionCommand = toggleNodeExpansionCommand },
                    CellType.TotalHeader => new TotalHeaderCell(),
                    CellType.GrandTotalHeader => new GrandTotalHeaderCell(),
                    CellType.MeasureTotalLabel => new MeasureTotalLabelCell(),
                    CellType.MeasureLabel when i == 0 && j == 0 => new MeasureContainerCell() ,
                    CellType.MeasureLabel when i == 0 && j == 1 => new DimensionContainerCell { Dimensions = ViewBuilder.ColumnDimensions, IsRows = false },
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
        IsBusy = false;
    }

    public async Task DimensionEventCommandHandler(DimensionEventArgs dimensionEvent)
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
        await BuildGrid(null);
    }

    private int IncrementCol(int rowIndex, int colIndex, BaseCell cell)
    {
        while (table[rowIndex, colIndex] == 1)
            colIndex++;

        for (int r = rowIndex; r < rowIndex + cell.RowSpan; r++)
            for (int c = colIndex; c < colIndex + cell.ColSpan; c++)
                table[r, c] = 1;

        return colIndex;
    }

    #region IDropTarget
    void IDropTarget.DragOver(IDropInfo dropInfo)
    {
        Dimension sourceItem = dropInfo.Data as Dimension;
        Dimension targetItem = dropInfo.TargetItem as Dimension;

        if (sourceItem != null && targetItem != null)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = DragDropEffects.Move;
        }
    }

    async void IDropTarget.Drop(IDropInfo dropInfo)
    {
        int insertIndex = dropInfo.UnfilteredInsertIndex;
        var sourceList = dropInfo.DragInfo.SourceCollection.TryGetList();
        var targetList = dropInfo.TargetCollection.TryGetList();
        Dimension sourceItem = (Dimension)dropInfo.Data;
        Dimension targetItem = (Dimension)dropInfo.TargetItem;

        if (sourceList != targetList)
        {
            sourceItem.IsRow = !sourceItem.IsRow;

            if (targetList.Count == 1 && sourceList.Count == 1)
            {
                // Swap dimensions across axis...
                Dimension otherDimension = targetList.Cast<Dimension>().First(x => x != sourceItem);
                targetList.Remove(otherDimension);
                otherDimension.IsRow = !otherDimension.IsRow;
                sourceList.Add(otherDimension);
            }
        }
        sourceItem.Sequence = insertIndex;
        IList<Dimension> dimensions = sourceItem.IsRow ? ViewBuilder.RowDimensions : ViewBuilder.ColumnDimensions;
        
        foreach (Dimension d in dimensions.Where(x => x != sourceItem && x.Sequence >= sourceItem.Sequence))
            d.Sequence++;

        await BuildGrid(null);
        
    }
    #endregion

    #region IDragSource implementation
    public void StartDrag(IDragInfo dragInfo)
    {
        dragInfo.Data = dragInfo.SourceItem;
        dragInfo.Effects = dragInfo.Data != null ? DragDropEffects.Copy | DragDropEffects.Move : DragDropEffects.None;
    }

    public bool CanStartDrag(IDragInfo dragInfo)
    {
        var sourceList = dragInfo.SourceCollection?.TryGetList();
        Dimension sourceItem = (Dimension)dragInfo.SourceItem;
        IList<Dimension> dimensions = sourceItem.IsRow ? ViewBuilder.RowDimensions : ViewBuilder.ColumnDimensions;
        
        // If source dimensions has exactly one dimension, count dimensions on cross axis.
        // If cross axis has exactly one dimension allow the drag and swap axis for each 
        // dimension on drop. 
        // Otherwise do not allow the drag if source dimensions has only one dimension.

        if (dimensions.Count == 1)
        {
            IList<Dimension> crossAxisDimensions = sourceItem.IsRow ? ViewBuilder.ColumnDimensions : ViewBuilder.RowDimensions;

            if (crossAxisDimensions.Count > 1)
                return false;
        }
        return true;
    }

    public void Dropped(IDropInfo dropInfo)
    {
    }

    public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
    {
    }

    public void DragCancelled()
    {
    }

    public bool TryCatchOccurredException(Exception exception)
    {
        return false;
    }
    #endregion
}

