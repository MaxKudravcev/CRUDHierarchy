using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace CRUDHierarchy
{
    /// <summary>
    /// Converts an instance to a string with its type and its name
    /// </summary>
    class InstancesToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ClassDescriptionAttribute)value.GetType().GetCustomAttribute(typeof(ClassDescriptionAttribute))).Name + ": " + (value as CRUD).GetName();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
