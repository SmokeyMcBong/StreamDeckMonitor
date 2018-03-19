using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            //make sure only one instance is running
            SettingsMgr.CheckForTwins();
            InitializeComponent();

            //load all values from config file
            currentProfile = SettingsMgr.config.Read("selectedProfile", "Current_Profile");
            PrepValueDisplay(currentProfile);
        }

        private void PrepValueDisplay(string currentProfile)
        {
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
        void HeaderFontSize1Input(object sender, TextChangedEventArgs e)
        {
            string value = HeaderFontSize1.Text;
            HeaderFontSize1.Text = FormatValue(value, "");
        }

        void HeaderFontSize2Input(object sender, TextChangedEventArgs e)
        {
            string value = HeaderFontSize2.Text;
            HeaderFontSize2.Text = FormatValue(value, "");
        }

        void ValuesFontSizeInput(object sender, TextChangedEventArgs e)
        {
            string value = ValuesFontSize.Text;
            ValuesFontSize.Text = FormatValue(value, "");
        }

        void AnimFramerateInput(object sender, TextChangedEventArgs e)
        {
            string value = AnimFramerate.Text;
            AnimFramerate.Text = FormatValue(value, "FR");
        }

        void FrameTotalInput(object sender, TextChangedEventArgs e)
        {
            string value = FrameTotal.Text;
            FrameTotal.Text = FormatValue(value, "FA");
        }

        private string FormatValue(string valueText, string type)
        {
            string formattedValue = "";

            formattedValue = Regex.Replace(valueText, "[^0-9]+", "");

            if (formattedValue.StartsWith("0"))
            {
                formattedValue = "1";
            }

            if (type != "")
            {
                if (type == "FR")
                {
                    if (int.Parse(formattedValue) > SettingsMgr.frMax)
                    {
                        formattedValue = SettingsMgr.frMax.ToString();
                    }
                }

                if (type == "FA")
                {
                    if (int.Parse(formattedValue) > SettingsMgr.faMax)
                    {
                        formattedValue = SettingsMgr.faMax.ToString();
                    }
                }
            }

            return formattedValue;
        }

        //Up and Down adjustment buttons
        private void ClickH1FSUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(HeaderFontSize1.Text);
            HeaderFontSize1.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickH1FSDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(HeaderFontSize1.Text);
            HeaderFontSize1.Text = ReturnValue(getValue, "FS", "Down").ToString();
        }

        private void ClickH2FSUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(HeaderFontSize2.Text);
            HeaderFontSize2.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickH2FSDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(HeaderFontSize2.Text);
            HeaderFontSize2.Text = ReturnValue(getValue, "FS", "Down").ToString();
        }

        private void ClickVFSUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(ValuesFontSize.Text);
            ValuesFontSize.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickVFSDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(ValuesFontSize.Text);
            ValuesFontSize.Text = ReturnValue(getValue, "FS", "Down").ToString();
        }

        private void ClickFRUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(AnimFramerate.Text);
            AnimFramerate.Text = ReturnValue(getValue, "FR", "Up").ToString();
        }

        private void ClickFRDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(AnimFramerate.Text);
            AnimFramerate.Text = ReturnValue(getValue, "FR", "Down").ToString();
        }

        private void ClickFAUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(FrameTotal.Text);
            FrameTotal.Text = ReturnValue(getValue, "FA", "Up").ToString();
        }

        private void ClickFADown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(FrameTotal.Text);
            FrameTotal.Text = ReturnValue(getValue, "FA", "Down").ToString();
        }

        private int ReturnValue(int value, string type, string direction)
        {
            int adjustedValue = 0;
            int maxValue = 0;

            //set max numerical numbers allowed in each textbox
            if (type == "FS")
            {
                maxValue = SettingsMgr.fsMax;
            }
            if (type == "FR")
            {
                maxValue = SettingsMgr.frMax;
            }
            if (type == "FA")
            {
                maxValue = SettingsMgr.faMax;
            }

            //process values
            if (direction == "Up")
            {
                if (value == maxValue)
                {
                    adjustedValue = maxValue;
                }
                else
                {
                    adjustedValue = value + 1;
                }
            }

            if (direction == "Down")
            {
                if (value == 1)
                {
                    adjustedValue = 1;
                }
                else
                {
                    adjustedValue = value - 1;
                }
            }

            return adjustedValue;
        }

        //profile switcher
        private void Profiles_DropDownClosed(object sender, EventArgs e)
        {
            //save selected profile to config
            currentProfile = Profiles.Text;
            string selectedProfile = "selectedProfile" + " " + currentProfile;

            List<string> configValueList = new List<string>
            {
                selectedProfile
            };

            ThreadMgr.DoSaveInBackground(configValueList, this);

            //display settings based on profile selected
            PrepValueDisplay(currentProfile);

            //display notification
            Brush selectedColor = Brushes.DarkCyan;
            string statusText = "Loaded " + currentProfile;
            ThreadMgr.DoStatusInBackground(selectedColor, statusText, "", this);
        }

        //brightness slider
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int sliderValue = (int)e.NewValue;
            BrightnessPercent.Content = sliderValue + "%";
        }

        private void ClickReload(object sender, RoutedEventArgs e)
        {
            //disable button while reloading values
            ButtonReload.IsEnabled = false;
            ReloadSettings();

            //display notification
            Brush selectedColor = Brushes.SlateBlue;
            string statusText = "Settings Reloaded !";
            ThreadMgr.DoStatusInBackground(selectedColor, statusText, "Reload", this);
        }

        private void ReloadSettings()
        {
            //reload stored values from config file
            currentProfile = Profiles.Text;
            PrepValueDisplay(currentProfile);
        }

        //external call back to ReloadSettings()
        public void ReloadExt()
        {
            ReloadSettings();
        }

        //save settings to config file
        private void ClickSave(object sender, RoutedEventArgs e)
        {
            //disable button while saving values
            ButtonSave.IsEnabled = false;

            //grab all values to save to config file
            string formattedValue = new String(BrightnessPercent.Content.ToString().Where(Char.IsDigit).ToArray());
            string displayBrightness = "displayBrightness" + " " + formattedValue;
            string headerFontSize1 = "headerFontSize_1" + " " + HeaderFontSize1.Text;
            string headerFontSize2 = "headerFontSize_2" + " " + HeaderFontSize2.Text;
            string valuesFontSize = "valuesFontSize" + " " + ValuesFontSize.Text;
            string animationFramerate = "animationFramerate" + " " + AnimFramerate.Text;
            string framesToProcess = "framesToProcess" + " " + FrameTotal.Text;
            string animationEnabled;

            if (EnableAnim.IsChecked == true)
            {
                animationEnabled = "animationEnabled" + " " + "True";
            }
            else
            {
                animationEnabled = "animationEnabled" + " " + "False";
            }

            string imageName = "imageName" + " " + StaticImages.SelectedValue.ToString();
            string animName = "animName" + " " + Animations.SelectedValue.ToString();
            string headerFontType1 = "headerFontType_1" + " " + HeaderFontType1.SelectedValue.ToString();
            string headerFontType2 = "headerFontType_2" + " " + HeaderFontType2.SelectedValue.ToString();
            string valuesFontType = "valuesFontType" + " " + ValuesFontType.SelectedValue.ToString();
            string headerfontColor1 = "headerfontColor_1" + " " + HeaderFontColor1.SelectedValue.ToString();
            string headerfontColor2 = "headerfontColor_2" + " " + HeaderFontColor2.SelectedValue.ToString();
            string valuesFontColor = "valuesFontColor" + " " + ValuesFontColor.SelectedValue.ToString();
            string backgroundColor = "backgroundColor" + " " + BackgroundFillColor.SelectedValue.ToString();

            //store all values to pass to DoSaveInBackground()
            List<string> configValueList = new List<string>
            {
                displayBrightness,
                headerFontSize1,
                headerFontSize2,
                valuesFontSize,
                animationFramerate,
                framesToProcess,
                animationEnabled,
                imageName,
                animName,
                headerFontType1,
                headerFontType2,
                valuesFontType,
                headerfontColor1,
                headerfontColor2,
                valuesFontColor,
                backgroundColor
             };

            //send list to be processed in background thread
            ThreadMgr.DoSaveInBackground(configValueList, this);

            //display notification
            Brush selectedColor = Brushes.Green;
            string statusText = "Settings Saved !";
            ThreadMgr.DoStatusInBackground(selectedColor, statusText, "Save", this);
        }

        //exit application
        private void ClickExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}