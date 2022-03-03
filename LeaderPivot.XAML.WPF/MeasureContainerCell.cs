using LeaderAnalytics.LeaderPivot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LeaderPivot.XAML.WPF;

internal class MeasureContainerCell : Cell
{
    public List<Measure> Measures { get; set; }

    static MeasureContainerCell() => DefaultStyleKeyProperty.OverrideMetadata(typeof(MeasureContainerCell), new FrameworkPropertyMetadata(typeof(MeasureContainerCell)));
}
