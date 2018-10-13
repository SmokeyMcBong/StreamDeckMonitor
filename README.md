#  ![alt text](https://i.imgur.com/qPAlSRq.png "StreamDeckMonitor") StreamDeckMonitor
  
##### Customizable C# app to display Real-Time System Statistics on the  [Elgato Stream Deck](https://www.elgato.com/en/gaming/stream-deck) Device

---

[![Latest Version](https://img.shields.io/github/release/SmokeyMcBong/StreamDeckMonitor.svg)](https://github.com/SmokeyMcBong/StreamDeckMonitor/releases) 

##### Table of Contents:
[Project Dependencies](https://github.com/SmokeyMcBong/StreamDeckMonitor#this-project-uses-)  
[Demo Videos](https://github.com/SmokeyMcBong/StreamDeckMonitor#demo-videos)  
[About StreamDeckMonitor](https://github.com/SmokeyMcBong/StreamDeckMonitor#about-streamdeckmonitor)  
[Installation](https://github.com/SmokeyMcBong/StreamDeckMonitor#installation)  
[Customization](https://github.com/SmokeyMcBong/StreamDeckMonitor#customization)  

---

##### Project Dependencies:
* StreamDeckSharp .NET interface (https://github.com/OpenStreamDeck/StreamDeckSharp)  
* MSI Afterburner .NET Class Library (https://forums.guru3d.com/threads/msi-afterburner-net-class-library.339656) 
* LibreHardwareMonitorLib interface (https://github.com/librehardwaremonitor)
* Accord .NET Framework video frame capture libraries (https://github.com/accord-net/framework)

###### and to make the post-build stuff a little easier ...
* Choose32 (http://www.westmesatech.com/editv.html)
* 7zip (https://www.7-zip.org/)

---

##### Demo Videos:
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

---

##### Installation:
- Download the [Latest Release](https://github.com/SmokeyMcBong/StreamDeckMonitor/releases) of StreamDeckMonitor
- Unzip the entire zip file to a folder somewhere
- Run StreamDeckMonitor.exe to start the application
- Run Configurator.exe to open the bundled customization application
 
[ ** For the framerate counter option to show the current game's FPS, the necessary [MSI Afterburner Application](https://www.guru3d.com/files-details/msi-afterburner-beta-download.html) must be running ]

---

##### Customization:
- Using the Configurator Application we can set...
    * Title Headers :: Font Type, Font Color, Font Size and Height Position,
    * Value Headers :: Font Type, Font Color, Font Size and Height Position,
    * Data Values :: Font Type, Font Color, Font Size and Height Position,
    * Clock Time :: Font Type, Font Color, Font Size and Height Position,
    * Clock Animated Colon :: Font Type, Font Color, Font Size and Height Position,
    * Clock Date :: Font Type, Font Color, Font Size and Height Position
    * Set Background Color of the Images,
    * Option to use either Static Images or Animations for the Surrounding Images,
    * Change the Animations Source or Static Image Source,
    * Set Animation Framerate (max 60 fps),
    * Set Animation Total Frame Amount (max 600 frames),
    * Set the Display Brightness.
    * View Full Digital Clock or Compact Digital Clock
    * Show or Hide the Date
    * Choose which Stream Deck Device to show options for (This will show once on the first start, double click the 'Selected Device' label in the top right to change devices)
    * Full Save/Load Profile support
    * Load Default Settings option
#
- To Add own Fonts, Static Images and Animations..
    * Add custom Font .ttf's to the following folder.. '\Customize\Fonts'
    * Add custom Static Image .png's (72x72) to the following folder.. '\Customize\Static Images'
    * Add custom Animation .mp4's to the following folder.. '\Customize\Animations'  
    ** The frames taken from the .mp4 video start from the very first frame ! **
#
######  StreamDeckMonitor Configurator Options ...![](https://i.imgur.com/ezUu0dN.png) ![](https://i.imgur.com/JVLqmBH.png) 
#

---
###### This project is NOT related to *Elgato Systems GmbH* in any way
---
