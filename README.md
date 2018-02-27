#  ![alt text](https://i.imgur.com/qPAlSRq.png "StreamDeckMonitor") StreamDeckMonitor
  
##### Customizable C# app to display Real-Time System Statistics on the  [Elgato Stream Deck](https://www.elgato.com/en/gaming/stream-deck) Device
---

[![Latest Version](https://img.shields.io/github/release/SmokeyMcBong/StreamDeckMonitor.svg)](https://github.com/SmokeyMcBong/StreamDeckMonitor/releases) 

##### Table of Contents:
[Dependencies](https://github.com/SmokeyMcBong/StreamDeckMonitor#this-project-uses)  
[About StreamDeckMonitor](https://github.com/SmokeyMcBong/StreamDeckMonitor#about-streamdeckmonitor)  
[Customization](https://github.com/SmokeyMcBong/StreamDeckMonitor#customization)  

---

###### This project uses...
* StreamDeckSharp .NET interface (https://github.com/OpenStreamDeck/StreamDeckSharp)  
* MSI Afterburner .NET Class Library (https://forums.guru3d.com/threads/msi-afterburner-net-class-library.339656) 
* OpenHardwareMonitorLib interface (https://github.com/openhardwaremonitor)
* Accord .NET Framework video frame capture libraries (https://github.com/accord-net/framework)
---

  ![](https://i.imgur.com/vl4t6N8.gif)
 
---

##### About StreamDeckMonitor:
- Runs as non-visible console application in the background
- Automatically gets the following data every second...  
    * Framerate Data,    
    * CPU Temperature,   
    * CPU Load,   
    * GPU Temperature,   
    * GPU Load,  
    * Current Time.   
- Customizable using the 'Configurator' application
- Closes itself when any button is pressed on the Stream Deck
- Lightweight code
#


---


##### Customization:
- To Add own Fonts, Static Images and Animations..
    * Add custom Fonts .ttf's to the following folder.. '\Customize\Fonts'
    * Add custom Static Image .png's (72x72) to the following folder.. '\Customize\Static Images'
    * Add custom Animation .mp4's to the following folder.. '\Customize\Fonts'  
    ** The frames taken from the .mp4 video start from the very first frame ! **
    
  ![](https://i.imgur.com/MQz8FZ1.png)  

- Using the Configurator Application we can set...
    * Font type, color and size of both title headers and data values,
    * Set Background color of the images,
    * Option to use either static images or animations for the surrounding images,
    * Change the animations source or static image source,
    * Set animation framerate (max 100 fps),
    * Set animation total frame amount (max 600 frames),
    * Display brightness.
- Full Save/Load Profile support. 

#
---
###### This project is NOT related to *Elgato Systems GmbH* in any way
---
 
