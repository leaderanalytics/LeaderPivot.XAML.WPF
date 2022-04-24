using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF.Converters;
public class DimensionEventCheckedConverter : IMultiValueConverter
{
  

    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        Dimension dimension = values[0] as Dimension ?? throw new Exception("Could not bind to Dimension");
        DimensionAction action = (DimensionAction)values[1];
        return action == DimensionAction.Hide? false : action == DimensionAction.SortAscending ? dimension.IsAscending : ! dimension.IsAscending;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
