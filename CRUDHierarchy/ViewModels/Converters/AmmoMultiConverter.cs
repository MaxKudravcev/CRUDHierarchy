using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CRUDHierarchy
{
    /// <summary>
    /// A converter that shows only ammo that is corresponding to selected weapon on 'Ammo' ComboBox. Converts instance of 'Ammo' to its name
    /// </summary>
    class AmmoMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0].GetType() == (Type)values[1])
            {
                if (values[2] is ComboBoxItem)
                    (values[2] as ComboBoxItem).Visibility = Visibility.Visible;
                
                return (values[0] as Ammo).GetName();
            }
            else
            {
                if (values[2] is ComboBoxItem)
                    (values[2] as ComboBoxItem).Visibility = Visibility.Collapsed;

                return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

