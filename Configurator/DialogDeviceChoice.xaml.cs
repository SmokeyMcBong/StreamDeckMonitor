using SharedManagers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Configurator
{
    public partial class DialogDeviceChoice : Window
    {
        public DialogDeviceChoice()
        {
            InitializeComponent();
        }

        private void DragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonSelectStandard_MouseEnter(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadow = new DropShadowEffect
            {
                Color = Colors.LightBlue
            };
            ButtonSelectStandard.Effect = dropShadow;
            ButtonSelectStandardImage.Effect = dropShadow;
        }

        private void ButtonSelectStandard_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonSelectStandard.Effect = null;
            ButtonSelectStandardImage.Effect = null;
        }

        private void ButtonSelectStandard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SharedSettings.config.Write("selectedDevice", "1", "StreamDeck_Device");
            SharedSettings.config.Write("choiceMade", "true", "StreamDeck_Device");

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void ButtonSelectMini_MouseEnter(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadow = new DropShadowEffect
            {
                Color = Colors.LightBlue
            };
            ButtonSelectMini.Effect = dropShadow;
            ButtonSelectMiniImage.Effect = dropShadow;
        }

        private void ButtonSelectMini_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonSelectMini.Effect = null;
            ButtonSelectMiniImage.Effect = null;
        }

        private void ButtonSelectMini_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SharedSettings.config.Write("selectedDevice", "2", "StreamDeck_Device");
            SharedSettings.config.Write("choiceMade", "true", "StreamDeck_Device");

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void ButtonExit_MouseEnter(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadow = new DropShadowEffect
            {
                Color = Colors.Red
            };
            ButtonExit.Effect = dropShadow;
        }

        private void ButtonExit_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonExit.Effect = null;
        }

        private void ButtonExit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ButtonExit.Effect = null;
            Application.Current.Shutdown();
        }
    }
}
