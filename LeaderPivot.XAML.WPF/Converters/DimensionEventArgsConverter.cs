using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF.Converters;
public class DimensionEventArgsConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        DimensionButton dimensionButton = values[0] as DimensionButton ?? throw new Exception("Could not bind to Dimension");
        DimensionAction action = (DimensionAction)values[1];
        return new DimensionEventArgs { Action = action, DimensionID = dimensionButton.Dimension.DisplayValue, IsRow = dimensionButton.Dimension.IsRow };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
