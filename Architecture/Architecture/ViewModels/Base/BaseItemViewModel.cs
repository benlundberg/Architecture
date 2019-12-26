using System.ComponentModel;

namespace Architecture
{
    public class BaseItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
