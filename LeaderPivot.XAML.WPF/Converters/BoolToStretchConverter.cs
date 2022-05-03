namespace LeaderAnalytics.LeaderPivot.XAML.WPF.Converters;

internal class BoolToStretchConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((bool)value) ? Stretch.UniformToFill : Stretch.None;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
