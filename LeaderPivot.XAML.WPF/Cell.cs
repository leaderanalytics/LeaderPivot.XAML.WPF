using LeaderAnalytics.LeaderPivot;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LeaderPivot.XAML.WPF;

internal abstract class Cell : ContentControl
{
    public string? DataLabel { get; set; }
    public int RowSpan { get; set; } = 1;
    public int ColSpan { get; set; } = 1;

    static Cell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(Cell), new FrameworkPropertyMetadata(typeof(Cell)));

    public Cell(MatrixCell mCell)
    {
        RowSpan = mCell.RowSpan;
        ColSpan = mCell.ColSpan;
        Content = mCell.Value?.ToString();
    }
}

internal abstract class BaseTotalCell : Cell 
{
    public BaseTotalCell(MatrixCell mCell) : base(mCell)
    {

    }
}


internal class GroupHeaderCell : Cell
{
    public bool IsExpanded { get; set; }
    public bool CanToggleExapansion { get; set; }
    public string? NodeID { get; set; }
    public ICommand ToggleExpansion { get; set; }

    static GroupHeaderCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupHeaderCell), new FrameworkPropertyMetadata(typeof(GroupHeaderCell)));

    public GroupHeaderCell(MatrixCell mCell) : base(mCell) { }

    public async Task HandleToggleExpand()
    {
        //await ToggleExpansion.InvokeAsync(NodeID);
    }
}
internal class GrandTotalHeaderCell : BaseTotalCell 
{
    static GrandTotalHeaderCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(GrandTotalHeaderCell), new FrameworkPropertyMetadata(typeof(GrandTotalHeaderCell)));
    public GrandTotalHeaderCell(MatrixCell mCell) : base(mCell) { } 
}

internal class GrandTotalCell : BaseTotalCell
{
    static GrandTotalCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(GrandTotalCell), new FrameworkPropertyMetadata(typeof(GrandTotalCell)));
    public GrandTotalCell(MatrixCell mCell) : base(mCell) { } 
}

internal class MeasureCell : Cell 
{
    static MeasureCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureCell), new FrameworkPropertyMetadata(typeof(MeasureCell)));
    public MeasureCell(MatrixCell mCell) : base(mCell) { } 
}

internal class MeasureLabelCell : Cell 
{
    static MeasureLabelCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureLabelCell), new FrameworkPropertyMetadata(typeof(MeasureLabelCell)));
    public MeasureLabelCell(MatrixCell mCell) : base(mCell) { } 
}

internal class MeasureTotalLabelCell : BaseTotalCell
{
    static MeasureTotalLabelCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureTotalLabelCell), new FrameworkPropertyMetadata(typeof(MeasureTotalLabelCell)));
    public MeasureTotalLabelCell(MatrixCell mCell) : base(mCell) { } 
}

internal class TotalCell : BaseTotalCell 
{
    static TotalCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TotalCell), new FrameworkPropertyMetadata(typeof(TotalCell)));
    public TotalCell(MatrixCell mCell) : base(mCell) { } 
}

internal class TotalHeaderCell : BaseTotalCell 
{
    static TotalHeaderCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TotalHeaderCell), new FrameworkPropertyMetadata(typeof(TotalHeaderCell)));
    public TotalHeaderCell(MatrixCell mCell) : base(mCell) { } 
}
