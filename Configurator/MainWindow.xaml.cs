using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Configurator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //load all values from config file
            SettingsMgr.LoadValues();

            //display all stored values
            DisplayValues();
        }

        private void DisplayValues()
        {
            //display current settings according to Settings.ini values
            Header1FS.Text = SettingsMgr.header1FontSize.ToString();
            Header2FS.Text = SettingsMgr.header2FontSize.ToString();
            ValuesFS.Text = SettingsMgr.valueFontSize.ToString();
            AnimFramerate.Text = SettingsMgr.animFramerate.ToString();
            HeadersFontType.ItemsSource = SettingsMgr.fontList;
            HeadersFontType.SelectedValue = SettingsMgr.headerFont;
            ValuesFontType.ItemsSource = SettingsMgr.fontList;
            ValuesFontType.SelectedValue = SettingsMgr.valueFont;
            HeadersFontColor.ItemsSource = SettingsMgr.colorList;
            HeadersFontColor.SelectedValue = SettingsMgr.fontColorHeaders;
            ValuesFontColor.ItemsSource = SettingsMgr.colorList;
            ValuesFontColor.SelectedValue = SettingsMgr.fontColorValues;
            BackgroundFillColor.ItemsSource = SettingsMgr.colorList;
            BackgroundFillColor.SelectedValue = SettingsMgr.backgroundFillColor;
            StaticImages.ItemsSource = SettingsMgr.imageList;
            StaticImages.SelectedValue = SettingsMgr.imageName;
            Animations.ItemsSource = SettingsMgr.animList;
            Animations.SelectedValue = SettingsMgr.animName;

            //display if animations are enabled
            if (SettingsMgr.isEnabled == 0)
            {
                EnableStatic.IsChecked = true;
            }
            else
            {
                EnableAnim.IsChecked = true;
            }
        }

        //format text inputs on the fly to make sure only numerical values are entered
        private string resetValue = "";

        void Header1FSInput(object sender, TextChangedEventArgs e)
        {
            Header1FS.Text = Regex.Replace(Header1FS.Text, "[^0-9]+", "");

            if (Header1FS.Text.StartsWith("0"))
            {
                Header1FS.Text = resetValue;
            }
        }

        void Header2FSInput(object sender, TextChangedEventArgs e)
        {
            Header2FS.Text = Regex.Replace(Header2FS.Text, "[^0-9]+", "");

            if (Header2FS.Text.StartsWith("0"))
            {
                Header2FS.Text = resetValue;
            }
        }

        void ValuesFSInput(object sender, TextChangedEventArgs e)
        {
            ValuesFS.Text = Regex.Replace(ValuesFS.Text, "[^0-9]+", "");

            if (ValuesFS.Text.StartsWith("0"))
            {
                ValuesFS.Text = resetValue;
            }
        }

        //format text inputs on the fly to make sure only numerical values are entered
        void AnimFramerateInput(object sender, TextChangedEventArgs e)
        {
            AnimFramerate.Text = Regex.Replace(AnimFramerate.Text, "[^0-9]+", "");

            if (AnimFramerate.Text.StartsWith("0"))
            {
                AnimFramerate.Text = resetValue;
            }

            //also limit max enterable value to 100
            if (AnimFramerate.Text.Length != 0)
            {
                if (int.Parse(AnimFramerate.Text) >= 101)
                {
                    AnimFramerate.Text = resetValue.ToString();
                }
            }
        }

        private void ClickReload(object sender, RoutedEventArgs e)
        {
            //disable button while reloading values
            ButtonReload.IsEnabled = false;

            //reload stored values from config file
            SettingsMgr.LoadValues();

            //display reloaded config values
            DisplayValues();

            //create a background worker to sleep for 2 seconds
            var backgroundReload = new BackgroundWorker();
            backgroundReload.DoWork += (s, ea) => Thread.Sleep(TimeSpan.FromSeconds(2));

            //define work to be done
            backgroundReload.RunWorkerCompleted += (s, ea) =>
            {
                StatusLabel.Content = "";
                ButtonReload.IsEnabled = true;
            };

            //display notification
            StatusLabel.Foreground = Brushes.Blue;
            StatusLabel.Content = "Settings Reloaded !";

            //start the background worker to reset both the label and button to default states
            backgroundReload.RunWorkerAsync();
        }

        //save settings to ini file
        private void ClickSave(object sender, RoutedEventArgs e)
        {
            //disable button while saving values
            ButtonSave.IsEnabled = false;

            if (Header1FS.Text != "")
            {
                SettingsMgr.config.Write("fontSizeHeader_1", Header1FS.Text, "Font_Sizes");
            }

            if (Header2FS.Text != "")
            {
                SettingsMgr.config.Write("fontSizeHeader_2", Header2FS.Text, "Font_Sizes");
            }

            if (ValuesFS.Text != "")
            {
                SettingsMgr.config.Write("fontSizeValues", ValuesFS.Text, "Font_Sizes");
            }

            if (AnimFramerate.Text != "")
            {
                SettingsMgr.config.Write("animationFramerate", AnimFramerate.Text, "Animated_Keys");
            }

            if (EnableAnim.IsChecked == true)
            {
                SettingsMgr.config.Write("animationEnabled", "True", "Animated_Keys");
            }
            else
            {
                SettingsMgr.config.Write("animationEnabled", "False", "Animated_Keys");

            }
            if (StaticImages.SelectedValue != null)
            {
                SettingsMgr.config.Write("imageName", StaticImages.SelectedValue.ToString(), "Selected_Image");
            }

            if (Animations.SelectedValue != null)
            {
                SettingsMgr.config.Write("animName", Animations.SelectedValue.ToString(), "Selected_Animation");
            }

            if (HeadersFontType.SelectedValue != null)
            {
                SettingsMgr.config.Write("fontType", HeadersFontType.SelectedValue.ToString(), "Font_Headers");
            }

            if (ValuesFontType.SelectedValue != null)
            {
                SettingsMgr.config.Write("fontType", ValuesFontType.SelectedValue.ToString(), "Font_Values");
            }

            if (HeadersFontColor.SelectedValue != null)
            {
                SettingsMgr.config.Write("fontColor", HeadersFontColor.SelectedValue.ToString(), "Font_Headers");
            }

            if (ValuesFontColor.SelectedValue != null)
            {
                SettingsMgr.config.Write("fontColor", ValuesFontColor.SelectedValue.ToString(), "Font_Values");
            }

            if (BackgroundFillColor.SelectedValue != null)
            {
                SettingsMgr.config.Write("backgroundColor", BackgroundFillColor.SelectedValue.ToString(), "Background_Color");
            }

            //if StreamDeckMonitor is running then refresh/restart the application to show new changes made
            SettingsMgr.RestartSDM();

            //create a background worker to sleep for 2 seconds
            var backgroundSave = new BackgroundWorker();
            backgroundSave.DoWork += (s, ea) => Thread.Sleep(TimeSpan.FromSeconds(2));

            //define work to be done
            backgroundSave.RunWorkerCompleted += (s, ea) =>
            {
                StatusLabel.Content = "";
                ButtonSave.IsEnabled = true;
            };

            //display notification
            StatusLabel.Foreground = Brushes.Green;
            StatusLabel.Content = "Settings Saved !";

            //start the background worker to reset both the label and button to default states
            backgroundSave.RunWorkerAsync();
        }

        //exit application
        private void ClickExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
