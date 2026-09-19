using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.ViewModel
{
    public class StaticInfoTextBox : System.ComponentModel.INotifyPropertyChanged
    {
        private string _systemInfoText = "";
        public string SystemInfoText
        {
            get => _systemInfoText;
            set
            {
                _systemInfoText = value;
                OnPropertyChanged(nameof(SystemInfoText));
            }
        }

        private string _cpuInfoText = "";
        public string CpuInfoText
        {
            get => _cpuInfoText;
            set
            {
                _cpuInfoText = value;
                OnPropertyChanged(nameof(CpuInfoText));
            }
        }

        private string _computerInfoText = "";
        public string ComputerInfoText
        {
            get => _computerInfoText;
            set
            {
                _computerInfoText = value;
                OnPropertyChanged(nameof(ComputerInfoText));
            }
        }

        private string _ramInfoText = "";
        public string RamInfoText
        {
            get => _ramInfoText;
            set
            {
                _ramInfoText = value;
                OnPropertyChanged(nameof(RamInfoText));
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }
}
