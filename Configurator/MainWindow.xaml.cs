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
            HeaderFontSize1.Text = SettingsMgr.headerFontSize1.ToString();
            HeaderFontSize2.Text = SettingsMgr.headerFontSize2.ToString();
            ValuesFontSize.Text = SettingsMgr.valueFontSize.ToString();
            AnimFramerate.Text = SettingsMgr.animFramerate.ToString();
            HeaderFontType1.ItemsSource = SettingsMgr.fontList;
            HeaderFontType1.SelectedValue = SettingsMgr.headerFont1;
            HeaderFontType2.ItemsSource = SettingsMgr.fontList;
            HeaderFontType2.SelectedValue = SettingsMgr.headerFont2;
            HeaderFontColor1.ItemsSource = SettingsMgr.colorList;
            HeaderFontColor1.SelectedValue = SettingsMgr.headerFontColor1;
            HeaderFontColor2.ItemsSource = SettingsMgr.colorList;
            HeaderFontColor2.SelectedValue = SettingsMgr.headerFontColor2;
            ValuesFontType.ItemsSource = SettingsMgr.fontList;
            ValuesFontType.SelectedValue = SettingsMgr.valueFont;
            ValuesFontColor.ItemsSource = SettingsMgr.colorList;
            ValuesFontColor.SelectedValue = SettingsMgr.valuesFontColor;
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

        void HeaderFontSize1Input(object sender, TextChangedEventArgs e)
        {
            HeaderFontSize1.Text = Regex.Replace(HeaderFontSize1.Text, "[^0-9]+", "");

            if (HeaderFontSize1.Text.StartsWith("0"))
            {
                HeaderFontSize1.Text = resetValue;
            }
        }

        void HeaderFontSize2Input(object sender, TextChangedEventArgs e)
        {
            HeaderFontSize2.Text = Regex.Replace(HeaderFontSize2.Text, "[^0-9]+", "");

            if (HeaderFontSize2.Text.StartsWith("0"))
            {
                HeaderFontSize2.Text = resetValue;
            }
        }

        void ValuesFontSizeInput(object sender, TextChangedEventArgs e)
        {
            ValuesFontSize.Text = Regex.Replace(ValuesFontSize.Text, "[^0-9]+", "");

            if (ValuesFontSize.Text.StartsWith("0"))
            {
                ValuesFontSize.Text = resetValue;
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

            if (HeaderFontSize1.Text != "")
            {
                SettingsMgr.config.Write("headerFontSize_1", HeaderFontSize1.Text, currentProfile);
            }

            if (HeaderFontSize2.Text != "")
            {
                SettingsMgr.config.Write("headerFontSize_2", HeaderFontSize2.Text, currentProfile);
            }

            if (ValuesFontSize.Text != "")
            {
                SettingsMgr.config.Write("valuesFontSize", ValuesFontSize.Text, currentProfile);
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

            if (HeaderFontType1.SelectedValue != null)
            {
                SettingsMgr.config.Write("headerFontType_1", HeaderFontType1.SelectedValue.ToString(), currentProfile);
            }

            if (HeaderFontType2.SelectedValue != null)
            {
                SettingsMgr.config.Write("headerFontType_2", HeaderFontType2.SelectedValue.ToString(), currentProfile);
            }

            if (ValuesFontType.SelectedValue != null)
            {
                SettingsMgr.config.Write("valuesFontType", ValuesFontType.SelectedValue.ToString(), currentProfile);
            }

            if (HeaderFontColor1.SelectedValue != null)
            {
                SettingsMgr.config.Write("headerfontColor_1", HeaderFontColor1.SelectedValue.ToString(), currentProfile);
            }

            if (HeaderFontColor2.SelectedValue != null)
            {
                SettingsMgr.config.Write("headerfontColor_2", HeaderFontColor2.SelectedValue.ToString(), currentProfile);
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
