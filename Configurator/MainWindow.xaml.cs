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

            //display if animations are enabled
            if (SettingsMgr.isEnabled == 0)
            {
                AnimSwitch.IsChecked = false;
            }
            else
            {
                AnimSwitch.IsChecked = true;
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
            //SetDefaultWidth();

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
            //SetDefaultWidth();

            //disable button while saving values
            ButtonSave.IsEnabled = false;

            if (Header1FS.Text != "")
            {
                SettingsMgr.settingsIni.Write("fontSizeHeader_1", Header1FS.Text, "Font_Sizes");
            }

            if (Header2FS.Text != "")
            {
                SettingsMgr.settingsIni.Write("fontSizeHeader_2", Header2FS.Text, "Font_Sizes");
            }

            if (ValuesFS.Text != "")
            {
                SettingsMgr.settingsIni.Write("fontSizeValues", ValuesFS.Text, "Font_Sizes");
            }

            if (AnimFramerate.Text != "")
            {
                SettingsMgr.settingsIni.Write("animationFramerate", AnimFramerate.Text, "Animated_Keys");
            }

            if (AnimSwitch.IsChecked == true)
            {
                SettingsMgr.settingsIni.Write("animationEnabled", "True", "Animated_Keys");
            }
            else
            {
                SettingsMgr.settingsIni.Write("animationEnabled", "False", "Animated_Keys");
            }

            if (HeadersFontType.SelectedValue != null)
            {
                SettingsMgr.settingsIni.Write("fontType", HeadersFontType.SelectedValue.ToString(), "Font_Headers");
            }

            if (ValuesFontType.SelectedValue != null)
            {
                SettingsMgr.settingsIni.Write("fontType", ValuesFontType.SelectedValue.ToString(), "Font_Values");
            }

            if (HeadersFontColor.SelectedValue != null)
            {
                SettingsMgr.settingsIni.Write("fontColor", HeadersFontColor.SelectedValue.ToString(), "Font_Headers");
            }

            if (ValuesFontColor.SelectedValue != null)
            {
                SettingsMgr.settingsIni.Write("fontColor", ValuesFontColor.SelectedValue.ToString(), "Font_Values");
            }

            if (BackgroundFillColor.SelectedValue != null)
            {
                SettingsMgr.settingsIni.Write("backgroundColor", BackgroundFillColor.SelectedValue.ToString(), "Background_Color");
            }

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

        //define default and extended window sizes
        private int defaultWidth = 475;
        private int extendedWidth = 1022;

        private void ClickPreview(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.Width == defaultWidth)
            {
                SetExtendedWidth();
            }
            else
            {
                SetDefaultWidth();
            }
        }

        private void SetDefaultWidth()
        {
            if (Application.Current.MainWindow.Width == extendedWidth)
            {
                ButtonPreview.Content = "Show Preview";
                Application.Current.MainWindow.Width = defaultWidth;
            }
        }

        private void SetExtendedWidth()
        {
            if (Application.Current.MainWindow.Width == defaultWidth)
            {
                ButtonPreview.Content = " Hide Preview";
                Application.Current.MainWindow.Width = extendedWidth;
            }
        }

        //exit application
        private void ClickExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
