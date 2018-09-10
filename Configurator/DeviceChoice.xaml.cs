using SharedManagers;
using System.Windows;

namespace Configurator
{
    public partial class DeviceChoice : Window
    {
        public DeviceChoice()
        {
            InitializeComponent();

            //check for device setting
            string deckDevice = SharedSettings.config.Read("selectedDevice", "StreamDeck_Device");
            if (deckDevice == "1")
            {
                EnableStandard.IsChecked = true;
            }
            else
            {
                EnableMini.IsChecked = true;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (EnableStandard.IsChecked == true)
            {
                SharedSettings.config.Write("selectedDevice", "1", "StreamDeck_Device");
            }

            if (EnableMini.IsChecked == true)
            {
                SharedSettings.config.Write("selectedDevice", "2", "StreamDeck_Device");
            }

            SharedSettings.config.Write("choiceMade", "true", "StreamDeck_Device");

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
