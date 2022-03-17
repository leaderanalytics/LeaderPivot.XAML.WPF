using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF.Converters;
public class DimensionEventCheckedConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        DimensionButton dimensionButton = values[0] as DimensionButton ?? throw new Exception("Could not bind to Dimension");
        DimensionAction action = (DimensionAction)values[1];
        
        return action == DimensionAction.Hide? false : action == DimensionAction.SortAscending ? dimensionButton.Dimension.IsAscending : ! dimensionButton.Dimension.IsAscending;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
        return new object[] { (bool)value };
    }
}
