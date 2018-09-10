#  ![alt text](https://i.imgur.com/qPAlSRq.png "StreamDeckMonitor") StreamDeckMonitor
  
##### Customizable C# app to display Real-Time System Statistics on the  [Elgato Stream Deck](https://www.elgato.com/en/gaming/stream-deck) Device
---

[![Latest Version](https://img.shields.io/github/release/SmokeyMcBong/StreamDeckMonitor.svg)](https://github.com/SmokeyMcBong/StreamDeckMonitor/releases) 

##### Table of Contents:
[About StreamDeckMonitor](https://github.com/SmokeyMcBong/StreamDeckMonitor#about-streamdeckmonitor)  
[Installation](https://github.com/SmokeyMcBong/StreamDeckMonitor#installation)  
[Customization](https://github.com/SmokeyMcBong/StreamDeckMonitor#customization)  

---

###### This project uses...
* StreamDeckSharp .NET interface (https://github.com/OpenStreamDeck/StreamDeckSharp)  
* MSI Afterburner .NET Class Library (https://forums.guru3d.com/threads/msi-afterburner-net-class-library.339656) 
* LibreHardwareMonitorLib interface (https://github.com/librehardwaremonitor)
* Accord .NET Framework video frame capture libraries (https://github.com/accord-net/framework)
---

  ![](https://i.imgur.com/vl4t6N8.gif) ![](https://i.imgur.com/obFUxFh.gif) 
 
---

##### About StreamDeckMonitor:
- Now supports both the standard Stream Deck and the Stream Deck Mini devices
- Runs as non-visible console application in the background
- Automatically gets the following data every second...  
    * Framerate Data,    
    * CPU Temperature,   
    * CPU Load,   
    * GPU Temperature,   
    * GPU Load,  
    * Current Time.   
- Full Digital Clock 
  - Press the Clock button (Top Left button) to view
  - Press the FPS button (Top Right button) to go back to StreamDeckMonitor
- Fully close and exit by pressing the Middle button (Middle button, Bottom row if using Stream Deck Mini)
- Customizable using the 'Configurator' application
- Lightweight code
#


---


##### Installation:
- Download the [Latest Release](https://github.com/SmokeyMcBong/StreamDeckMonitor/releases) of StreamDeckMonitor
- Unzip the entire zip file to a folder somewhere
- Run StreamDeckMonitor.exe to start the application
- Run Configurator.exe to open the bundled customization application
 
[ ** For the framerate counter option to work, the necessary [MSI Afterburner Application](http://www.guru3d.com/files-details/msi-afterburner-beta-download.html) must be running ]
#
---


##### Customization:
- To Add own Fonts, Static Images and Animations..
    * Add custom Font .ttf's to the following folder.. '\Customize\Fonts'
    * Add custom Static Image .png's (72x72) to the following folder.. '\Customize\Static Images'
    * Add custom Animation .mp4's to the following folder.. '\Customize\Animations'  
    ** The frames taken from the .mp4 video start from the very first frame ! **
    
  Stream Deck Standard (15 buttons) Options ...![](https://i.imgur.com/nXdv7jZ.png)  

  Stream Deck Mini (6 buttons) Options ...![](https://i.imgur.com/qhEFNs1.png)

- Using the Configurator Application we can set...
    * Font type, Font color, Font Size and Height Position for:
      - Title header Text,
      - Value Headers Text,
      - Data Value Text,
      - Clock Time,
      - Clock Animated Colon,
      - Clock Date
    * Set Background color of the images,
    * Option to use either static images or animations for the surrounding images,
    * Change the animations source or static image source,
    * Set animation framerate (max 60 fps),
    * Set animation total frame amount (max 600 frames),
    * Set the Display brightness.
    * View full digital clock or compact digital clock
    * Show or hide the Date
    * Choose which Stream Deck Device to show options for (This will show once on the first start, double click the 'Device' label in the top right to change devices)
    * Full Save/Load Profile support
    * Load default settings option

#
---
###### This project is NOT related to *Elgato Systems GmbH* in any way
---
 
