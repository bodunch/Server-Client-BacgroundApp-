using AdminPanel.Services.Charts;
using AdminPanel.Services.CPU;
using AdminPanel.Services.Procceses;
using AdminPanel.Services.Procceses.ThreatManager;
using AdminPanel.Services.RAM;
using AdminPanel.ViewModel;
using AdminPanel.ViewModel.SendCommand;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
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
        private readonly HttpClient _httpClient;

        public MainWindow()
        {
            InitializeComponent();

            var cts = new CancellationToken();
            _ = BackgroundCheckerHash.HashCheckers(cts);

            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
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
                    await mvm.ClientInfo(tag, mvm.SelectedMachineName);
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

        private async void SendStandartCommand(object sender, RoutedEventArgs e)
        {
            if(sender is Button button && button.Tag is string dataType)
            {
                if(DataContext is MainViewModel viewModel)
                {
                    await viewModel.DetermineCommand(viewModel.SelectedMachineName, dataType, _httpClient);
                }
                
            }    
        }

        private async void SendAdminCommand(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                string command = TxtCustomCommand.Text.Trim();

                if (string.IsNullOrEmpty(command)) return;

                if(DataContext is MainViewModel viewModel)
                {
                    
                    SendCommandToServer scs = new SendCommandToServer();
                    await scs.SendAdminCustomCommand(viewModel.SelectedMachineName, _httpClient, command);
                    TxtCustomCommand.Clear();
                }
            }
        }
    }
}