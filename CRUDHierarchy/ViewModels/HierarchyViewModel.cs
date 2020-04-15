using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Reflection;
using System.Windows.Input;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;

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
                fileService.Save<CRUD>(dialogService.FilePath, Instances.ToList());
            }
        }

        private void Load()
        {
            if (dialogService.OpenFileDialog() == true)
            {
                Type fileServiceType = fileServices.Single(fs =>
                     ((SerializationFormatAttribute)fs.GetCustomAttribute(typeof(SerializationFormatAttribute))).FilterString.EndsWith(Path.GetExtension(dialogService.FilePath)));
                IFileService fileService = Activator.CreateInstance(fileServiceType) as IFileService;

                //Instances = null;
                selectedInstance = null;
                //todo: Maybe come up with generic solution of getting rid of duplicates between agregated fields and collection elements 
                if (fileService.GetType() == typeof(BinaryFileService))
                    Instances = new ObservableCollection<CRUD>(fileService.Open<CRUD>(dialogService.FilePath));
                else
                {
                    ObservableCollection<CRUD> tmp = new ObservableCollection<CRUD>(fileService.Open<CRUD>(dialogService.FilePath));

                    foreach (CRUD weapon in tmp)
                    {
                        if (typeof(Firearm).IsAssignableFrom(weapon.GetType()))
                        {
                            FieldInfo[] fields = weapon.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                            foreach(FieldInfo field in fields)
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
        }
        #endregion
    }
}
