namespace LeaderAnalytics.LeaderPivot.XAML.WPF.Converters;

public class DimensionEventEnabledConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        DimensionAction action = (DimensionAction)values[0];

        if (action != DimensionAction.Hide)
            return true;

        // Do not allow user to hide a Dimension if that dimension is the only one for the axis:

        DimensionContainerCell cell = values[1] as DimensionContainerCell ?? throw new Exception("Could not bind to DimensionContainerCell");
        return cell.Dimensions.Count > 1;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
