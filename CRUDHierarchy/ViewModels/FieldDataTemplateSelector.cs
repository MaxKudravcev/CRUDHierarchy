using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace CRUDHierarchy
{
    /// <summary>
    /// Selects proper DataTemplate for displaying a field based on its type 
    /// </summary>
    class FieldDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultDataTemplate { get; set; }
        public DataTemplate BooleanDataTemplate { get; set; }
        public DataTemplate EnumDataTemplate { get; set; }
        public DataTemplate AmmoDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FieldInfo field = (item as Field).FieldInfo;
            
            if (field.FieldType.IsEnum)
            {
                return EnumDataTemplate;
            }
            if (field.FieldType == typeof(bool))
            {
                return BooleanDataTemplate;
            }
            if (field.FieldType.IsSubclassOf(typeof(Ammo)))
            {
                return AmmoDataTemplate;
            }
            return DefaultDataTemplate;
        }
    }
}
