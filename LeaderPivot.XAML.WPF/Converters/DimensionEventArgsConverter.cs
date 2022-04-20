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
        if (values[0] == null)
            return new DimensionEventArgs { Action = DimensionAction.NoOp }; // Occurs when there are no hidden dimensions

        // sgw remove this
        if (values[1] == null)
            return new DimensionEventArgs { Action = DimensionAction.NoOp }; // Occurs when there are no hidden dimensions
        // sgw remove this


        Dimension dimension = values[0] as Dimension ?? throw new Exception("Could not bind to Dimension");
        DimensionAction action = (DimensionAction)values[1];
        return new DimensionEventArgs { Action = action, DimensionID = dimension.DisplayValue };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
