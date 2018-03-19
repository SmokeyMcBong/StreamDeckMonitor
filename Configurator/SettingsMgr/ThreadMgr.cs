using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media;

namespace Configurator
{
    class ThreadMgr
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
        public static void DoSaveInBackground(List<string> configValueList, MainWindow configurator)
        {
            string currentProfile;

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

                            if (settingType == "selectedProfile")
                            {
                                currentProfile = "Current_Profile";
                            }
                            else
                            {
                                currentProfile = configurator.Profiles.Text;
                            }

                            //write the type and value to config file under the correct profile heading
                            SettingsMgr.config.Write(settingType, settingValue, currentProfile);
                        }
                    }

                    configurator.ReloadExt();
                    SettingsMgr.RestartSDM();
                };

                //start the background worker to reset both the label and button to default states
                backgroundSave.RunWorkerAsync();
            }
        }
    }
}