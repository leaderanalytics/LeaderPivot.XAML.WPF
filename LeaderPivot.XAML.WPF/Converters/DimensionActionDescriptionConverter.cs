using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF.Converters;
internal class DimensionActionDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DimensionAction action = ((DimensionAction)value);
        FieldInfo fi = action.GetType().GetField(action.ToString());
        return ((DescriptionAttribute)fi.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
