using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Recorder.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void _onPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged == null) return;
            foreach (string name in propertyNames)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        protected void _onPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public virtual void Dispose() {}
    }
}
