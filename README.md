#  ![alt text](https://i.imgur.com/qPAlSRq.png "StreamDeckMonitor") StreamDeckMonitor
  
##### Customizable C# app to display Real-Time System Statistics on the  [Elgato Stream Deck](https://www.elgato.com/en/gaming/stream-deck) Device
---

[![Latest Version](https://img.shields.io/github/release/SmokeyMcBong/StreamDeckMonitor.svg)](https://github.com/SmokeyMcBong/StreamDeckMonitor/releases) [![Download Count](https://img.shields.io/github/downloads/SmokeyMcBong/StreamDeckMonitor/latest/total.svg)](https://github.com/SmokeyMcBong/StreamDeckMonitor/releases)

##### Table of Contents:
[Dependencies](https://github.com/SmokeyMcBong/StreamDeckMonitor#this-project-uses)  
[About StreamDeckMonitor](https://github.com/SmokeyMcBong/StreamDeckMonitor#streamdeckmonitor)  
[Font Customization Guide](https://github.com/SmokeyMcBong/StreamDeckMonitor#customization)  

---
#

###### This project uses
* StreamDeckSharp .NET interface (https://github.com/OpenStreamDeck/StreamDeckSharp)  
* MSI Afterburner .NET Class Library (https://forums.guru3d.com/threads/msi-afterburner-net-class-library.339656/) 
* OpenHardwareMonitorLib interface (https://github.com/openhardwaremonitor)
---

 ![](https://i.imgur.com/5shefdi.jpg)
 
---

##### StreamDeckMonitor
- Runs as non-visible console application in the background
- Fully customizable font and background color
- 'FontSettings.ini' file to change the Font Types, Colors, Sizes and background color
- Ability to use Any .ttf font files for headers or values fonts.
- Automatically gets the following data every second  
Framerate Data,   
CPU Temperature,   
CPU Load,   
GPU Temperature,   
GPU Load,  
Current Time.   
- Closes itself when any button is pressed on the Stream Deck
- Lightweight code
#

---

##### Customization
- Using the FontSettings.ini file we can set the font type, color and size of the headers and the data value fonts and also the background color
- To use ANY other custom .ttf font, the file must be added to the fonts folder and the fontType name value in FontSettings.ini changed to the new font file name 
#

###### Some quick examples using the custom font options in FontSettings.ini ...
![](https://i.imgur.com/6jyt54d.png?1) ![](https://i.imgur.com/exgHk4k.png?2) 
![](https://i.imgur.com/K3pcqO7.png?1) ![](https://i.imgur.com/W0vcnuq.png?1) 

---
# 





##### Here's a quick look at the FontSettings.ini file with a breakdown of what each values changes ...

###### Font Type and Color Settings ...
   ![](https://i.imgur.com/atrAuUu.jpg)
   
---  
###### Font Size Settings ...
   ![](https://i.imgur.com/o3R2jzb.jpg)
   
#
#
---
 
###### This project is not related to *Elgato Systems GmbH* in any way

---
 
