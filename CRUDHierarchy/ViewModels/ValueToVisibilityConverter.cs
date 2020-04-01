using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CRUDHierarchy
{
    /// <summary>
    /// Hides empty strings from a ComboBox
    /// </summary>
    class ValueToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;  
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
