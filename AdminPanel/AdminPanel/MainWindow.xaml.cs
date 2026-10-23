using AdminPanel.Services.Charts;
using AdminPanel.Services.CPU;
using AdminPanel.Services.Procceses;
using AdminPanel.Services.Procceses.ThreatManager;
using AdminPanel.Services.RAM;
using AdminPanel.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var cts = new CancellationToken();
            _ = BackgroundCheckerHash.HashCheckers(cts);
        }

        private async void OpenClientInfo(object sender, RoutedEventArgs e)
        {
            if(sender is Button button && button.Tag != null)
            {
                AnalizingCpuActivity.ResetHistory();
                AnalizingRamActivity.ResetHistory();

                DynamicButtonsPanel.Visibility = Visibility.Visible;

                string tag = button.Tag.ToString()!;

                if (DataContext is MainViewModel mvm)
                {
                    await mvm.ClientInfo(tag);
                }
            }  
        }
        
        private async void OpenDynamicClientInfo(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string dataType)
            {
                OnDataTypeSelected(dataType);
            }
        }

        private async void OnDataTypeSelected(string dataType)
        {
            if (DataContext is MainViewModel viewModel && !string.IsNullOrEmpty(viewModel.SelectedClientId))
            {
                await viewModel.DetermineDataType(viewModel.SelectedClientId, dataType, dynamicInfoTextBox, cartesianChart);
            }
        }
    }
}