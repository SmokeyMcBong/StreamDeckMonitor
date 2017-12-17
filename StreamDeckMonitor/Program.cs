﻿using MSI.Afterburner;
using OpenHardwareMonitor.Hardware;
using StreamDeckSharp;
using System;
using System.Collections.Generic;

namespace StreamDeckMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            /////////////////////////////
            ////   -key locations-   ////
            ////    4  3  2  1  0    ////
            ////    9  8  7  6  5    ////
            ////   14 13 12 11 10    ////
            /////////////////////////////

            //Set key locations
            var key_location_framerate = 0;
            var key_location_cpu_temp = 8;
            var key_location_gpu_temp = 7;
            var key_location_cpu_load = 13;
            var key_location_gpu_load = 12;
            var key_location_header_temp = 9;
            var key_location_header_load = 14;
            var key_location_header_cpu = 3;
            var key_location_header_gpu = 2;
            var key_location_blank1 = 4;
            var key_location_blank2 = 1;
            var key_location_blank3 = 6;
            var key_location_blank4 = 5;
            var key_location_blank5 = 11;
            var key_location_blank6 = 10;

            //Set directory locations
            string fps = "fps_images\\";
            string temp = "temp_images\\";
            string load = "load_images\\";

            // add CPU and GPU as hardware
            // note that, CPU temperature data requires 'highestAvailable' permission.
            Computer computer = new Computer() { CPUEnabled = true, GPUEnabled = true };
            computer.Open();

            //Open the Stream Deck device
            using (var deck = StreamDeck.FromHID())
            {
                //Set loop
                while (true)
                {
                    //Set static images
                    Set_Value("", "temp", key_location_header_temp);
                    Set_Value("", "load", key_location_header_load);
                    Set_Value("", "cpu", key_location_header_cpu);
                    Set_Value("", "gpu", key_location_header_gpu);
                    Set_Value("", "blank", key_location_blank1);
                    Set_Value("", "blank", key_location_blank2);
                    Set_Value("", "blank", key_location_blank3);
                    Set_Value("", "blank", key_location_blank4);
                    Set_Value("", "blank", key_location_blank5);
                    Set_Value("", "blank", key_location_blank6);

                    try
                    {
                        //Connect to MSI Afterburner MACM shared memory
                        HardwareMonitor mahm = new HardwareMonitor();
                        //Get monitoring data
                        HardwareMonitorEntry framerate = mahm.GetEntry(HardwareMonitor.GPU_GLOBAL_INDEX, MONITORING_SOURCE_ID.FRAMERATE);

                        //Get values for framerate and pass to Set_Value to process
                        int framerateInt = (int)Math.Round(framerate.Data);

                        //Limit fps data to deal with to144fps
                        if (framerateInt < 145)
                        {
                            Set_Value(fps, framerateInt.ToString(), key_location_framerate);
                        }

                    }
                    finally { }

                    //Search hardware
                    foreach (IHardware hardware in computer.Hardware)
                    {
                        hardware.Update();

                        //Check for gpu sensor
                        if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAti)
                        {
                            foreach (ISensor sensor in hardware.Sensors)
                            {
                                //Search for temp sensor
                                if (sensor.SensorType == SensorType.Temperature)
                                {
                                    //Limit fps data to deal with to 99c
                                    if (sensor.Value < 100)
                                    {
                                        Set_Value(temp, sensor.Value.ToString(), key_location_gpu_temp);
                                    }
                                }

                                //Search for load sensor
                                if (sensor.SensorType == SensorType.Load)
                                {
                                    //Add gpu load sensors to list
                                    string get_values = sensor.Name + sensor.Value.ToString();
                                    List<string> value_list = new List<string>
                                {
                                    get_values
                                };
                                    //Get values for gpu and pass to Set_Value to process
                                    foreach (string value in value_list)
                                    {
                                        if (value.Contains("GPU Core"))
                                        {
                                            //Get values for gpu and pass to Set_Value to process
                                            int gpuloadInt = (int)Math.Round(sensor.Value.Value);

                                            //Limit gpu load data to deal with to 99%
                                            if (gpuloadInt < 100)
                                            {
                                                Set_Value(load, gpuloadInt.ToString(), key_location_gpu_load);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Check for cpu sensor
                        if (hardware.HardwareType == HardwareType.CPU)
                        {
                            foreach (ISensor sensor in hardware.Sensors)
                            {
                                if (sensor.SensorType == SensorType.Temperature)
                                {
                                    //Add cpu temp sensors to list
                                    string get_values = sensor.Name + sensor.Value.ToString();
                                    List<string> value_list = new List<string>
                                {
                                    get_values
                                };
                                    //Get values for cpu and pass to Set_Value to process
                                    foreach (string value in value_list)
                                    {
                                        if (value.Contains("Core #1"))
                                        {
                                            string result_package = value.Substring(Math.Max(0, value.Length - 2));
                                            if (!result_package.Contains("#"))
                                            {
                                                Set_Value(temp, result_package, key_location_cpu_temp);
                                            }
                                        }
                                    }
                                }
                                //Search for load sensor
                                if (sensor.SensorType == SensorType.Load)
                                {
                                    //Add cpu load sensors to list
                                    string get_values = sensor.Name + sensor.Value.ToString();
                                    List<string> value_list = new List<string>
                                {
                                    get_values
                                };
                                    //Get values for cpu and change Stream Deck image
                                    foreach (string value in value_list)
                                    {
                                        if (value.Contains("CPU Total"))
                                        {
                                            //Get values for cpu and pass to Set_Value to process
                                            int cpuloadInt = (int)Math.Round(sensor.Value.Value);

                                            //Limit gpu load data to deal with to 99%
                                            if (cpuloadInt < 100)
                                            {
                                                Set_Value(load, cpuloadInt.ToString(), key_location_cpu_load);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Wait 1 second before restarting loop
                    System.Threading.Thread.Sleep(1000);

                    //Check for key presses, if pressed send exit command 
                    deck.KeyPressed += Deck_KeyPressed;

                    void Set_Value(string type, string value, int key_location)
                    {
                        if (!type.Equals(null))
                        {
                            //Set corresponding images to the stored data values
                            string value_icon = value + ".png";
                            var value_bitmap = StreamDeckKeyBitmap.FromFile("monitor_images\\" + type + value_icon);
                            deck.SetKeyBitmap(key_location, value_bitmap);
                        }
                    }

                    void Deck_KeyPressed(object sender, StreamDeckKeyEventArgs e)
                    {
                        //Kill console App
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}