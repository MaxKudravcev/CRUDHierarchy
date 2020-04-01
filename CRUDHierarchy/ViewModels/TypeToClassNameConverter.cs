using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace CRUDHierarchy
{
    /// <summary>
    /// A converter that takes a <Type> and returns its name from its ClassDescription attribute
    /// </summary>
    public class TypeToClassNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value as Type;

            if (type == null)
                return null;

            return ((ClassDescriptionAttribute)type.GetCustomAttribute(typeof(ClassDescriptionAttribute))).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
