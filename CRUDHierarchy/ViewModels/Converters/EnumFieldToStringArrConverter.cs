using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace CRUDHierarchy
{
    /// <summary>
    /// Takes a <Field> object with FieldInfo.FieldType == Enum and converts Enum to array of strings
    /// </summary>
    public class EnumFieldToStringArrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FieldInfo field = (value as Field).FieldInfo;
            return field.FieldType.GetEnumValues();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
