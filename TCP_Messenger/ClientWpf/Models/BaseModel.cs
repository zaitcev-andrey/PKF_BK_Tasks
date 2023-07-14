using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientWpf.Models
{
    internal class BaseModel : INotifyPropertyChanged
    {
        #region INotifyPropetyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
