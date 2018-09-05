using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geocentric
{
    public class MainWindowVM: INotifyPropertyChanged
    {
        public MainWindowVM()
        {

        }
        string textInfo;
        public string TextInfo
        {
            get
            {
                return textInfo;
            }
            private set
            {
                textInfo = value;
                OnPropertyChanged(nameof(TextInfo));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
