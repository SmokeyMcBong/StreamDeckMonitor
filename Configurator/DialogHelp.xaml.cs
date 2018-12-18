using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Configurator
{
    public partial class DialogHelp : Window
    {
        public DialogHelp()
        {
            InitializeComponent();
        }

        private void DragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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
            Close();
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
