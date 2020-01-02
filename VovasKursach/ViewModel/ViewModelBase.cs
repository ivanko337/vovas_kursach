using System.ComponentModel;

namespace VovasKursach.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected ViewModelBase()
        { }

        public virtual void OnProperyChanged(string propName)
        {
            PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
            if (propertyChangedEventHandler != null)
            {
                propertyChangedEventHandler.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
