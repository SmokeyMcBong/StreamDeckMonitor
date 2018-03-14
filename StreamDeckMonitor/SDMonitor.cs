using MSI.Afterburner;
using OpenHardwareMonitor.Hardware;
using StreamDeckSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamDeckMonitor
{
    class SDMonitor
    {
        static void Main(string[] args)
        {
            //make sure only one instance is running
            SettingsMgr.CheckForTwins();

            //MSI Afterburner MACM shared memory
            HardwareMonitor mahm = new HardwareMonitor();

            //define openhardwaremonitor sensors and connect (CPU temp data requires 'highestAvailable' requestedExecutionLevel !!)
            Computer computer = new Computer() { CPUEnabled = true, GPUEnabled = true };
            computer.Open();

            //clean display and set brightness
            ImageMgr.deck.ClearKeys();
            var displayBrightness = Convert.ToByte(SettingsMgr.displayBrightness);
            ImageMgr.deck.SetBrightness(displayBrightness);

            //create all necessary header template images 
            ImageMgr.ProcessHeaderImages();

            //set if using animations or static images
            if (SettingsMgr.AnimCheck() == 0)
            {
                ImageMgr.SetStaticImg("cpu", SettingsMgr.KeyLocCpuHeader);
                ImageMgr.SetStaticImg("gpu", SettingsMgr.KeyLocGpuHeader);
                ImageMgr.SetStaticImg(SettingsMgr.imageName, SettingsMgr.KeyLocBgImg1);
                ImageMgr.SetStaticImg(SettingsMgr.imageName, SettingsMgr.KeyLocBgImg2);
                ImageMgr.SetStaticImg(SettingsMgr.imageName, SettingsMgr.KeyLocBgImg3);
                ImageMgr.SetStaticImg(SettingsMgr.imageName, SettingsMgr.KeyLocBgImg4);
                ImageMgr.SetStaticImg(SettingsMgr.imageName, SettingsMgr.KeyLocBgImg5);
                ImageMgr.SetStaticImg(SettingsMgr.imageName, SettingsMgr.KeyLocBgImg6);
                ImageMgr.SetStaticImg(SettingsMgr.imageName, SettingsMgr.KeyLocBgImg7);

                //start standard loop without the image animations 
                StartMe();
            }
            else
            {
                ImageMgr.SetStaticImg("cpu", SettingsMgr.KeyLocCpuHeader);
                ImageMgr.SetStaticImg("gpu", SettingsMgr.KeyLocGpuHeader);

                //start both loops in parallel
                Parallel.Invoke(() => ImageMgr.StartAnimation(), () => StartMe());
            }

            void StartMe()
            {
                //start loop
                while (true)
                {
                    //add key press handler, if pressed send exit command 
                    ImageMgr.deck.KeyPressed += DeckKeyPressed;

                    //connect to msi afterburner and reload values
                    var framerateEntry = mahm.GetEntry(HardwareMonitor.GPU_GLOBAL_INDEX, MONITORING_SOURCE_ID.FRAMERATE);
                    mahm.Connect();
                    mahm.ReloadEntry(framerateEntry);

                    try
                    {
                        //get values for framerate and pass to process
                        int framerateInt = (int)Math.Round(framerateEntry.Data);
                        string dataValue = framerateInt.ToString();
                        string type = "f";
                        ImageMgr.ProcessValueImg(dataValue, type, SettingsMgr.KeyLocFps);
                    }

                    finally
                    {
                        //get and set time 
                        string timeOutput = DateTime.Now.ToString("hh:mm");

                        if (timeOutput.StartsWith("0"))
                        {
                            timeOutput = timeOutput.Remove(0, 1);
                        }

                        ImageMgr.ProcessValueImg(timeOutput, "ti", SettingsMgr.KeyLocTimeHeader);
                    }

                    //search hardware
                    foreach (IHardware hardware in computer.Hardware)
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
                                    ImageMgr.ProcessValueImg(dataValue, type, SettingsMgr.KeyLocGpuTemp);
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
                                            ImageMgr.ProcessValueImg(dataValue, type, SettingsMgr.KeyLocGpuLoad);
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
                                                ImageMgr.ProcessValueImg(dataValue, type, SettingsMgr.KeyLocCpuTemp);
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
                                            ImageMgr.ProcessValueImg(dataValue, type, SettingsMgr.KeyLocCpuLoad);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //wait 1 second before restarting loop
                    System.Threading.Thread.Sleep(1000);

                    //close msi afturburner monitoring connection
                    mahm.Disconnect();

                    //remove handler
                    ImageMgr.deck.KeyPressed -= DeckKeyPressed;
                }
            }

            //check for button input
            void DeckKeyPressed(object sender, StreamDeckKeyEventArgs e)
            {
                //clean display and set brightness back to rough default of 60%
                ImageMgr.deck.ClearKeys();
                ImageMgr.deck.SetBrightness(60);

                //close monitoring connections
                mahm.Disconnect();
                computer.Close();

                //exit
                Environment.Exit(Environment.ExitCode);
            }
        }
    }
}