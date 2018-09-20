using SharedManagers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media;

namespace Configurator
{
    class ThreadManager
    {
        //background worker for displaying current status
        public static void DoStatusInBackground(Brush selectedColor, string statusText, string button, MainWindow configurator)
        {
            //create a background worker
            var backgroundStatus = new BackgroundWorker();
            backgroundStatus.DoWork += (s, ea) => Thread.Sleep(TimeSpan.FromSeconds(1));

            //define work to be done
            backgroundStatus.RunWorkerCompleted += (s, ea) =>
            {
                configurator.StatusLabel.Content = "";
                configurator.StatusLabel.Background = null;

                if (button != "")
                {
                    if (button == "Reload")
                    {
                        configurator.ButtonReload.IsEnabled = true;
                    }

                    if (button == "Save")
                    {
                        configurator.ButtonSave.IsEnabled = true;
                    }

                    if (button == "Restore")
                    {
                        configurator.ButtonRestoreConfig.IsEnabled = true;
                    }
                }
            };

            //display notification
            configurator.StatusLabel.Background = selectedColor;
            configurator.StatusLabel.Foreground = Brushes.White;
            configurator.StatusLabel.Content = statusText;

            //start the background worker to reset both the label and button to default states
            backgroundStatus.RunWorkerAsync();
        }

        //background worker for saving settings to config file
        public static void DoSaveInBackground(List<string> configValueList, string configType, MainWindow configurator)
        {
            if (configType == "mainconfig" || configType == "miniconfig")
            {
                SaveSettings(configValueList, configurator, "");
            }

            else if (configType == "clockconfig")
            {
                SaveSettings(configValueList, configurator, "clockconfig");
            }
        }

        private static void SaveSettings(List<string> configValueList, MainWindow configurator, string configType)
        {
            string settingHeading = configurator.Profiles.Text;

            if (configValueList != null)
            {
                //create a background worker
                var backgroundSave = new BackgroundWorker();
                backgroundSave.DoWork += (s, ea) => Thread.Sleep(TimeSpan.FromSeconds(0));

                //define work to be done
                backgroundSave.RunWorkerCompleted += (s, ea) =>
                {
                    foreach (var configSetting in configValueList)
                    {
                        if (configSetting != "" || configSetting != null)
                        {
                            //split the raw string into both type and value
                            string rawString = configSetting;
                            string[] splitString = rawString.Split(new char[] { ' ' }, 2);

                            splitString[1] = splitString[1].TrimStart();
                            string settingType = splitString[0];
                            string settingValue = splitString[1];

                            if (configType == "clockconfig")
                            {
                                settingHeading = "Clock_Settings";
                            }

                            if (settingType == "selectedProfile")
                            {
                                settingHeading = "Current_Profile";
                            }

                            if (settingType == "showFpsCounter")
                            {
                                settingHeading = "Mini_Settings";
                            }

                            //write the type and value to config file under the correct profile heading
                            SharedSettings.config.Write(settingType, settingValue, settingHeading);
                        }
                    }

                    //reload saved values
                    configurator.ReloadExt();
                };

                //start the background worker to reset both the label and button to default states
                backgroundSave.RunWorkerAsync();
            }
        }

        public static void ResetAllProfiles(MainWindow configurator, string selectedProfiles)
        {
            //create a background worker
            var backgroundSave = new BackgroundWorker();
            backgroundSave.DoWork += (s, ea) => Thread.Sleep(TimeSpan.FromSeconds(0));

            //define work to be done
            backgroundSave.RunWorkerCompleted += (s, ea) =>
            {
                if (selectedProfiles.Contains("Clock"))
                {
                    DefaultClockConfig();
                }

                if (selectedProfiles.Contains("1"))
                {
                    DefaultProfile1Config();
                }

                if (selectedProfiles.Contains("2"))
                {
                    DefaultProfile2Config();
                }

                if (selectedProfiles.Contains("3"))
                {
                    DefaultProfile3Config();
                }

                if (selectedProfiles.Contains("All"))
                {
                    DefaultClockConfig();
                    DefaultProfile1Config();
                    DefaultProfile2Config();
                    DefaultProfile3Config();
                    DefaultDeviceStateConfig();
                    DeviceChoice deviceChoice = new DeviceChoice();
                    deviceChoice.Show();
                    configurator.Close();
                }

                //reload saved values
                configurator.ReloadExt();
                SettingsConfigurator.RestartSDM();
            };

            //start the background worker
            backgroundSave.RunWorkerAsync();
        }

        private static void DefaultClockConfig()
        {
            //clock config
            SharedSettings.config.Write("compactView", "False", "Clock_Settings");
            SharedSettings.config.Write("showDate", "True", "Clock_Settings");
            SharedSettings.config.Write("timeFontType", "Sketch Block", "Clock_Settings");
            SharedSettings.config.Write("colonFontType", "Sketch Block", "Clock_Settings");
            SharedSettings.config.Write("dateFontType", "Birth of a Hero", "Clock_Settings");
            SharedSettings.config.Write("timeFontSize", "40", "Clock_Settings");
            SharedSettings.config.Write("colonFontSize", "43", "Clock_Settings");
            SharedSettings.config.Write("dateFontSize", "30", "Clock_Settings");
            SharedSettings.config.Write("timeFontColor", "Teal", "Clock_Settings");
            SharedSettings.config.Write("colonFontColor", "Teal", "Clock_Settings");
            SharedSettings.config.Write("dateFontColor", "Teal", "Clock_Settings");
            SharedSettings.config.Write("timeFontPosition", "40", "Clock_Settings");
            SharedSettings.config.Write("colonFontPosition", "35", "Clock_Settings");
            SharedSettings.config.Write("dateFontPosition", "35", "Clock_Settings");
        }

        private static void DefaultProfile1Config()
        {
            //Profile 1 config
            SharedSettings.config.Write("headerFontType_1", "Beyond The Mountains", "Profile 1");
            SharedSettings.config.Write("headerFontType_2", "Beyond The Mountains", "Profile 1");
            SharedSettings.config.Write("headerFontColor_1", "Gold", "Profile 1");
            SharedSettings.config.Write("headerFontColor_2", "Gold", "Profile 1");
            SharedSettings.config.Write("headerFontSize_1", "28", "Profile 1");
            SharedSettings.config.Write("headerFontSize_2", "18", "Profile 1");
            SharedSettings.config.Write("valuesFontType", "Strange Shadow", "Profile 1");
            SharedSettings.config.Write("valuesFontColor", "White", "Profile 1");
            SharedSettings.config.Write("valuesFontSize", "15", "Profile 1");
            SharedSettings.config.Write("headerFontPosition_1", "35", "Profile 1");
            SharedSettings.config.Write("headerFontPosition_2", "18", "Profile 1");
            SharedSettings.config.Write("valuesFontPosition", "50", "Profile 1");
            SharedSettings.config.Write("backgroundColor", "Black", "Profile 1");
            SharedSettings.config.Write("animationEnabled", "False", "Profile 1");
            SharedSettings.config.Write("animationFramerate", "30", "Profile 1");
            SharedSettings.config.Write("framesToProcess", "150", "Profile 1");
            SharedSettings.config.Write("imageName", "Faded Red Lines", "Profile 1");
            SharedSettings.config.Write("animName", "80's Triangles", "Profile 1");
            SharedSettings.config.Write("displayBrightness", "60", "Profile 1");
        }

        private static void DefaultProfile2Config()
        {
            //Profile 2 config
            SharedSettings.config.Write("headerFontType_1", "Beauty and Beast", "Profile 2");
            SharedSettings.config.Write("headerFontType_2", "Beyond The Mountains", "Profile 2");
            SharedSettings.config.Write("headerFontColor_1", "Teal", "Profile 2");
            SharedSettings.config.Write("headerFontColor_2", "Teal", "Profile 2");
            SharedSettings.config.Write("headerFontSize_1", "30", "Profile 2");
            SharedSettings.config.Write("headerFontSize_2", "14", "Profile 2");
            SharedSettings.config.Write("valuesFontType", "Gloss and Bloom", "Profile 2");
            SharedSettings.config.Write("valuesFontColor", "White", "Profile 2");
            SharedSettings.config.Write("valuesFontSize", "20", "Profile 2");
            SharedSettings.config.Write("headerFontPosition_1", "35", "Profile 2");
            SharedSettings.config.Write("headerFontPosition_2", "16", "Profile 2");
            SharedSettings.config.Write("valuesFontPosition", "50", "Profile 2");
            SharedSettings.config.Write("backgroundColor", "Black", "Profile 2");
            SharedSettings.config.Write("animationEnabled", "True", "Profile 2");
            SharedSettings.config.Write("animationFramerate", "30", "Profile 2");
            SharedSettings.config.Write("framesToProcess", "48", "Profile 2");
            SharedSettings.config.Write("imageName", "Blue Carbon", "Profile 2");
            SharedSettings.config.Write("animName", "Black and White Squares", "Profile 2");
            SharedSettings.config.Write("displayBrightness", "60", "Profile 2");
        }

        private static void DefaultProfile3Config()
        {
            //Profile 3 config
            SharedSettings.config.Write("headerFontType_1", "True Lies", "Profile 3");
            SharedSettings.config.Write("headerFontType_2", "True Lies", "Profile 3");
            SharedSettings.config.Write("headerFontColor_1", "Pink", "Profile 3");
            SharedSettings.config.Write("headerFontColor_2", "Pink", "Profile 3");
            SharedSettings.config.Write("headerFontSize_1", "28", "Profile 3");
            SharedSettings.config.Write("headerFontSize_2", "14", "Profile 3");
            SharedSettings.config.Write("valuesFontType", "Gloss and Bloom", "Profile 3");
            SharedSettings.config.Write("valuesFontColor", "Mint", "Profile 3");
            SharedSettings.config.Write("valuesFontSize", "20", "Profile 3");
            SharedSettings.config.Write("headerFontPosition_1", "35", "Profile 3");
            SharedSettings.config.Write("headerFontPosition_2", "18", "Profile 3");
            SharedSettings.config.Write("valuesFontPosition", "50", "Profile 3");
            SharedSettings.config.Write("backgroundColor", "Black", "Profile 3");
            SharedSettings.config.Write("animationEnabled", "True", "Profile 3");
            SharedSettings.config.Write("animationFramerate", "30", "Profile 3");
            SharedSettings.config.Write("framesToProcess", "124", "Profile 3");
            SharedSettings.config.Write("imageName", "Faded Red Lines", "Profile 3");
            SharedSettings.config.Write("animName", "80's Triangles", "Profile 3");
            SharedSettings.config.Write("displayBrightness", "60", "Profile 3");
        }

        private static void DefaultDeviceStateConfig()
        {
            //device config
            SharedSettings.config.Write("choiceMade", "false", "StreamDeck_Device");
            SharedSettings.config.Write("selectedDevice", "1", "StreamDeck_Device");
            //state config
            SharedSettings.config.Write("seletedState", "1", "Current_State");
            //profile selection config
            SharedSettings.config.Write("selectedProfile", "Profile 2", "Current_Profile");
        }
    }
}
