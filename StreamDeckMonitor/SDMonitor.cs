using MSI.Afterburner;
using OpenHardwareMonitor.Hardware;
using OpenMacroBoard.SDK;
using SharedManagers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamDeckMonitor
{
    class SDMonitor
    {
        static void Main(string[] args)
        {
            var isMiniDeck = false;

            //make sure only one instance is running
            SharedSettings.CheckForTwins();

            //check for device layout and adjust for Mini or Standard StreamDeck
            if (SettingsManagerSDM.CheckForLayout() != null)
            {
                if (SettingsManagerSDM.CheckForLayout() == "Mini")
                {
                    isMiniDeck = true;
                }
            }

            //clean display and set brightness
            ImageManager.deck.ClearKeys();
            var displayBrightness = Convert.ToByte(SettingsManagerSDM.displayBrightness);
            ImageManager.deck.SetBrightness(displayBrightness);

            //create and process necessary header images 
            ImageManager.ProcessHeaderImages();

            //check for State setting and start State accodingly
            if (SharedSettings.CurrentSate() == 1)
            {
                if (isMiniDeck == true)
                {
                    StartMonitorStateMini();
                }
                else
                {
                    StartMonitorState();
                }
            }

            if (SharedSettings.CurrentSate() == 2)
            {
                StartClockState();
            }

            //MonitorStateMini
            void StartMonitorStateMini()
            {
                try
                {
                    //define Librehardwaremonitor sensors and connect (CPU temp data requires 'highestAvailable' requestedExecutionLevel !!)
                    Computer computer = new Computer() { CPUEnabled = true, GPUEnabled = true };
                    computer.Open();

                    //set static header images 
                    ImageManager.SetStaticHeaders();

                    StartMonitor();

                    void StartMonitor()
                    {
                        int counterDefault = 1;

                        //start loop
                        while (true)
                        {
                            if (ImageManager.exitflag) break;

                            int countValue = counterDefault++;

                            try
                            {
                                //add key press handler, if pressed send exit command 
                                ImageManager.deck.KeyStateChanged += DeckKeyPressed;

                                //search hardware
                                foreach (OpenHardwareMonitor.Hardware.IHardware hardware in computer.Hardware)
                                {
                                    hardware.Update();

                                    //check for gpu sensor
                                    if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAti)
                                    {
                                        foreach (ISensor sensor in hardware.Sensors)
                                        {
                                            //search for temp sensor
                                            if (sensor.SensorType == SensorType.Temperature)
                                            {
                                                string dataValue = sensor.Value.ToString() + "c";
                                                string type = "t";
                                                ImageManager.ProcessValueImg(dataValue, type, SettingsManagerSDM.KeyLocGpuTempMini);
                                            }

                                            //search for load sensor
                                            if (sensor.SensorType == SensorType.Load)
                                            {
                                                //add gpu load sensors to list
                                                string getValues = sensor.Name + sensor.Value.ToString();
                                                List<string> valueList = new List<string>
                                            {
                                                getValues
                                            };
                                                //get values for gpu and pass to process
                                                foreach (string value in valueList)
                                                {
                                                    if (value.Contains("GPU Core"))
                                                    {
                                                        //get values for gpu and pass to process
                                                        int gpuLoadInt = (int)Math.Round(sensor.Value.Value);
                                                        string dataValue = gpuLoadInt.ToString() + "%";
                                                        string type = "l";
                                                        ImageManager.ProcessValueImg(dataValue, type, SettingsManagerSDM.KeyLocGpuLoadMini);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //check for cpu sensor
                                    if (hardware.HardwareType == HardwareType.CPU)
                                    {

                                        foreach (ISensor sensor in hardware.Sensors)
                                        {
                                            //search for temp sensor
                                            if (sensor.SensorType == SensorType.Temperature)
                                            {
                                                //add cpu temp sensors to list
                                                string getValues = sensor.Name + sensor.Value.ToString();
                                                List<string> valueList = new List<string>
                                            {
                                                getValues
                                            };
                                                //get values for cpu and pass to process
                                                foreach (string value in valueList)
                                                {
                                                    if (value.Contains("CPU Package"))
                                                    {
                                                        string resultPackage = value.Substring(Math.Max(0, value.Length - 2));
                                                        if (!resultPackage.Contains("#"))
                                                        {
                                                            string dataValue = resultPackage.ToString() + "c";
                                                            string type = "t";
                                                            ImageManager.ProcessValueImg(dataValue, type, SettingsManagerSDM.KeyLocCpuTempMini);
                                                        }
                                                    }
                                                }
                                            }

                                            //search for load sensor
                                            if (sensor.SensorType == SensorType.Load)
                                            {
                                                //add cpu load sensors to list
                                                string getValues = sensor.Name + sensor.Value.ToString();
                                                List<string> valueList = new List<string>
                                            {
                                                getValues
                                            };
                                                //get values for cpu and change Stream Deck image
                                                foreach (string value in valueList)
                                                {
                                                    if (value.Contains("CPU Total"))
                                                    {
                                                        //get values for cpu and pass to process
                                                        int cpuLoadInt = (int)Math.Round(sensor.Value.Value);
                                                        string dataValue = cpuLoadInt.ToString() + "%";
                                                        string type = "l";
                                                        ImageManager.ProcessValueImg(dataValue, type, SettingsManagerSDM.KeyLocCpuLoadMini);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //wait 1 second before restarting loop
                                System.Threading.Thread.Sleep(1000);

                                //remove handler
                                ImageManager.deck.KeyStateChanged -= DeckKeyPressed;
                            }

                            catch (Exception)
                            {
                                ExitApp();
                            }
                        }

                        //check for button input
                        void DeckKeyPressed(object sender, OpenMacroBoard.SDK.KeyEventArgs e)
                        {
                            try
                            {
                                if (e.Key == 0)
                                {
                                    if (e.IsDown == true)
                                    {
                                        //write State to config file then start ClockState
                                        SharedSettings.config.Write("seletedState", "2", "Current_State");
                                        //restart to show new state
                                        RestartApp();
                                    }
                                }

                                if (e.Key == 4)
                                {
                                    if (e.IsDown == true)
                                    {
                                        //stop animation and clear display for clean exit
                                        ImageManager.exitflag = true;
                                        ImageManager.deck.ClearKeys();
                                        System.Threading.Thread.Sleep(1000);
                                        ImageManager.deck.ShowLogo();

                                        //close StreamDeckMonitor
                                        ExitApp();
                                    }
                                }
                            }

                            catch (Exception)
                            {
                                ExitApp();
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    if (ex.InnerException is StreamDeckSharp.Exceptions.StreamDeckNotFoundException)
                    {
                        string deviceNotFound = " 'Stream Deck' Device not found/connected ! ";
                        MessageBox.Show(deviceNotFound);
                    }
                    ExitApp();
                }
            }

            //MonitorState
            void StartMonitorState()
            {
                try
                {
                    //MSI Afterburner shared memory
                    HardwareMonitor mahm;

                    //check if MSIAfterburner process is running
                    bool isABRunning = false;
                    if (SettingsManagerSDM.CheckForAB() == true)
                    {
                        mahm = new HardwareMonitor();
                        mahm.Connect();
                        isABRunning = true;
                    }
                    else
                    {
                        mahm = null;
                    }

                    //define Librehardwaremonitor sensors and connect (CPU temp data requires 'highestAvailable' requestedExecutionLevel !!)
                    Computer computer = new Computer() { CPUEnabled = true, GPUEnabled = true };
                    computer.Open();

                    //set static header images 
                    ImageManager.SetStaticHeaders();

                    //check if using animations or static images
                    string currentProfile = SharedSettings.config.Read("selectedProfile", "Current_Profile");
                    if (SharedSettings.IsAnimationEnabled(currentProfile) != "True")
                    {
                        foreach (var button in SettingsManagerSDM.BgButtonList())
                        {
                            ImageManager.SetStaticImg(SettingsManagerSDM.imageName, button);
                        }

                        //start standard loop without the image animations 
                        StartMonitor();
                    }
                    else
                    {
                        //start both loops in parallel
                        Parallel.Invoke(() => ImageManager.StartAnimation(), () => StartMonitor());
                    }

                    void StartMonitor()
                    {
                        int counterDefault = 1;

                        //start loop
                        while (true)
                        {
                            int countValue = counterDefault++;

                            if (ImageManager.exitflag) break;

                            try
                            {
                                //add key press handler, if pressed send exit command 
                                ImageManager.deck.KeyStateChanged += DeckKeyPressed;

                                //connect to msi afterburner and reload values
                                if (isABRunning == true)
                                {
                                    var framerateEntry = mahm.GetEntry(HardwareMonitor.GPU_GLOBAL_INDEX, MONITORING_SOURCE_ID.FRAMERATE);
                                    mahm.ReloadEntry(framerateEntry);

                                    //get values for framerate and pass to process
                                    int framerateInt = (int)Math.Round(framerateEntry.Data);
                                    string dataValue = framerateInt.ToString();
                                    string type = "f";
                                    ImageManager.ProcessValueImg(dataValue, type, SettingsManagerSDM.KeyLocFps);
                                }
                                else
                                {
                                    string dataValue = "0";
                                    string type = "f";
                                    ImageManager.ProcessValueImg(dataValue, type, SettingsManagerSDM.KeyLocFps);
                                }

                                //get and set time 
                                string timeOutput = DateTime.Now.ToString("hh:mm");

                                if (timeOutput.StartsWith("0"))
                                {
                                    timeOutput = timeOutput.Remove(0, 1);
                                }
                                ImageManager.ProcessValueImg(timeOutput, "ti", SettingsManagerSDM.KeyLocTimeHeader);

                                //search hardware
                                foreach (OpenHardwareMonitor.Hardware.IHardware hardware in computer.Hardware)
                                {
                                    hardware.Update();

                                    //check for gpu sensor
                                    if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAti)
                                    {
                                        foreach (ISensor sensor in hardware.Sensors)
                                        {
                                            //search for temp sensor
                                            if (sensor.SensorType == SensorType.Temperature)
                                            {
                                                string dataValue = sensor.Value.ToString() + "c";
                                                string type = "t";
                                                ImageManager.ProcessValueImg(dataValue, type, SettingsManagerSDM.KeyLocGpuTemp);
                                            }

                                            //search for load sensor
                                            if (sensor.SensorType == SensorType.Load)
                                            {
                                                //add gpu load sensors to list
                                                string getValues = sensor.Name + sensor.Value.ToString();
                                                List<string> valueList = new List<string>
                                            {
                                                getValues
                                            };
                                                //get values for gpu and pass to process
                                                foreach (string value in valueList)
                                                {
                                                    if (value.Contains("GPU Core"))
                                                    {
                                                        //get values for gpu and pass to process
                                                        int gpuLoadInt = (int)Math.Round(sensor.Value.Value);
                                                        string dataValue = gpuLoadInt.ToString() + "%";
                                                        string type = "l";
                                                        ImageManager.ProcessValueImg(dataValue, type, SettingsManagerSDM.KeyLocGpuLoad);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //check for cpu sensor
                                    if (hardware.HardwareType == HardwareType.CPU)
                                    {

                                        foreach (ISensor sensor in hardware.Sensors)
                                        {
                                            //search for temp sensor
                                            if (sensor.SensorType == SensorType.Temperature)
                                            {
                                                //add cpu temp sensors to list
                                                string getValues = sensor.Name + sensor.Value.ToString();
                                                List<string> valueList = new List<string>
                                            {
                                                getValues
                                            };
                                                //get values for cpu and pass to process
                                                foreach (string value in valueList)
                                                {
                                                    if (value.Contains("CPU Package"))
                                                    {
                                                        string resultPackage = value.Substring(Math.Max(0, value.Length - 2));
                                                        if (!resultPackage.Contains("#"))
                                                        {
                                                            string dataValue = resultPackage.ToString() + "c";
                                                            string type = "t";
                                                            ImageManager.ProcessValueImg(dataValue, type, SettingsManagerSDM.KeyLocCpuTemp);
                                                        }
                                                    }
                                                }
                                            }

                                            //search for load sensor
                                            if (sensor.SensorType == SensorType.Load)
                                            {
                                                //add cpu load sensors to list
                                                string getValues = sensor.Name + sensor.Value.ToString();
                                                List<string> valueList = new List<string>
                                            {
                                                getValues
                                            };
                                                //get values for cpu and change Stream Deck image
                                                foreach (string value in valueList)
                                                {
                                                    if (value.Contains("CPU Total"))
                                                    {
                                                        //get values for cpu and pass to process
                                                        int cpuLoadInt = (int)Math.Round(sensor.Value.Value);
                                                        string dataValue = cpuLoadInt.ToString() + "%";
                                                        string type = "l";
                                                        ImageManager.ProcessValueImg(dataValue, type, SettingsManagerSDM.KeyLocCpuLoad);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //wait 1 second before restarting loop
                                System.Threading.Thread.Sleep(1000);

                                //remove handler
                                ImageManager.deck.KeyStateChanged -= DeckKeyPressed;
                            }

                            catch (Exception)
                            {
                                ExitApp();
                            }
                        }

                        //check for button input
                        void DeckKeyPressed(object sender, OpenMacroBoard.SDK.KeyEventArgs e)
                        {
                            try
                            {
                                if (e.Key == 0)
                                {
                                    if (e.IsDown == true)
                                    {
                                        //write State to config file then start ClockState
                                        SharedSettings.config.Write("seletedState", "2", "Current_State");
                                        //restart to show new state
                                        RestartApp();
                                    }
                                }

                                if (e.Key == 7)
                                {
                                    if (e.IsDown == true)
                                    {
                                        //close all monitoring connections
                                        if (isABRunning == true)
                                        {
                                            mahm.Disconnect();
                                        }
                                        computer.Close();

                                        //stop animation and clear display for clean exit
                                        ImageManager.exitflag = true;
                                        ImageManager.deck.ClearKeys();
                                        System.Threading.Thread.Sleep(1000);
                                        ImageManager.deck.ShowLogo();

                                        //close StreamDeckMonitor
                                        ExitApp();
                                    }
                                }
                            }

                            catch (Exception)
                            {
                                ExitApp();
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    if (ex.InnerException is StreamDeckSharp.Exceptions.StreamDeckNotFoundException)
                    {
                        string deviceNotFound = " 'Stream Deck' Device not found/connected ! ";
                        MessageBox.Show(deviceNotFound);
                    }
                    ExitApp();
                }
            }

            // ClockState
            void StartClockState()
            {
                if (isMiniDeck == false)
                {
                    //clear the FPS monitoring button image/animation again seperatly. This is to prevent corrupted images being left on that button when switching to Clock state
                    ImageManager.deck.ClearKey(0);
                }

                //start both loops in parallel
                Parallel.Invoke(() => ImageManager.StartAnimClock(), () => StartClock());
            }

            void StartClock()
            {
                //start loop
                while (true)
                {
                    if (ImageManager.exitflag) break;
                    try
                    {
                        //add key press handler, if pressed send exit command 
                        ImageManager.deck.KeyStateChanged += DeckKeyPressed;

                        //get current time
                        DateTime now = DateTime.Now;

                        string hours = now.Hour.ToString();

                        if (hours.Length < 2)
                        {
                            hours = "0" + hours;
                        }

                        string minutes = now.ToString("mm");

                        if (isMiniDeck == true)
                        {
                            //send current time to ImageManager
                            ImageManager.ClockStateMini(hours, minutes);
                        }
                        else
                        {
                            //send current time to ImageManager
                            ImageManager.ClockState(hours, minutes);
                        }

                        //wait 10 seconds between getting current time
                        System.Threading.Thread.Sleep(10000);

                        //remove handler
                        ImageManager.deck.KeyStateChanged -= DeckKeyPressed;
                    }

                    catch (Exception)
                    {
                        ExitApp();
                    }
                }

                //check for button input
                void DeckKeyPressed(object sender, OpenMacroBoard.SDK.KeyEventArgs e)
                {
                    var monitorButton = 4;
                    var exitButton = 7;

                    if (isMiniDeck == true)
                    {
                        monitorButton = 2;
                        exitButton = 4;
                    }

                    try
                    {
                        if (e.Key == monitorButton)
                        {
                            if (e.IsDown == true)
                            {
                                //write State to config file then start MonitorState
                                SharedSettings.config.Write("seletedState", "1", "Current_State");
                                //restart to show new state
                                RestartApp();
                            }
                        }

                        if (e.Key == exitButton)
                        {
                            if (e.IsDown == true)
                            {
                                //clear display for clean exit
                                ImageManager.exitflag = true;
                                ImageManager.deck.ClearKeys();
                                System.Threading.Thread.Sleep(1000);
                                ImageManager.deck.ShowLogo();

                                //close StreamDeckMonitor
                                ExitApp();
                            }
                        }
                    }

                    catch (Exception)
                    {
                        ExitApp();
                    }
                }
            }
        }

        static void ExitApp()
        {
            Environment.Exit(Environment.ExitCode);
        }

        static void RestartApp()
        {
            //start a new instance of the program then close current
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            ExitApp();
        }
    }
}
