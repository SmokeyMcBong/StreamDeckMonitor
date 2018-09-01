using SharedManagers;
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
            SharedSettings.CheckForTwins();
            InitializeComponent();

            //load all values from config file
            currentProfile = SharedSettings.config.Read("selectedProfile", "Current_Profile");
            PrepValueDisplay(currentProfile);
        }

        private void PrepValueDisplay(string currentProfile)
        {
            SettingsManagerConfig.LoadValues(currentProfile);
            DisplayValues(currentProfile);
        }

        private void DisplayValues(string currentProfile)
        {
            //display current settings according to Settings.ini values
            HeaderFontSize1.Text = SettingsManagerConfig.headerFontSize1.ToString();
            HeaderFontSize2.Text = SettingsManagerConfig.headerFontSize2.ToString();
            ValuesFontSize.Text = SettingsManagerConfig.valueFontSize.ToString();
            AnimFramerate.Text = SettingsManagerConfig.animFramerate.ToString();
            HeaderFontType1.ItemsSource = SettingsManagerConfig.fontList;
            HeaderFontType1.SelectedValue = SettingsManagerConfig.headerFont1;
            HeaderFontType2.ItemsSource = SettingsManagerConfig.fontList;
            HeaderFontType2.SelectedValue = SettingsManagerConfig.headerFont2;
            HeaderFontColor1.ItemsSource = SettingsManagerConfig.colorList;
            HeaderFontColor1.SelectedValue = SettingsManagerConfig.headerFontColor1;
            HeaderFontColor2.ItemsSource = SettingsManagerConfig.colorList;
            HeaderFontColor2.SelectedValue = SettingsManagerConfig.headerFontColor2;
            ValuesFontType.ItemsSource = SettingsManagerConfig.fontList;
            ValuesFontType.SelectedValue = SettingsManagerConfig.valueFont;
            ValuesFontColor.ItemsSource = SettingsManagerConfig.colorList;
            ValuesFontColor.SelectedValue = SettingsManagerConfig.valuesFontColor;
            BackgroundFillColor.ItemsSource = SettingsManagerConfig.colorList;
            BackgroundFillColor.SelectedValue = SettingsManagerConfig.backgroundFillColor;
            StaticImages.ItemsSource = SettingsManagerConfig.imageList;
            StaticImages.SelectedValue = SettingsManagerConfig.imageName;
            Animations.ItemsSource = SettingsManagerConfig.animList;
            Animations.SelectedValue = SettingsManagerConfig.animName;
            FrameTotal.Text = SettingsManagerConfig.framesToProcess.ToString();
            BrightnessSlider.Value = SettingsManagerConfig.displayBrightness;
            Profiles.ItemsSource = SettingsManagerConfig.profileList;
            Profiles.SelectedValue = currentProfile;
            //clock settings
            TimeFontType.ItemsSource = SettingsManagerConfig.fontList;
            TimeFontType.SelectedValue = SettingsManagerConfig.timeFont;
            DateFontType.ItemsSource = SettingsManagerConfig.fontList;
            DateFontType.SelectedValue = SettingsManagerConfig.dateFont;
            TimeFontSize.Text = SettingsManagerConfig.timeFontSize.ToString();
            DateFontSize.Text = SettingsManagerConfig.dateFontSize.ToString();
            TimeFontColor.ItemsSource = SettingsManagerConfig.colorList;
            TimeFontColor.SelectedValue = SettingsManagerConfig.timeFontColor;
            DateFontColor.ItemsSource = SettingsManagerConfig.colorList;
            DateFontColor.SelectedValue = SettingsManagerConfig.dateFontColor;


            if (SharedSettings.IsCompactView() == "True")
            {
                IsCompact.IsChecked = true;
            }
            else
            {
                IsCompact.IsChecked = false;
            }

            if (SharedSettings.IsDateShown() == "True")
            {
                IsDateShown.IsChecked = true;
            }
            else
            {
                IsDateShown.IsChecked = false;
            }

            //display if animations are enabled
            if (SharedSettings.IsAnimationEnabled(currentProfile) != "True")
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
            if (value == "" || value == " ")
            {
                HeaderFontSize1.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    HeaderFontSize1.Text = FormatValue(value, "");
                }
                else
                {
                    HeaderFontSize1.Text = FormatValue("1", "");
                }
            }
        }

        void HeaderFontSize2Input(object sender, TextChangedEventArgs e)
        {
            string value = HeaderFontSize2.Text;
            if (value == "" || value == " ")
            {
                HeaderFontSize2.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    HeaderFontSize2.Text = FormatValue(value, "");
                }
                else
                {
                    HeaderFontSize2.Text = FormatValue("1", "");
                }
            }
        }

        void ValuesFontSizeInput(object sender, TextChangedEventArgs e)
        {
            string value = ValuesFontSize.Text;
            if (value == "" || value == " ")
            {
                ValuesFontSize.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    ValuesFontSize.Text = FormatValue(value, "");
                }
                else
                {
                    ValuesFontSize.Text = FormatValue("1", "");
                }
            }
        }

        void AnimFramerateInput(object sender, TextChangedEventArgs e)
        {
            string value = AnimFramerate.Text;
            if (value == "" || value == " ")
            {
                AnimFramerate.Text = FormatValue("1", "FR");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    AnimFramerate.Text = FormatValue(value, "FR");
                }
                else
                {
                    AnimFramerate.Text = FormatValue("1", "FR");
                }
            }
        }

        void FrameTotalInput(object sender, TextChangedEventArgs e)
        {
            string value = FrameTotal.Text;
            if (value == "" || value == " ")
            {
                FrameTotal.Text = FormatValue("1", "FA");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    FrameTotal.Text = FormatValue(value, "FA");
                }
                else
                {
                    FrameTotal.Text = FormatValue("1", "FA");
                }
            }
        }

        void HeaderFontSizeClock1Input(object sender, TextChangedEventArgs e)
        {
            string value = TimeFontSize.Text;
            if (value == "" || value == " ")
            {
                TimeFontSize.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    TimeFontSize.Text = FormatValue(value, "");
                }
                else
                {
                    TimeFontSize.Text = FormatValue("1", "");
                }
            }
        }

        void HeaderFontSizeClock2Input(object sender, TextChangedEventArgs e)
        {
            string value = DateFontSize.Text;
            if (value == "" || value == " ")
            {
                DateFontSize.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    DateFontSize.Text = FormatValue(value, "");
                }
                else
                {
                    DateFontSize.Text = FormatValue("1", "");
                }
            }
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
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
                    if (int.Parse(formattedValue) > SettingsManagerConfig.frMax)
                    {
                        formattedValue = SettingsManagerConfig.frMax.ToString();
                    }
                }

                if (type == "FA")
                {
                    if (int.Parse(formattedValue) > SettingsManagerConfig.faMax)
                    {
                        formattedValue = SettingsManagerConfig.faMax.ToString();
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

        //clock settings
        private void ClickTFSUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(TimeFontSize.Text);
            TimeFontSize.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickTFSDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(TimeFontSize.Text);
            TimeFontSize.Text = ReturnValue(getValue, "FS", "Down").ToString();
        }

        private void ClickDFSUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(DateFontSize.Text);
            DateFontSize.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickDFSDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(DateFontSize.Text);
            DateFontSize.Text = ReturnValue(getValue, "FS", "Down").ToString();
        }

        private int ReturnValue(int value, string type, string direction)
        {
            int adjustedValue = 0;
            int maxValue = 0;

            //set max numerical numbers allowed in each textbox
            if (type == "FS")
            {
                maxValue = SettingsManagerConfig.fsMax;
            }
            if (type == "FR")
            {
                maxValue = SettingsManagerConfig.frMax;
            }
            if (type == "FA")
            {
                maxValue = SettingsManagerConfig.faMax;
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

            ThreadManager.DoSaveInBackground(configValueList, "mainconfig", this);

            //display settings based on profile selected
            PrepValueDisplay(currentProfile);

            //display notification
            Brush selectedColor = Brushes.DarkCyan;
            string statusText = "Loaded " + currentProfile;
            ThreadManager.DoStatusInBackground(selectedColor, statusText, "", this);
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
            ThreadManager.DoStatusInBackground(selectedColor, statusText, "Reload", this);
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
            //clock settings
            string timeFont = "timeFontType" + " " + TimeFontType.SelectedValue.ToString();
            string dateFont = "dateFontType" + " " + DateFontType.SelectedValue.ToString();
            string timeFontSize = "timeFontSize" + " " + TimeFontSize.Text;
            string dateFontSize = "dateFontSize" + " " + DateFontSize.Text;
            string timeFontColor = "timeFontColor" + " " + TimeFontColor.SelectedValue.ToString();
            string dateFontColor = "dateFontColor" + " " + DateFontColor.SelectedValue.ToString();
            string isCompact;
            string isDateShown;

            if (IsCompact.IsChecked == true)
            {
                isCompact = "compactView" + " " + "True";
            }
            else
            {
                isCompact = "compactView" + " " + "False";
            }

            if (IsDateShown.IsChecked == true)
            {
                isDateShown = "showDate" + " " + "True";
            }
            else
            {
                isDateShown = "showDate" + " " + "False";
            }

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
                backgroundColor,
             };

            //store all clock values to pass to DoSaveInBackground()
            List<string> clockValueList = new List<string>
            {
                timeFont,
                dateFont,
                timeFontSize,
                dateFontSize,
                timeFontColor,
                dateFontColor,
                isCompact,
                isDateShown
             };

            //send lists to be processed in background threads
            ThreadManager.DoSaveInBackground(configValueList, "mainconfig", this);
            ThreadManager.DoSaveInBackground(clockValueList, "clockconfig", this);

            //display notification
            Brush selectedColor = Brushes.Green;
            string statusText = "Settings Saved !";
            ThreadManager.DoStatusInBackground(selectedColor, statusText, "Save", this);
        }

        //exit application
        private void ClickExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}