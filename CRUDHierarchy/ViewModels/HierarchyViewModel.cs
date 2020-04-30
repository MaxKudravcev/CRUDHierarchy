using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Reflection;
using System.Windows.Input;
using System.Collections.Generic;
using System.IO;
using PluginsSupport;

namespace CRUDHierarchy
{
    /// <summary>
    /// The main ViewModel
    /// </summary>
    class HierarchyViewModel : BaseViewModel
    {
        #region Fields and properties

        //A class, that is selected in the 'Class' ComboBox
        private Type selectedType;

        //An instance that is selected on the TreeView
        private CRUD selectedInstance;

        //A collection of all available (non-abstract) classes in the hierarchy
        public ObservableCollection<Type> Classes { get; set; }

        //A collection of all fields within the selected class
        public ObservableCollection<Field> Fields { get; set; }

        //A collection of all instances, that were created
        public ObservableCollection<CRUD> Instances { get; set; }
        
        //All available FileServices
        private Type[] fileServices;

        //Current DialogService
        private IDialogService dialogService;

        //A collection of plugins, found in 'Plugins' folder
        public ObservableCollection<string> Plugins { get; set; }
        
        //Selected plugin
        public string SelectedPlugin { get; set; }
        #endregion



        #region Commands
        //Commands for creating, updating and deleting instances
        public ICommand CreateCommand { get; set; }

        public ICommand UpdateCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand LoadCommand { get; set; }

        public ICommand SaveCommand { get; set; }
        #endregion



        #region Getters&Setters

        /// <summary>
        /// When set - obtains all fields of the selected type
        /// </summary>
        public Type SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                selectedType = value;    
                if (value != null)
                {
                    Fields = new ObservableCollection<Field>(selectedType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Select(field => new Field(field)));
                }
                SelectedInstance = null;
            }
        }

        /// <summary>
        /// When set - obtains all field values of the selected instance
        /// </summary>
        public CRUD SelectedInstance
        {
            get
            {
                return selectedInstance;
            }
            set
            {
                if (value != selectedInstance)
                {
                    selectedInstance = value;

                    if (value != null)
                        ReadInstance(selectedInstance);
                }
            }
        }

        #endregion



        #region Helper methods

        /// <summary>
        /// Gets all classes that are not-abstract and are inherited from super-class 'CRUD' and updates the 'Classes' Collection
        /// </summary>
        private void GetClasses()
        {
            //Get all types in the same assembly that "CRUD" is and select those, that inherit from it and aren't abstract
            this.Classes = new ObservableCollection<Type>(typeof(CRUD).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(CRUD)) && !type.IsAbstract));
        }

        /// <summary>
        /// Gets values of all fields of the selected instance and writes them into Fields[i].Value
        /// </summary>
        /// <param name="instance">The instance, which field values will be obtained</param>
        private void ReadInstance(CRUD instance)
        {
            object[] fieldValues = instance.Read();

            for (int i = 0; i < Fields.Count; i++)
            {
                Fields[i].Value = fieldValues[i];
            }
        }

        /// <summary>
        /// Updates SelectedInstance and SelectedType properties based on the item, that is selected on the ListView
        /// </summary>
        /// <param name="value">Instance, that is selected on the ListView</param>
        public void TreeViewUpdateProperties(object value)
        {
            if (value != null)
            {
                SelectedType = null;
                SelectedType = (value as CRUD).GetType();
                SelectedInstance = value as CRUD;
            }
        }

        /// <summary>
        /// Finds correct plugin based on file extension
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>An instance of used archivator (if such plugin exists)</returns>
        private object GetPluginByPath(string path)
        {
            List<Type> pluginTypes = new List<Type>();
            foreach (string plugin in Plugins)
            {
                if (plugin != "-NONE-")
                    pluginTypes.Add(Assembly.LoadFrom("Plugins/" + plugin + ".dll").GetType(plugin + ".Archiver", true, true));
            }
            List<object> objects = new List<object>();
            foreach (Type plugin in pluginTypes)
            {
                objects.Add(Activator.CreateInstance(plugin));
            }

            for (int i = 0; i < objects.Count; i++)            
                if (dialogService.FilePath.EndsWith((string)pluginTypes[i].InvokeMember("extension", BindingFlags.GetField, null, objects[i], null)))                
                    return objects[i];
            return null;        
        }

        #endregion



        #region Methods for commands

        /// <summary>
        /// Check if all the fields on the view are correctly filled
        /// </summary>
        /// <returns>Can an instance be created</returns>
        private bool AreFieldsFilled()
        {
            if (!Fields.All(field => field.Value != null) || Fields.Count == 0)
                return false;
            else
            {
                if (Fields.All(field => field.Value != null))
                    return Fields.All(field => !string.IsNullOrWhiteSpace(field.Value.ToString()));
                else return false;
            }
        }

        /// <summary>
        /// Check if the instance is selected and all fields are filled 
        /// </summary>
        /// <returns>Can the selected instance be updated</returns>
        private bool CanUpdate()
        {
            return AreFieldsFilled() && (SelectedInstance != null);
        }

        /// <summary>
        /// Check if the instance is selected
        /// </summary>
        /// <returns>Can the instance be deleted</returns>
        private bool CanDelete()
        {
            return SelectedInstance != null;
        }

        /// <summary>
        /// Check if there are some instances that can be saved
        /// </summary>
        /// <returns></returns>
        private bool CanSave()
        {
            return Instances.Count > 0;
        }

        /// <summary>
        /// Create an instance of the selected type and pass a list of arguments (got from View) to a Create metod
        /// </summary>
        private void Create()
        {
            List<object> args = new List<object>(Fields.Select(field => field.Value));
            Instances.Add((CRUD)(Activator.CreateInstance(SelectedType)));
            Instances.Last().Create(args);
        }

        /// <summary>
        /// Updates field values of the selected instance with values got from View
        /// </summary>
        private void Update()
        {
            List<object> args = new List<object>(Fields.Select(field => field.Value));
            SelectedInstance.Create(args);
            var tmp = Instances;
            Instances = null;
            Instances = tmp;
            TreeViewUpdateProperties(SelectedInstance);
        }

        /// <summary>
        /// Delete the selected instance from 'Instances' Collection and from all instances, that contain it
        /// </summary>
        private void Delete()
        {
            Instances.Remove(SelectedInstance);
            foreach (CRUD instance in Instances)
            {
                FieldInfo[] fields = instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (field.GetValue(instance) == SelectedInstance)
                    {
                        field.SetValue(instance, null);
                        break;
                    }
                }
            }
            SelectedInstance = null;
        }

        /// <summary>
        /// Serialize the Instances collection to a choosen format
        /// </summary>
        private void Save()
        {
            if (dialogService.SaveFileDialog() == true)
            {
                Type fileServiceType = fileServices.Single(fs => 
                     ((SerializationFormatAttribute)fs.GetCustomAttribute(typeof(SerializationFormatAttribute))).FilterString.EndsWith(Path.GetExtension(dialogService.FilePath)));
                IFileService fileService = Activator.CreateInstance(fileServiceType) as IFileService;
                using (MemoryStream stream = new MemoryStream())
                {                                        
                    fileService.Save<CRUD>(stream, Instances.ToList());
                    stream.Position = 0;
                    string tmp = dialogService.FilePath;

                    if (SelectedPlugin != "-NONE-")
                    {
                        Assembly plugin;
                        try
                        {
                             plugin = Assembly.LoadFrom("Plugins/" + SelectedPlugin + ".dll");
                        }
                        catch (Exception)
                        {
                            throw new Exception("ERROR: Plugin not found.");
                        }

                        Type t = plugin.GetType(SelectedPlugin + ".Archiver", true, true);
                        IPlugin obj = (IPlugin)Activator.CreateInstance(t);
                        tmp += ((PluginExtensionAttribute)t.GetCustomAttribute(typeof(PluginExtensionAttribute))).FilterString.Split('*').Last();

                        using (FileStream fs = new FileStream(tmp, FileMode.Create))
                        {
                            obj.Compress(stream, fs);
                        }
                    }
                    else
                    {
                        using (FileStream fs = new FileStream(tmp, FileMode.Create))
                        {
                            stream.CopyTo(fs);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Deserialize selected file to a collection of CRUD
        /// </summary>
        private void Load()
        {
            if (dialogService.OpenFileDialog() == true)
            {
                ObservableCollection<CRUD> tmp;
                using (MemoryStream stream = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(dialogService.FilePath, FileMode.Open))
                    {
                        try
                        {
                            Type pluginType = Directory.GetFiles("Plugins/").Select(name =>
                                              Assembly.LoadFrom(name).GetType(Path.GetFileNameWithoutExtension(name) + ".Archiver", true, true)).Single(archiver =>
                                              ((PluginExtensionAttribute)archiver.GetCustomAttribute(typeof(PluginExtensionAttribute))).FilterString.EndsWith(Path.GetExtension(dialogService.FilePath)));
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginType);
                            plugin.Decompress(fs, stream);
                        }
                        catch (Exception)
                        {
                            fs.CopyTo(stream);
                        }
                    }
                    stream.Position = 0;
                    string extension = dialogService.FilePath.Split('.')[1];

                    Type fileServiceType;
                    try
                    {
                        fileServiceType = fileServices.Single(fs =>
                         ((SerializationFormatAttribute)fs.GetCustomAttribute(typeof(SerializationFormatAttribute))).FilterString.EndsWith(extension));
                    }
                    catch (Exception)
                    {
                        throw new Exception("ERROR: Invalid file format");
                    }
                    IFileService fileService = Activator.CreateInstance(fileServiceType) as IFileService;

                    //Instances = null;
                    selectedInstance = null;
                    tmp = new ObservableCollection<CRUD>(fileService.Open<CRUD>(stream));
                    
                }
                //todo: Maybe come up with generic solution of getting rid of duplicates between agregated fields and collection elements        
                foreach (CRUD weapon in tmp)
                {
                    if (typeof(Firearm).IsAssignableFrom(weapon.GetType()))
                    {
                        FieldInfo[] fields = weapon.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                        foreach (FieldInfo field in fields)
                        {
                            if (typeof(Ammo).IsAssignableFrom(field.FieldType))
                            {
                                field.SetValue(weapon, tmp.Single(crud => crud.GetName() == ((CRUD)field.GetValue(weapon)).GetName()));
                            }
                        }
                    }
                }
                Instances = tmp;
            }
  
        }
        
        
        #endregion



        #region Constructor
        public HierarchyViewModel(IDialogService dialogService)
        {
            GetClasses();
            Fields = new ObservableCollection<Field>();
            Instances = new ObservableCollection<CRUD>();
            CreateCommand = new RelayCommand<object>(obj => Create(), obj => AreFieldsFilled());
            UpdateCommand = new RelayCommand<object>(obj => Update(), obj => CanUpdate());
            DeleteCommand = new RelayCommand<object>(obj => Delete(), obj => CanDelete());
            SaveCommand = new RelayCommand<object>(obj => Save(), obj => CanSave());
            LoadCommand = new RelayCommand<object>(obj => Load());

            this.dialogService = dialogService;
            fileServices = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IFileService).IsAssignableFrom(t) && !t.IsInterface).ToArray();

            Plugins = new ObservableCollection<string>(Directory.GetFiles("Plugins/").Select(str => Path.GetFileNameWithoutExtension(str)));
            Plugins.Add("-NONE-");
            SelectedPlugin = "-NONE-";
        }
        #endregion
    }
}
