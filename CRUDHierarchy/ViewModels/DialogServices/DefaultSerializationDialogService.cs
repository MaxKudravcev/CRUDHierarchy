using Microsoft.Win32;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CRUDHierarchy
{
    /// <summary>
    /// Default Win32 implementation of IDialogService
    /// !This DialogService is bound to available FileServices!
    /// </summary>
    class DefaultSerializationDialogService : IDialogService
    {
        public string FilePath { get; set; }
        private string filters;

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filters;
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
            saveFileDialog.Filter = filters;
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
            Type[] fileServices = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(IFileService))).ToArray();
            string[] formatAttributes = fileServices.Select(fs => ((SerializationFormatAttribute)fs.GetCustomAttribute(typeof(SerializationFormatAttribute))).FilterString).ToArray();
            filters = string.Join("|", formatAttributes);
        }

        public DefaultSerializationDialogService()
        {
            GetFilters();
        }
    }
}
