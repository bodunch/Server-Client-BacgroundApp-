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
using AdminPanel.ViewModel;

namespace AdminPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OpenClientInfo(object sender, RoutedEventArgs e)
        {
            if(sender is Button button && button.Tag != null)
            {
                DynamicButtonsPanel.Visibility = Visibility.Visible;

                string tag = button.Tag.ToString()!;

                if (DataContext is MainViewModel mvm)
                {
                    await mvm.ClientInfo(tag);
                }
            }
            
        }
    }
}