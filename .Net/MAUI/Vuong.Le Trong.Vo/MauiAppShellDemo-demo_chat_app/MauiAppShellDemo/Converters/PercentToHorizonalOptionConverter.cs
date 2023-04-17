using System.Globalization;

namespace MAUIAppDemo.Converters
{
    public class PercentToHorizonalOptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var percent = (double)value;
            if (percent < 0.4)
            {
                return LayoutOptions.End;
            }
            else if (percent >= 0.4 && percent < 0.6)
            {
                return LayoutOptions.Center;
            }
            else
            {
                return LayoutOptions.Start;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
