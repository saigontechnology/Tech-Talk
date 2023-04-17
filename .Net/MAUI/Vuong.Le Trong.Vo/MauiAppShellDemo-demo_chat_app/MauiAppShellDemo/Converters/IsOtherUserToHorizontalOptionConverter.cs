using System.Globalization;

namespace MAUIAppDemo.Converters
{
    public class IsOtherUserToHorizontalOptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return LayoutOptions.End;
            }
            return LayoutOptions.Start;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
