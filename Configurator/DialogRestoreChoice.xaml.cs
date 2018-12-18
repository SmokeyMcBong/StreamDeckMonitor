using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Configurator
{
    public partial class DialogRestoreChoice : Window
    {
        public DialogRestoreChoice()
        {
            InitializeComponent();
        }

        private void DragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonLoadDefaults_MouseEnter(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadow = new DropShadowEffect
            {
                Color = Colors.Orange
            };
            ButtonLoadDefaults.Effect = dropShadow;
            ButtonLoadDefaultsImage.Effect = dropShadow;
        }

        private void ButtonLoadDefaults_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonLoadDefaults.Effect = null;
            ButtonLoadDefaultsImage.Effect = null;
        }

        private void ButtonLoadDefaults_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string selectedProfiles = "";
            bool isSelectionMade = false;

            if (ResetClock.IsChecked == true)
            {
                selectedProfiles = selectedProfiles + "  * Clock\n";
                isSelectionMade = true;
            }

            if (ResetProfile1.IsChecked == true)
            {
                selectedProfiles = selectedProfiles + "  * Profile 1\n";
                isSelectionMade = true;
            }

            if (ResetProfile2.IsChecked == true)
            {
                selectedProfiles = selectedProfiles + "  * Profile 2\n";
                isSelectionMade = true;
            }

            if (ResetProfile3.IsChecked == true)
            {
                selectedProfiles = selectedProfiles + "  * Profile 3\n";
                isSelectionMade = true;
            }

            if (ResetAllConfig.IsChecked == true)
            {
                selectedProfiles = selectedProfiles + "  * All Profiles & Settings\n";
                isSelectionMade = true;
            }

            if (isSelectionMade != true)
            {
                MessageBox.Show(" No Profiles/Settings selected");
            }

            else if (isSelectionMade == true)
            {
                MainWindow.DoProfileRestore(selectedProfiles);
                Close();
            }
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
            MainWindow.RestoreCancelExt();
            Close();
        }

        private void ResetAllConfig_Click(object sender, RoutedEventArgs e)
        {
            if (ResetAllConfig.IsChecked == true)
            {
                ResetClock.IsChecked = false;
                ResetProfile1.IsChecked = false;
                ResetProfile2.IsChecked = false;
                ResetProfile3.IsChecked = false;
            }
        }

        private void ResetClock_Click(object sender, RoutedEventArgs e)
        {
            if (ResetClock.IsChecked == true)
            {
                ResetAllConfig.IsChecked = false;
            }
        }

        private void ResetProfile1_Click(object sender, RoutedEventArgs e)
        {
            if (ResetProfile1.IsChecked == true)
            {
                ResetAllConfig.IsChecked = false;
            }
        }

        private void ResetProfile2_Click(object sender, RoutedEventArgs e)
        {
            if (ResetProfile2.IsChecked == true)
            {
                ResetAllConfig.IsChecked = false;
            }
        }

        private void ResetProfile3_Click(object sender, RoutedEventArgs e)
        {
            if (ResetProfile3.IsChecked == true)
            {
                ResetAllConfig.IsChecked = false;
            }
        }
    }
}
