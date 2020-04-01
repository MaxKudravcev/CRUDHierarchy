using System;
using System.Globalization;
using System.Windows.Data;

namespace CRUDHierarchy
{
    /// <summary>
    /// Converts an object (which is a Field.Value) to boolean and backwards
    /// </summary>
    class ObjectToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) 
                return false;
            else return (bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as object;
        }
    }
}
