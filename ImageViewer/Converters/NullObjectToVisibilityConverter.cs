using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ImageViewer.Converters
{
    public class NullObjectToVisibilityConverter : IValueConverter
    {
        public Visibility NullObjectVisibility { get; set; }

        public Visibility NotNullIbjectVisibility { get; set; }

        public NullObjectToVisibilityConverter()
        {
            NullObjectVisibility = Visibility.Hidden;
            NotNullIbjectVisibility = Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return NullObjectVisibility;
            else
                return NotNullIbjectVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
