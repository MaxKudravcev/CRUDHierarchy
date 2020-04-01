using System.ComponentModel;

namespace CRUDHierarchy
{
    /// <summary>
    /// A base view model, that implements INotifyPropertyChanged
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}
