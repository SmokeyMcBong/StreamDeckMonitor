#  ![alt text](https://i.imgur.com/qPAlSRq.png "StreamDeckMonitor") StreamDeckMonitor
  
##### A Simple C# app to display Real-Time System Statistics on the  [Elgato Stream Deck](https://www.elgato.com/en/gaming/stream-deck) Device
---

#
##### Table of Contents:
[Dependencies](https://github.com/SmokeyMcBong/StreamDeckMonitor#this-project-uses)  
[About StreamDeckMonitor](https://github.com/SmokeyMcBong/StreamDeckMonitor#streamdeckmonitor)  
[StreamDeckMonitor Font Customization Walkthrough](https://github.com/SmokeyMcBong/StreamDeckMonitor#customization)  

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
- Fully customizable font
'FontSettings.ini' file to change the Font Types, Colors and Sizes, 
Ability to use Any .ttf font files for headers or data value fonts.
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
- Using the FontSettings.ini file we can set the font type, color and size of the headers and the data value fonts
- To use ANY other custom .ttf font, the file must be added to the fonts folder and the fontType name value in FontSettings.ini changed to the new font file name 

#
Here's a quick look at the FontSettings.ini file with a breakdown of what each values changes ...

   ![](https://i.imgur.com/ieWWp5H.jpg)
   ![](https://i.imgur.com/uH1BFg1.jpg)
   
#
---
 
###### This project is not related to *Elgato Systems GmbH* in any way

---
 
