using SharedManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Configurator
{
    public partial class MainWindow : Window
    {
        string currentProfile;

        public MainWindow()
        {
            //show Stream Deck Device choice window
            string choiceMade = SharedSettings.config.Read("choiceMade", "StreamDeck_Device");
            if (choiceMade == "false")
            {
                DialogDeviceChoice deviceChoice = new DialogDeviceChoice();
                deviceChoice.Show();
                Close();
            }

            //make sure only one instance is running
            SharedSettings.CheckForTwins();
            InitializeComponent();

            //load all values from config file
            currentProfile = SharedSettings.config.Read("selectedProfile", "Current_Profile");
            PrepValueDisplay(currentProfile);
        }

        private void PrepValueDisplay(string currentProfile)
        {
            SettingsConfigurator.LoadValues(currentProfile);
            DisplayValues(currentProfile);
        }

        private void DisplayValues(string currentProfile)
        {
            string deckDevice = SharedSettings.config.Read("selectedDevice", "StreamDeck_Device");

            if (deckDevice == "1")
            {
                SelectDevice15Button.Visibility = Visibility.Visible;
                StaticImages.ItemsSource = SettingsConfigurator.imageList;
                StaticImages.SelectedValue = SettingsConfigurator.imageName;
                Animations.ItemsSource = SettingsConfigurator.animList;
                Animations.SelectedValue = SettingsConfigurator.animName;
                FrameTotal.Text = SettingsConfigurator.framesToProcess.ToString();
                AnimFramerate.Text = SettingsConfigurator.animFramerate.ToString();

                //display is compact view is enabled
                if (SharedSettings.IsCompactView() == "True")
                {
                    IsCompact.IsChecked = true;
                }
                else
                {
                    IsCompact.IsChecked = false;
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

            if (deckDevice == "2")
            {
                SelectDevice6Button.Visibility = Visibility.Visible;
                EnableStatic.Visibility = Visibility.Collapsed;
                EnableAnim.Visibility = Visibility.Collapsed;
                StaticImages.Visibility = Visibility.Collapsed;
                StaticImages.ItemsSource = null;
                Animations.Visibility = Visibility.Collapsed;
                Animations.ItemsSource = null;
                AnimFramerate.Visibility = Visibility.Collapsed;
                AnimFramerate.Text = null;
                FrameTotal.Visibility = Visibility.Collapsed;
                FrameTotal.Text = null;
                ButtonFRUp.Visibility = Visibility.Collapsed;
                ButtonFRDown.Visibility = Visibility.Collapsed;
                ButtonFAUp.Visibility = Visibility.Collapsed;
                ButtonFADown.Visibility = Visibility.Collapsed;
                IsCompact.IsChecked = true;
                IsCompact.IsEnabled = false;
                IsMiniFps.Visibility = Visibility.Visible;
                SurroundingImageSperator.Visibility = Visibility.Collapsed;
                SurroundingImageLabel.Content = "Additional Options";

                if (SharedSettings.IsFpsCounter() == "True")
                {
                    IsMiniFps.IsChecked = true;
                }
                else
                {
                    IsMiniFps.IsChecked = false;
                }
            }

            //display current settings according to Settings.ini values
            HeaderFontSize1.Text = SettingsConfigurator.headerFontSize1.ToString();
            HeaderFontSize2.Text = SettingsConfigurator.headerFontSize2.ToString();
            ValuesFontSize.Text = SettingsConfigurator.valueFontSize.ToString();
            HeaderFontType1.ItemsSource = SettingsConfigurator.fontList;
            HeaderFontType1.SelectedValue = SettingsConfigurator.headerFont1;
            HeaderFontType2.ItemsSource = SettingsConfigurator.fontList;
            HeaderFontType2.SelectedValue = SettingsConfigurator.headerFont2;
            HeaderFontColor1.ItemsSource = SettingsConfigurator.colorList;
            HeaderFontColor1.SelectedValue = SettingsConfigurator.headerFontColor1;
            HeaderFontColor2.ItemsSource = SettingsConfigurator.colorList;
            HeaderFontColor2.SelectedValue = SettingsConfigurator.headerFontColor2;
            ValuesFontType.ItemsSource = SettingsConfigurator.fontList;
            ValuesFontType.SelectedValue = SettingsConfigurator.valueFont;
            ValuesFontColor.ItemsSource = SettingsConfigurator.colorList;
            ValuesFontColor.SelectedValue = SettingsConfigurator.valuesFontColor;
            Header1Position.Text = SettingsConfigurator.headerFont1Position.ToString();
            Header2Position.Text = SettingsConfigurator.headerFont2Position.ToString();
            ValuePosition.Text = SettingsConfigurator.valuesFontPosition.ToString();
            BackgroundFillColor.ItemsSource = SettingsConfigurator.colorList;
            BackgroundFillColor.SelectedValue = SettingsConfigurator.backgroundFillColor;
            Profiles.ItemsSource = SettingsConfigurator.profileList;
            Profiles.SelectedValue = currentProfile;
            BrightnessSlider.Value = SettingsConfigurator.displayBrightness;

            //clock settings
            TimeFontType.ItemsSource = SettingsConfigurator.fontList;
            TimeFontType.SelectedValue = SettingsConfigurator.timeFont;
            ColonFontType.ItemsSource = SettingsConfigurator.fontList;
            ColonFontType.SelectedValue = SettingsConfigurator.colonFont;
            DateFontType.ItemsSource = SettingsConfigurator.fontList;
            DateFontType.SelectedValue = SettingsConfigurator.dateFont;
            TimeFontSize.Text = SettingsConfigurator.timeFontSize.ToString();
            ColonFontSize.Text = SettingsConfigurator.colonFontSize.ToString();
            DateFontSize.Text = SettingsConfigurator.dateFontSize.ToString();
            TimeFontColor.ItemsSource = SettingsConfigurator.colorList;
            TimeFontColor.SelectedValue = SettingsConfigurator.timeFontColor;
            ColonFontColor.ItemsSource = SettingsConfigurator.colorList;
            ColonFontColor.SelectedValue = SettingsConfigurator.colonFontColor;
            DateFontColor.ItemsSource = SettingsConfigurator.colorList;
            DateFontColor.SelectedValue = SettingsConfigurator.dateFontColor;
            TimePosition.Text = SettingsConfigurator.timePosition.ToString();
            ColonPosition.Text = SettingsConfigurator.colonPosition.ToString();
            DatePosition.Text = SettingsConfigurator.datePosition.ToString();

            if (SharedSettings.IsDateShown() == "True")
            {
                IsDateShown.IsChecked = true;
            }
            else
            {
                IsDateShown.IsChecked = false;
            }
        }

        private void DragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //format text inputs on the fly to make sure only numerical values are entered        
        private void HeaderFontSize1Input(object sender, TextChangedEventArgs e)
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

        private void HeaderFontSize2Input(object sender, TextChangedEventArgs e)
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

        private void ValuesFontSizeInput(object sender, TextChangedEventArgs e)
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

        private void Header1PositionInput(object sender, TextChangedEventArgs e)
        {
            string value = Header1Position.Text;
            if (value == "" || value == " ")
            {
                Header1Position.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    Header1Position.Text = FormatValue(value, "");
                }
                else
                {
                    Header1Position.Text = FormatValue("1", "");
                }
            }
        }

        private void Header2PositionInput(object sender, TextChangedEventArgs e)
        {
            string value = Header2Position.Text;
            if (value == "" || value == " ")
            {
                Header2Position.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    Header2Position.Text = FormatValue(value, "");
                }
                else
                {
                    Header2Position.Text = FormatValue("1", "");
                }
            }
        }

        private void ValuesPositionInput(object sender, TextChangedEventArgs e)
        {
            string value = ValuePosition.Text;
            if (value == "" || value == " ")
            {
                ValuePosition.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    ValuePosition.Text = FormatValue(value, "");
                }
                else
                {
                    ValuePosition.Text = FormatValue("1", "");
                }
            }
        }

        private void AnimFramerateInput(object sender, TextChangedEventArgs e)
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

        private void FrameTotalInput(object sender, TextChangedEventArgs e)
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

        private void HeaderFontSizeClock1Input(object sender, TextChangedEventArgs e)
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

        private void HeaderFontSizeClock2Input(object sender, TextChangedEventArgs e)
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

        private void TimePositionInput(object sender, TextChangedEventArgs e)
        {
            string value = TimePosition.Text;
            if (value == "" || value == " ")
            {
                TimePosition.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    TimePosition.Text = FormatValue(value, "");
                }
                else
                {
                    TimePosition.Text = FormatValue("1", "");
                }
            }
        }

        private void ColonPositionInput(object sender, TextChangedEventArgs e)
        {
            string value = ColonPosition.Text;
            if (value == "" || value == " ")
            {
                ColonPosition.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    ColonPosition.Text = FormatValue(value, "");
                }
                else
                {
                    ColonPosition.Text = FormatValue("1", "");
                }
            }
        }

        private void DatePositionInput(object sender, TextChangedEventArgs e)
        {
            string value = DatePosition.Text;
            if (value == "" || value == " ")
            {
                DatePosition.Text = FormatValue("1", "");
            }
            else
            {
                if (IsDigitsOnly(value))
                {
                    DatePosition.Text = FormatValue(value, "");
                }
                else
                {
                    DatePosition.Text = FormatValue("1", "");
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
                    if (int.Parse(formattedValue) > SettingsConfigurator.frMax)
                    {
                        formattedValue = SettingsConfigurator.frMax.ToString();
                    }
                }

                if (type == "FA")
                {
                    if (int.Parse(formattedValue) > SettingsConfigurator.faMax)
                    {
                        formattedValue = SettingsConfigurator.faMax.ToString();
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

        private void ClickH1PUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(Header1Position.Text);
            Header1Position.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickH1PDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(Header1Position.Text);
            Header1Position.Text = ReturnValue(getValue, "FS", "Down").ToString();
        }

        private void ClickH2PUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(Header2Position.Text);
            Header2Position.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickH2PDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(Header2Position.Text);
            Header2Position.Text = ReturnValue(getValue, "FS", "Down").ToString();
        }

        private void ClickVPUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(ValuePosition.Text);
            ValuePosition.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickVPDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(ValuePosition.Text);
            ValuePosition.Text = ReturnValue(getValue, "FS", "Down").ToString();
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

        private void ClickCFSUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(ColonFontSize.Text);
            ColonFontSize.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickCFSDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(ColonFontSize.Text);
            ColonFontSize.Text = ReturnValue(getValue, "FS", "Down").ToString();
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

        private void ClickTPUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(TimePosition.Text);
            TimePosition.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickTPDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(TimePosition.Text);
            TimePosition.Text = ReturnValue(getValue, "FS", "Down").ToString();
        }

        private void ClickCPUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(ColonPosition.Text);
            ColonPosition.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickCPDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(ColonPosition.Text);
            ColonPosition.Text = ReturnValue(getValue, "FS", "Down").ToString();
        }

        private void ClickDPUp(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(DatePosition.Text);
            DatePosition.Text = ReturnValue(getValue, "FS", "Up").ToString();
        }

        private void ClickDPDown(object sender, RoutedEventArgs e)
        {
            int getValue = int.Parse(DatePosition.Text);
            DatePosition.Text = ReturnValue(getValue, "FS", "Down").ToString();
        }

        private int ReturnValue(int value, string type, string direction)
        {
            int adjustedValue = 0;
            int maxValue = 0;

            //set max numerical numbers allowed in each textbox
            if (type == "FS")
            {
                maxValue = SettingsConfigurator.fsMax;
            }
            if (type == "FR")
            {
                maxValue = SettingsConfigurator.frMax;
            }
            if (type == "FA")
            {
                maxValue = SettingsConfigurator.faMax;
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
            string statusText = "Loaded " + currentProfile;
            ThreadManager.DoStatusInBackground(statusText, "", this);

            SettingsConfigurator.RestartSDM();
        }

        //brightness slider
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int sliderValue = (int)e.NewValue;
            BrightnessPercent.Content = sliderValue + "%";
        }

        //reload stored values from config file
        private void ReloadSettings()
        {
            currentProfile = Profiles.Text;
            PrepValueDisplay(currentProfile);
        }

        //external call back to ReloadSettings()
        public void ReloadExt()
        {
            ReloadSettings();
        }

        public static void RestoreCancelExt()
        {
            MainWindow ExistingInstanceOfMainWindow = GetWindow(Application.Current.MainWindow) as MainWindow;
            ExistingInstanceOfMainWindow.DoRestoreCancel();
        }

        private void DoRestoreCancel()
        {
            ButtonLoadDefaults.IsEnabled = true;
            ButtonLoadDefaults.Effect = null;
        }

        public static void DoProfileRestore(string selectedProfiles)
        {
            if (System.Windows.Forms.MessageBox.Show("Are you sure you want to reset..\n\n\n" + selectedProfiles + "\n\nto default stock values ?? ", " Reset Profiles ", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                MainWindow ExistingInstanceOfMainWindow = GetWindow(Application.Current.MainWindow) as MainWindow;
                ThreadManager.ResetAllProfiles(ExistingInstanceOfMainWindow, selectedProfiles);

                //display notification
                string statusText = "Stock Settings Restored";
                ThreadManager.DoStatusInBackground(statusText, "Restore", ExistingInstanceOfMainWindow);
            }

            else
            {
                RestoreCancelExt();
            }
        }

        //main font button
        private void ButtonMonitor_MouseEnter(object sender, MouseEventArgs e)
        {
            if (TabMonitor.IsSelected == false)
            {
                DropShadowEffect dropShadow = new DropShadowEffect
                {
                    Color = Colors.LightBlue
                };
                ButtonMonitor.Effect = dropShadow;
                ButtonMonitorImage.Effect = dropShadow;
            }
        }

        private void ButtonMonitor_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TabMonitor.IsSelected == false)
            {
                ButtonMonitor.Effect = null;
                ButtonMonitorImage.Effect = null;
            }
        }

        private void ButtonMonitor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (TabClock.IsSelected == true)
            {
                TabMonitor.IsSelected = true;
                TabClock.IsSelected = false;
                ButtonMonitor.Effect = new DropShadowEffect();
                ButtonMonitorImage.Effect = new DropShadowEffect();
                ButtonClock.Effect = null;
                ButtonClockImage.Effect = null;
            }
        }

        //clock font button
        private void ButtonClock_MouseEnter(object sender, MouseEventArgs e)
        {
            if (TabClock.IsSelected == false)
            {
                DropShadowEffect dropShadow = new DropShadowEffect
                {
                    Color = Colors.LightBlue
                };
                ButtonClock.Effect = dropShadow;
                ButtonClockImage.Effect = dropShadow;
            }
        }

        private void ButtonClock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TabClock.IsSelected == false)
            {
                ButtonClock.Effect = null;
                ButtonClockImage.Effect = null;
            }
        }

        private void ButtonClock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (TabMonitor.IsSelected == true)
            {
                TabClock.IsSelected = true;
                TabMonitor.IsSelected = false;
                ButtonClock.Effect = new DropShadowEffect();
                ButtonClockImage.Effect = new DropShadowEffect();
                ButtonMonitor.Effect = null;
                ButtonMonitorImage.Effect = null;
            }
        }

        private void ButtonSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //disable button while saving values
            ButtonSave.IsEnabled = false;

            //set lists depending on device
            List<string> configValueList;

            //grab values used by both standard and mini configs
            string formattedValue = new string(BrightnessPercent.Content.ToString().Where(char.IsDigit).ToArray());
            string displayBrightness = "displayBrightness" + " " + formattedValue;
            string headerFontSize1 = "headerFontSize_1" + " " + HeaderFontSize1.Text;
            string headerFontSize2 = "headerFontSize_2" + " " + HeaderFontSize2.Text;
            string valuesFontSize = "valuesFontSize" + " " + ValuesFontSize.Text;
            string headerFontType1 = "headerFontType_1" + " " + HeaderFontType1.SelectedValue.ToString();
            string headerFontType2 = "headerFontType_2" + " " + HeaderFontType2.SelectedValue.ToString();
            string valuesFontType = "valuesFontType" + " " + ValuesFontType.SelectedValue.ToString();
            string headerfontColor1 = "headerfontColor_1" + " " + HeaderFontColor1.SelectedValue.ToString();
            string headerfontColor2 = "headerfontColor_2" + " " + HeaderFontColor2.SelectedValue.ToString();
            string valuesFontColor = "valuesFontColor" + " " + ValuesFontColor.SelectedValue.ToString();
            string backgroundColor = "backgroundColor" + " " + BackgroundFillColor.SelectedValue.ToString();
            string valuesFontPosition = "valuesFontPosition" + " " + ValuePosition.Text;
            string headerfont1Position = "headerFontPosition_1" + " " + Header1Position.Text;
            string headerfont2Position = "headerFontPosition_2" + " " + Header2Position.Text;

            //grab values used for mini config
            if (IsMiniFps.Visibility == Visibility.Visible)
            {
                string isFpsShown;

                if (IsMiniFps.IsChecked == true)
                {
                    isFpsShown = "showFpsCounter" + " " + "True";
                }
                else
                {
                    isFpsShown = "showFpsCounter" + " " + "False";
                }

                //store all mini values to pass to DoSaveInBackground()
                List<string> configMiniValueList = new List<string>
                {
                    isFpsShown
                };

                //send list to be processed in background threads
                ThreadManager.DoSaveInBackground(configMiniValueList, "miniconfig", this);

                //store all values to pass to DoSaveInBackground()
                configValueList = new List<string>
                {
                    headerFontSize1,
                    headerFontSize2,
                    valuesFontSize,
                    headerFontType1,
                    headerFontType2,
                    valuesFontType,
                    headerfontColor1,
                    headerfontColor2,
                    valuesFontColor,
                    valuesFontPosition,
                    headerfont1Position,
                    headerfont2Position,
                    backgroundColor,
                };
            }

            else
            {
                //grab values used for standard config
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

                //store all values to pass to DoSaveInBackground()
                configValueList = new List<string>
                {
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
                    valuesFontPosition,
                    headerfont1Position,
                    headerfont2Position,
                    backgroundColor,
                };
            }

            //clock settings
            string timeFont = "timeFontType" + " " + TimeFontType.SelectedValue.ToString();
            string colonFont = "colonFontType" + " " + ColonFontType.SelectedValue.ToString();
            string dateFont = "dateFontType" + " " + DateFontType.SelectedValue.ToString();
            string timeFontSize = "timeFontSize" + " " + TimeFontSize.Text;
            string colonFontSize = "colonFontSize" + " " + ColonFontSize.Text;
            string dateFontSize = "dateFontSize" + " " + DateFontSize.Text;
            string timeFontColor = "timeFontColor" + " " + TimeFontColor.SelectedValue.ToString();
            string colonFontColor = "colonFontColor" + " " + ColonFontColor.SelectedValue.ToString();
            string dateFontColor = "dateFontColor" + " " + DateFontColor.SelectedValue.ToString();
            string timePosition = "timeFontPosition" + " " + TimePosition.Text;
            string colonPosition = "colonFontPosition" + " " + ColonPosition.Text;
            string datePosition = "dateFontPosition" + " " + DatePosition.Text;
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

            //store all clock values to pass to DoSaveInBackground()
            List<string> clockValueList = new List<string>
            {
                timeFont,
                colonFont,
                dateFont,
                timeFontSize,
                colonFontSize,
                dateFontSize,
                timeFontColor,
                colonFontColor,
                dateFontColor,
                timePosition,
                colonPosition,
                datePosition,
                isCompact,
                isDateShown
             };

            //store all mini values to pass to DoSaveInBackground()
            List<string> brightnessSetting = new List<string>
                {
                    displayBrightness
                };

            //send lists to be processed in background threads
            ThreadManager.DoSaveInBackground(brightnessSetting, "mainconfig", this);
            ThreadManager.DoSaveInBackground(configValueList, "mainconfig", this);
            ThreadManager.DoSaveInBackground(clockValueList, "clockconfig", this);

            //display notification
            string statusText = "Settings Saved";
            ThreadManager.DoStatusInBackground(statusText, "Save", this);

            SettingsConfigurator.RestartSDM();
        }

        private void ButtonSave_MouseEnter(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadow = new DropShadowEffect
            {
                Color = Colors.LimeGreen
            };
            ButtonSave.Effect = dropShadow;
            ButtonSaveImage.Effect = dropShadow;
        }

        private void ButtonSave_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonSave.Effect = null;
            ButtonSaveImage.Effect = null;
        }

        //restore default settings button
        private void ButtonLoadDefaults_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ButtonLoadDefaults.Effect = new DropShadowEffect();
        }

        private void ButtonLoadDefaults_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ButtonLoadDefaults.IsEnabled = false;
            ButtonLoadDefaults.Effect = new DropShadowEffect();
            DialogRestoreChoice restoreConfig = new DialogRestoreChoice();
            restoreConfig.Show();
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

        private void SelectDevice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DialogDeviceChoice deviceChoice = new DialogDeviceChoice();
            deviceChoice.Show();
            Close();
        }

        private void SelectDevice_MouseEnter(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadow = new DropShadowEffect
            {
                Color = Colors.LightBlue
            };

            if (SelectDevice6Button.Visibility == Visibility.Visible)
            {
                SelectDevice6Button.ToolTip = "Stream Deck Mini (Click to change device)";
                SelectDevice6Button.Effect = dropShadow;
            }

            if (SelectDevice15Button.Visibility == Visibility.Visible)
            {
                SelectDevice15Button.ToolTip = "Stream Deck Full Size (Click to change device)";
                SelectDevice15Button.Effect = dropShadow;
            }
        }

        private void SelectDevice_MouseLeave(object sender, MouseEventArgs e)
        {
            if (SelectDevice6Button.Visibility == Visibility.Visible)
            {
                SelectDevice6Button.Effect = null;
            }

            if (SelectDevice15Button.Visibility == Visibility.Visible)
            {
                SelectDevice15Button.Effect = null;
            }
        }

        private void ButtonHelp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ButtonHelp.Effect = null;
            DialogHelp helpInfo = new DialogHelp();
            helpInfo.Show();
        }

        private void ButtonHelp_MouseEnter(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadow = new DropShadowEffect
            {
                Color = Colors.Gold
            };
            ButtonHelp.Effect = dropShadow;
        }

        private void ButtonHelp_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonHelp.Effect = null;
        }

        private void ButtonExit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ButtonExit.Effect = null;
            Application.Current.Shutdown();
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

        private void ButtonBrightness_MouseEnter(object sender, MouseEventArgs e)
        {
            DropShadowEffect dropShadow = new DropShadowEffect
            {
                Color = Colors.LightBlue
            };
            ButtonBrightness.Effect = dropShadow;
            ButtonBrightnessImage.Effect = dropShadow;
            BrightnessSlider.Effect = dropShadow;
        }

        private void ButtonBrightness_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonBrightness.Effect = null;
            ButtonBrightnessImage.Effect = null;
            BrightnessSlider.Effect = null;
        }
    }
}
