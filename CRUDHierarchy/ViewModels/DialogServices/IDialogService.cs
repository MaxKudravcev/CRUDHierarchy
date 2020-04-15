namespace CRUDHierarchy
{ 
    /// <summary>
    /// Provides basic communication between user and program:
    /// Open/Save dialogs and Messagebox
    /// </summary>
    interface IDialogService
    {
        void ShowMessage(string message);
        string FilePath { get; set; }
        bool OpenFileDialog();
        bool SaveFileDialog();
    }
}
