using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Configurator
{
    public partial class MainWindow : Window
    {
        string currentProfile;

        public MainWindow()
        {
            InitializeComponent();

            //load all values from config file
            currentProfile = SettingsMgr.config.Read("selectedProfile", "Current_Profile");
            SettingsMgr.LoadValues(currentProfile);
            DisplayValues(currentProfile);
        }

        private void DisplayValues(string currentProfile)
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
            FrameTotal.Text = SettingsMgr.framesToProcess.ToString();
            BrightnessSlider.Value = SettingsMgr.displayBrightness;
            Profiles.ItemsSource = SettingsMgr.profileList;
            Profiles.SelectedValue = currentProfile;

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

        void FrameTotalInput(object sender, TextChangedEventArgs e)
        {
            FrameTotal.Text = Regex.Replace(FrameTotal.Text, "[^0-9]+", "");

            if (FrameTotal.Text.StartsWith("0"))
            {
                FrameTotal.Text = resetValue;
            }

            //also limit max enterable value to 600
            if (FrameTotal.Text.Length != 0)
            {
                if (int.Parse(FrameTotal.Text) >= 601)
                {
                    FrameTotal.Text = resetValue.ToString();
                }
            }
        }

        private void Profiles_DropDownClosed(object sender, EventArgs e)
        {
            currentProfile = Profiles.Text;
            SettingsMgr.LoadValues(currentProfile);
            DisplayValues(currentProfile);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int sliderValue = (int)e.NewValue;
            BrightnessPercent.Content = sliderValue + "%";
        }

        private void ClickReload(object sender, RoutedEventArgs e)
        {
            //disable button while reloading values
            ButtonReload.IsEnabled = false;

            //reload stored values from config file
            currentProfile = Profiles.Text;
            SettingsMgr.LoadValues(currentProfile);
            DisplayValues(currentProfile);

            //create a background worker to sleep for 1 second
            var backgroundReload = new BackgroundWorker();
            backgroundReload.DoWork += (s, ea) => Thread.Sleep(TimeSpan.FromSeconds(1));

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

            currentProfile = Profiles.Text;

            if (currentProfile != "")
            {
                SettingsMgr.config.Write("selectedProfile", Profiles.Text, "Current_Profile");
            }

            if (BrightnessPercent != null)
            {
                string formattedValue = new String(BrightnessPercent.Content.ToString().Where(Char.IsDigit).ToArray());
                SettingsMgr.config.Write("displayBrightness", formattedValue, currentProfile);
            }

            if (Header1FS.Text != "")
            {
                SettingsMgr.config.Write("headersFontSize_1", Header1FS.Text, currentProfile);
            }

            if (Header2FS.Text != "")
            {
                SettingsMgr.config.Write("headersFontSize_2", Header2FS.Text, currentProfile);
            }

            if (ValuesFS.Text != "")
            {
                SettingsMgr.config.Write("valuesFontSize", ValuesFS.Text, currentProfile);
            }

            if (AnimFramerate.Text != "")
            {
                SettingsMgr.config.Write("animationFramerate", AnimFramerate.Text, currentProfile);
            }

            if (FrameTotal.Text != "")
            {
                SettingsMgr.config.Write("framesToProcess", FrameTotal.Text, currentProfile);
            }

            if (EnableAnim.IsChecked == true)
            {
                SettingsMgr.config.Write("animationEnabled", "True", currentProfile);
            }
            else
            {
                SettingsMgr.config.Write("animationEnabled", "False", currentProfile);
            }

            if (StaticImages.SelectedValue != null)
            {
                SettingsMgr.config.Write("imageName", StaticImages.SelectedValue.ToString(), currentProfile);
            }

            if (Animations.SelectedValue != null)
            {
                SettingsMgr.config.Write("animName", Animations.SelectedValue.ToString(), currentProfile);
            }

            if (HeadersFontType.SelectedValue != null)
            {
                SettingsMgr.config.Write("headersFontType", HeadersFontType.SelectedValue.ToString(), currentProfile);
            }

            if (ValuesFontType.SelectedValue != null)
            {
                SettingsMgr.config.Write("valuesFontType", ValuesFontType.SelectedValue.ToString(), currentProfile);
            }

            if (HeadersFontColor.SelectedValue != null)
            {
                SettingsMgr.config.Write("headersfontColor", HeadersFontColor.SelectedValue.ToString(), currentProfile);
            }

            if (ValuesFontColor.SelectedValue != null)
            {
                SettingsMgr.config.Write("valuesFontColor", ValuesFontColor.SelectedValue.ToString(), currentProfile);
            }

            if (BackgroundFillColor.SelectedValue != null)
            {
                SettingsMgr.config.Write("backgroundColor", BackgroundFillColor.SelectedValue.ToString(), currentProfile);
            }

            //if StreamDeckMonitor is running then refresh/restart the application to show new changes made
            SettingsMgr.RestartSDM();

            //create a background worker to sleep for 1 second
            var backgroundSave = new BackgroundWorker();
            backgroundSave.DoWork += (s, ea) => Thread.Sleep(TimeSpan.FromSeconds(1));

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

            currentProfile = Profiles.Text;

            //reload stored values from config file
            SettingsMgr.LoadValues(currentProfile);
            DisplayValues(currentProfile);
        }

        //exit application
        private void ClickExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
