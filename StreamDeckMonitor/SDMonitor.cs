using MSI.Afterburner;
using OpenHardwareMonitor.Hardware;
using StreamDeckSharp;
using System;
using System.Collections.Generic;

namespace StreamDeckMonitor
{
    class SDMonitor
    {
        static void Main(string[] args)
        {
            //define openhardwaremonitor sensors (CPU temp data requires 'highestAvailable' requestedExecutionLevel !!)
            Computer computer = new Computer() { CPUEnabled = true, GPUEnabled = true };
            computer.Open();

            //create all necessary header template images 
            ImageMgr.Process_Header_Images();

            //set static images
            ImageMgr.Set_Static("cpu", SettingsMgr.Keylocation_cpu_header);
            ImageMgr.Set_Static("gpu", SettingsMgr.Keylocation_gpu_header);
            ImageMgr.Set_Static("blank", SettingsMgr.Keylocation_blankimage1);
            ImageMgr.Set_Static("blank", SettingsMgr.Keylocation_blankimage2);
            ImageMgr.Set_Static("blank", SettingsMgr.Keylocation_blankimage3);
            ImageMgr.Set_Static("blank", SettingsMgr.Keylocation_blankimage4);
            ImageMgr.Set_Static("blank", SettingsMgr.Keylocation_blankimage5);
            ImageMgr.Set_Static("blank", SettingsMgr.Keylocation_blankimage6);
            ImageMgr.Set_Static("blank", SettingsMgr.Keylocation_blankimage7);

            //start loop
            while (true)
            {
                try
                {
                    //connect to MSI Afterburner MACM shared memory
                    HardwareMonitor mahm = new HardwareMonitor();
                    //Get monitoring data
                    HardwareMonitorEntry framerate = mahm.GetEntry(HardwareMonitor.GPU_GLOBAL_INDEX, MONITORING_SOURCE_ID.FRAMERATE);

                    //get values for framerate and pass to Set_Value to process
                    int framerateInt = (int)Math.Round(framerate.Data);
                    string dataValue = framerateInt.ToString();
                    string type = "f";
                    ImageMgr.Set_Value(dataValue, type, SettingsMgr.Keylocation_framerate);
                }

                finally
                {
                    //get and set time 
                    string time_output = DateTime.Now.ToString("hh:mm");

                    //check if hours start with "0"
                    if (time_output.StartsWith("0"))
                    {
                        //remove starting "0" from string
                        time_output = time_output.Remove(0, 1);
                    }

                    ImageMgr.Set_Value(time_output, "ti", SettingsMgr.Keylocation_time_header);
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
                                ImageMgr.Set_Value(dataValue, type, SettingsMgr.Keylocation_gpu_temp);
                            }

                            //search for load sensor
                            if (sensor.SensorType == SensorType.Load)
                            {
                                //add gpu load sensors to list
                                string get_values = sensor.Name + sensor.Value.ToString();
                                List<string> value_list = new List<string>
                                {
                                    get_values
                                };
                                //get values for gpu and pass to Set_Value to process
                                foreach (string value in value_list)
                                {
                                    if (value.Contains("GPU Core"))
                                    {
                                        //get values for gpu and pass to Set_Value to process
                                        int gpuloadInt = (int)Math.Round(sensor.Value.Value);
                                        string dataValue = gpuloadInt.ToString() + "%";
                                        string type = "l";
                                        ImageMgr.Set_Value(dataValue, type, SettingsMgr.Keylocation_gpu_load);
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
                            if (sensor.SensorType == SensorType.Temperature)
                            {
                                //add cpu temp sensors to list
                                string get_values = sensor.Name + sensor.Value.ToString();
                                List<string> value_list = new List<string>
                                {
                                    get_values
                                };
                                //get values for cpu and pass to Set_Value to process
                                foreach (string value in value_list)
                                {
                                    if (value.Contains("Core #1"))
                                    {
                                        string result_package = value.Substring(Math.Max(0, value.Length - 2));
                                        if (!result_package.Contains("#"))
                                        {
                                            string dataValue = result_package.ToString() + "c";
                                            string type = "t";
                                            ImageMgr.Set_Value(dataValue, type, SettingsMgr.Keylocation_cpu_temp);
                                        }
                                    }
                                }
                            }
                            //search for load sensor
                            if (sensor.SensorType == SensorType.Load)
                            {
                                //add cpu load sensors to list
                                string get_values = sensor.Name + sensor.Value.ToString();
                                List<string> value_list = new List<string>
                                {
                                    get_values
                                };
                                //get values for cpu and change Stream Deck image
                                foreach (string value in value_list)
                                {
                                    if (value.Contains("CPU Total"))
                                    {
                                        //get values for cpu and pass to Set_Value to process
                                        int cpuloadInt = (int)Math.Round(sensor.Value.Value);
                                        string dataValue = cpuloadInt.ToString() + "%";
                                        string type = "l";
                                        ImageMgr.Set_Value(dataValue, type, SettingsMgr.Keylocation_cpu_load);
                                    }
                                }
                            }
                        }
                    }
                }

                //wait 1 second before restarting loop
                System.Threading.Thread.Sleep(1000);

                //check for key presses, if pressed send exit command 
                StreamDeck.FromHID().KeyPressed += Deck_KeyPressed;
            }

            //check for button input
            void Deck_KeyPressed(object sender, StreamDeckKeyEventArgs e)
            {
                //kill running App
                Environment.Exit(0);
            }
        }
    }
}