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
        static bool isMiniDeck = false;
        static bool isABRunning = false;
        static HardwareMonitor msiAB = new HardwareMonitor();
        static Computer ohmComputer = new Computer() { CPUEnabled = true, GPUEnabled = true };

        static void Main(string[] args)
        {
            //make sure only one instance is running
            SharedSettings.CheckForTwins();

            //check for device layout and adjust for Mini or Standard StreamDeck
            if (SettingsSDMonitor.CheckForLayout() != null)
            {
                if (SettingsSDMonitor.CheckForLayout() == "Mini")
                {
                    isMiniDeck = true;
                }
            }

            //clean display and set brightness
            ImageManager.deck.ClearKeys();
            var displayBrightness = Convert.ToByte(SettingsSDMonitor.displayBrightness);
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
                var showFps = false;
                if (SettingsSDMonitor.isFpsCounter == "True")
                {
                    showFps = true;
                }

                try
                {
                    //check if MSIAfterburner process is running
                    if (SettingsSDMonitor.CheckForAB() == true)
                    {
                        msiAB.Connect();
                        isABRunning = true;
                    }

                    else
                    {
                        msiAB = null;
                    }

                    //define Librehardwaremonitor sensors and connect (CPU temp data requires 'highestAvailable' requestedExecutionLevel !!)
                    ohmComputer.Open();

                    //set static header images 
                    ImageManager.SetStaticHeaders();

                    StartMonitorMini();

                    void StartMonitorMini()
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

                                if (showFps == true)
                                {
                                    //connect to msi afterburner and reload values
                                    if (isABRunning == true)
                                    {
                                        var framerateEntry = msiAB.GetEntry(HardwareMonitor.GPU_GLOBAL_INDEX, MONITORING_SOURCE_ID.FRAMERATE);
                                        msiAB.ReloadEntry(framerateEntry);

                                        //get values for framerate and pass to process
                                        int framerateInt = (int)Math.Round(framerateEntry.Data);
                                        string dataValue = framerateInt.ToString();
                                        string type = "fmini";
                                        ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocFpsMini);
                                    }
                                    else
                                    {
                                        string dataValue = "0";
                                        string type = "fmini";
                                        ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocFpsMini);
                                    }
                                }

                                //search hardware
                                foreach (IHardware hardware in ohmComputer.Hardware)
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
                                                ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocGpuTempMini);
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
                                                        ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocGpuLoadMini);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //check for cpu sensor
                                    if (hardware.HardwareType == HardwareType.CPU)
                                    {
                                        string cpuTempString;

                                        if (hardware.Name.Contains("Ryzen"))
                                        {
                                            cpuTempString = "Core (Tdie)";
                                        }

                                        if (hardware.Name.Contains("Intel"))
                                        {
                                            cpuTempString = "CPU Package";
                                        }

                                        else
                                        {
                                            cpuTempString = "Core #0";
                                        }

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
                                                    if (value.Contains(cpuTempString))
                                                    {
                                                        string resultPackage = value.Substring(Math.Max(0, value.Length - 2));
                                                        if (!resultPackage.Contains("#"))
                                                        {
                                                            string dataValue = resultPackage.ToString() + "c";
                                                            string type = "t";
                                                            ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocCpuTempMini);
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
                                                        ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocCpuLoadMini);
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
                    //check if MSIAfterburner process is running
                    if (SettingsSDMonitor.CheckForAB() == true)
                    {
                        msiAB.Connect();
                        isABRunning = true;
                    }
                    else
                    {
                        msiAB = null;
                    }

                    //define Librehardwaremonitor sensors and connect (CPU temp data requires 'highestAvailable' requestedExecutionLevel !!)
                    ohmComputer.Open();

                    //set static header images 
                    ImageManager.SetStaticHeaders();

                    //check if using animations or static images
                    string currentProfile = SharedSettings.config.Read("selectedProfile", "Current_Profile");
                    if (SharedSettings.IsAnimationEnabled(currentProfile) != "True")
                    {
                        foreach (var button in SettingsSDMonitor.BgButtonList())
                        {
                            ImageManager.SetStaticImg(SettingsSDMonitor.imageName, button);
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
                                    var framerateEntry = msiAB.GetEntry(HardwareMonitor.GPU_GLOBAL_INDEX, MONITORING_SOURCE_ID.FRAMERATE);
                                    msiAB.ReloadEntry(framerateEntry);

                                    //get values for framerate and pass to process
                                    int framerateInt = (int)Math.Round(framerateEntry.Data);
                                    string dataValue = framerateInt.ToString();
                                    string type = "f";
                                    ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocFps);
                                }

                                else
                                {
                                    string dataValue = "0";
                                    string type = "f";
                                    ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocFps);
                                }

                                //get and set time 
                                string timeOutput = DateTime.Now.ToString("hh:mm");

                                if (timeOutput.StartsWith("0"))
                                {
                                    timeOutput = timeOutput.Remove(0, 1);
                                }
                                ImageManager.ProcessValueImg(timeOutput, "ti", SettingsSDMonitor.KeyLocTimeHeader);

                                //search hardware
                                foreach (IHardware hardware in ohmComputer.Hardware)
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
                                                ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocGpuTemp);
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
                                                        ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocGpuLoad);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //check for cpu sensor
                                    if (hardware.HardwareType == HardwareType.CPU)
                                    {
                                        string cpuTempString;

                                        if (hardware.Name.Contains("Ryzen"))
                                        {
                                            cpuTempString = "Core (Tdie)";
                                        }

                                        if (hardware.Name.Contains("Intel"))
                                        {
                                            cpuTempString = "CPU Package";
                                        }

                                        else
                                        {
                                            cpuTempString = "Core #0";
                                        }

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
                                                    if (value.Contains(cpuTempString))
                                                    {
                                                        string resultPackage = value.Substring(Math.Max(0, value.Length - 2));
                                                        if (!resultPackage.Contains("#"))
                                                        {
                                                            string dataValue = resultPackage.ToString() + "c";
                                                            string type = "t";
                                                            ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocCpuTemp);
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
                                                        ImageManager.ProcessValueImg(dataValue, type, SettingsSDMonitor.KeyLocCpuLoad);
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
            }
        }

        //key pressed
        public static void DeckKeyPressed(object sender, OpenMacroBoard.SDK.KeyEventArgs e)
        {
            var clockButton = 0;
            var monitorButton = 4;
            var exitButton = 7;

            if (isMiniDeck == true)
            {
                monitorButton = 2;
                exitButton = 4;
            }

            try
            {
                if (e.Key == clockButton)
                {
                    if (e.IsDown == true)
                    {
                        //write State to config file then start MonitorState
                        SharedSettings.config.Write("seletedState", "2", "Current_State");
                        //restart to show new state
                        RestartApp();
                    }
                }

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
                        if (isMiniDeck)
                        {
                            ImageManager.exitflag = true;
                            ImageManager.deck.ShowLogo();

                            //close StreamDeckMonitor
                            ExitApp();
                        }

                        else
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
            }

            catch (Exception)
            {
                ExitApp();
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
