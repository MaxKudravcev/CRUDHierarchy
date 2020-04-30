using Microsoft.Win32;
using PluginsSupport;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CRUDHierarchy
{
    /// <summary>
    /// Default Win32 implementation of IDialogService
    /// !This DialogService is bound to available FileServices and Plugins!
    /// </summary>
    class DefaultSerializationDialogService : IDialogService
    {
        public string FilePath { get; set; }
        private string OpenFilters;
        private string SaveFilters;

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = OpenFilters;
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = SaveFilters;
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void GetFilters()
        {
            Type[] plugins = Directory.GetFiles("Plugins/").Select(name => Assembly.LoadFrom(name).GetType(Path.GetFileNameWithoutExtension(name) + ".Archiver", true, true)).ToArray();
            Type[] fileServices = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IFileService).IsAssignableFrom(t) && !t.IsInterface).ToArray();
            string[] formatAttributes = fileServices.Select(fs => ((SerializationFormatAttribute)fs.GetCustomAttribute(typeof(SerializationFormatAttribute))).FilterString).ToArray();
            SaveFilters = string.Join("|", formatAttributes);
            formatAttributes = formatAttributes.Concat(plugins.Select(plugin => ((PluginExtensionAttribute)plugin.GetCustomAttribute(typeof(PluginExtensionAttribute))).FilterString)).ToArray();
            OpenFilters = string.Join("|", formatAttributes);
        }

        public DefaultSerializationDialogService()
        {
            GetFilters();
        }
    }
}
