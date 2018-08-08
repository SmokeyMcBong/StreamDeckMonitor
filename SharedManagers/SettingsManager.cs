using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SharedManagers
{
    class SharedSettings
    {
        //check for duplicate instances
        public static void CheckForTwins()
        {
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        //resource directory locations
        public static string absoluteRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static string fontDir = absoluteRoot + "/Customize/Fonts/";
        public static string staticImgDir = absoluteRoot + "/Customize/Static Images/";
        public static string animationImgDir = absoluteRoot + "/Customize/Animations/";
        public static string frameDir = absoluteRoot + "/ext/_/frames/";
        public static string generatedDir = absoluteRoot + "/ext/_/generated/";

        //config file 
        public static string configFile = absoluteRoot + "/ext/cfg/SharedConfig.cfg";
        public static ConfigParser config = new ConfigParser(configFile);

        //config file values
        public static int CurrentSate()
        {
            return int.Parse(config.Read("seletedState", "Current_State"));
        }
        public static string CompactView()
        {
            return config.Read("compactView", "Clock_Settings");
        }
        public static string ShowDate()
        {
            return config.Read("showDate", "Clock_Settings");
        }
        public static string HeaderFont1(string currentProfile)
        {
            return config.Read("headerFontType_1", currentProfile);
        }
        public static string HeaderFont2(string currentProfile)
        {
            return config.Read("headerFontType_2", currentProfile);
        }
        public static string ValueFont(string currentProfile)
        {
            return config.Read("valuesFontType", currentProfile);
        }
        public static string HeaderFontColor1(string currentProfile)
        {
            return config.Read("headerfontColor_1", currentProfile);
        }
        public static string HeaderFontColor2(string currentProfile)
        {
            return config.Read("headerfontColor_2", currentProfile);
        }
        public static string ValuesFontColor(string currentProfile)
        {
            return config.Read("valuesFontColor", currentProfile);
        }
        public static string BackgroundFillColor(string currentProfile)
        {
            return config.Read("backgroundColor", currentProfile);
        }
        public static int HeaderFontSize1(string currentProfile)
        {
            return int.Parse(config.Read("headerFontSize_1", currentProfile));
        }
        public static int HeaderFontSize2(string currentProfile)
        {
            return int.Parse(config.Read("headerFontSize_2", currentProfile));
        }
        public static int ValueFontSize(string currentProfile)
        {
            return int.Parse(config.Read("valuesFontSize", currentProfile));
        }
        public static int DisplayBrightness(string currentProfile)
        {
            return int.Parse(config.Read("displayBrightness", currentProfile));
        }
        public static int FramesToProcess(string currentProfile)
        {
            return int.Parse(config.Read("framesToProcess", currentProfile));
        }
        public static int AnimFramerate(string currentProfile)
        {
            return int.Parse(config.Read("animationFramerate", currentProfile));
        }
        public static string ImageName(string currentProfile)
        {
            return config.Read("imageName", currentProfile);
        }
        public static string AnimName(string currentProfile)
        {
            return config.Read("animName", currentProfile);
        }
        public static string IsAnimationEnabled(string currentProfile)
        {
            return config.Read("animationEnabled", currentProfile);
        }
        //clock settings
        public static string TimeFontType()
        {
            return config.Read("timeFontType", "Clock_Settings");
        }
        public static string DateFontType()
        {
            return config.Read("dateFontType", "Clock_Settings");
        }
        public static int TimeFontSize()
        {
            return int.Parse(config.Read("timeFontSize", "Clock_Settings"));
        }
        public static int DateFontSize()
        {
            return int.Parse(config.Read("dateFontSize", "Clock_Settings"));
        }
        public static string TimeFontColor()
        {
            return config.Read("timeFontColor", "Clock_Settings");
        }
        public static string DateFontColor()
        {
            return config.Read("dateFontColor", "Clock_Settings");
        }
        public static string ClockBackgroundColor()
        {
            return config.Read("clockBackgroundColor", "Clock_Settings");
        }
        public static string IsCompactView()
        {
            return config.Read("compactView", "Clock_Settings");
        }
        public static string IsDateShown()
        {
            return config.Read("showDate", "Clock_Settings");
        }
    }

    class SettingsManagerConfig
    {
        public static string headerFont1;
        public static string headerFont2;
        public static string valueFont;
        public static string headerFontColor1;
        public static string headerFontColor2;
        public static string valuesFontColor;
        public static string backgroundFillColor;
        public static int headerFontSize1;
        public static int headerFontSize2;
        public static int valueFontSize;
        public static int animFramerate;
        public static string imageName;
        public static string animName;
        private static string isAnimationEnabled;
        public static int framesToProcess;
        public static int displayBrightness;
        //clock settings
        public static string timeFont;
        public static string dateFont;
        public static int timeFontSize;
        public static int dateFontSize;
        public static string timeFontColor;
        public static string dateFontColor;
        public static string clockBackgroundColor;
        public static string isCompactView;
        public static string isDateShown;

        //set font size, framerate and frame amount max values
        public static int fsMax = 99;
        public static int frMax = 60;
        public static int faMax = 600;

        public static List<string> fontList;
        public static List<string> colorList;
        public static List<string> imageList;
        public static List<string> animList;
        public static List<string> profileList;

        public static void LoadValues(string currentProfile)
        {
            headerFont1 = SharedSettings.HeaderFont1(currentProfile);
            headerFont2 = SharedSettings.HeaderFont2(currentProfile);
            valueFont = SharedSettings.ValueFont(currentProfile);
            headerFontColor1 = SharedSettings.HeaderFontColor1(currentProfile);
            headerFontColor2 = SharedSettings.HeaderFontColor2(currentProfile);
            valuesFontColor = SharedSettings.ValuesFontColor(currentProfile);
            backgroundFillColor = SharedSettings.BackgroundFillColor(currentProfile);
            headerFontSize1 = int.Parse(SharedSettings.HeaderFontSize1(currentProfile).ToString());
            headerFontSize2 = int.Parse(SharedSettings.HeaderFontSize2(currentProfile).ToString());
            valueFontSize = int.Parse(SharedSettings.ValueFontSize(currentProfile).ToString());
            displayBrightness = int.Parse(SharedSettings.DisplayBrightness(currentProfile).ToString());
            framesToProcess = int.Parse(SharedSettings.FramesToProcess(currentProfile).ToString());
            animFramerate = int.Parse(SharedSettings.AnimFramerate(currentProfile).ToString());
            imageName = SharedSettings.ImageName(currentProfile);
            animName = SharedSettings.AnimName(currentProfile);
            isAnimationEnabled = SharedSettings.IsAnimationEnabled(currentProfile);
            //clock settings
            timeFont = SharedSettings.TimeFontType();
            dateFont = SharedSettings.DateFontType();
            timeFontSize = int.Parse(SharedSettings.TimeFontSize().ToString());
            dateFontSize = int.Parse(SharedSettings.DateFontSize().ToString());
            timeFontColor = SharedSettings.TimeFontColor();
            dateFontColor = SharedSettings.DateFontColor();
            clockBackgroundColor = SharedSettings.ClockBackgroundColor();
            isCompactView = SharedSettings.IsCompactView();
            isDateShown = SharedSettings.IsDateShown();

            //create file list of fonts in fontDir
            fontList = new List<string> { };
            foreach (var font in Directory.GetFiles(SharedSettings.fontDir))
            {
                fontList.Add((Path.GetFileNameWithoutExtension(font)));
            }

            //create file list of images in staticImgDir
            imageList = new List<string> { };
            foreach (var img in Directory.GetFiles(SharedSettings.staticImgDir))
            {
                if (img.Contains(".png"))
                {
                    imageList.Add((Path.GetFileNameWithoutExtension(img)));
                }
            }

            //create file list of videos in animationImgDir
            animList = new List<string> { };
            foreach (var vid in Directory.GetFiles(SharedSettings.animationImgDir))
            {
                if (vid.Contains(".mp4"))
                {
                    animList.Add((Path.GetFileNameWithoutExtension(vid)));
                }
            }

            //create list of available StreamDeckMonitor colors
            string[] colors = { "Beige", "Black", "Blue", "Brown", "Cyan", "Gold", "Green", "Grey",
                "HoneyDew", "Khaki", "Lime", "Mint", "Olive", "Orange", "Pink", "Purple",
                "Red", "Salmon", "Silver", "SkyBlue", "Teal", "White", "Yellow" };

            colorList = new List<string> { };
            foreach (var color in colors)
            {
                colorList.Add(color);
            }

            //create list of profiles
            string[] profiles = { "Profile 1", "Profile 2", "Profile 3" };

            profileList = new List<string> { };
            foreach (var profile in profiles)
            {
                profileList.Add(profile);
            }
        }

        public static void RestartSDM()
        {
            //if StreamDeckMonitor is running then refresh/restart the application to show new changes made
            string processName = "StreamDeckMonitor";
            if (Process.GetProcessesByName(processName).Length > 0)
            {
                Process.GetProcessesByName(processName).First().Kill();
                Process.Start(processName);
            }
        }
    }

    class SettingsManagerSDM
    {
        //check if MSIAfterburner is running 
        private static readonly string ab = "MSIAfterburner";
        public static bool CheckForAB()
        {
            bool abRunning = false;
            if (IsProcessRunning(ab) == true)
            {
                abRunning = true;
            }
            return abRunning;
        }

        //running process checker
        static bool IsProcessRunning(string processName)
        {
            bool isRunning = false;

            if (processName == ab)
            {
                Process[] proc = Process.GetProcessesByName(processName);
                if (proc.Length > 0)
                {
                    isRunning = true;
                }
            }

            return isRunning;
        }

        //define the FontFamily
        private static PrivateFontCollection myFonts;
        private static FontFamily LoadFontFamily(string fileName, out PrivateFontCollection myFonts)
        {
            myFonts = new PrivateFontCollection();
            myFonts.AddFontFile(fileName);
            return myFonts.Families[0];
        }

        //store settings from ini file
        public static string currentProfile = SharedSettings.config.Read("selectedProfile", "Current_Profile");
        private static string headerFont1 = SharedSettings.HeaderFont1(currentProfile);
        private static string headerFont2 = SharedSettings.HeaderFont2(currentProfile);
        private static string valueFont = SharedSettings.ValueFont(currentProfile);
        private static string headerFontColor1 = SharedSettings.HeaderFontColor1(currentProfile);
        private static string headerFontColor2 = SharedSettings.HeaderFontColor2(currentProfile);
        private static string valuesFontColor = SharedSettings.ValuesFontColor(currentProfile);
        public static int headerFontSize1 = int.Parse(SharedSettings.HeaderFontSize1(currentProfile).ToString());
        public static int headerFontSize2 = int.Parse(SharedSettings.HeaderFontSize2(currentProfile).ToString());
        public static int valueFontSize = int.Parse(SharedSettings.ValueFontSize(currentProfile).ToString());
        private static string backgroundFillColor = SharedSettings.BackgroundFillColor(currentProfile);
        private static string isAnimationEnabled = SharedSettings.IsAnimationEnabled(currentProfile);
        private static int animFramerate = int.Parse(SharedSettings.AnimFramerate(currentProfile).ToString());
        public static int displayBrightness = int.Parse(SharedSettings.DisplayBrightness(currentProfile).ToString());
        public static int framesToProcess = int.Parse(SharedSettings.FramesToProcess(currentProfile).ToString());
        public static string imageName = SharedSettings.ImageName(currentProfile);
        public static string animName = SharedSettings.AnimName(currentProfile);
        //clock settings
        public static string timeFont = SharedSettings.TimeFontType();
        public static string dateFont = SharedSettings.DateFontType();
        public static int timeFontSize = int.Parse(SharedSettings.TimeFontSize().ToString());
        public static int dateFontSize = int.Parse(SharedSettings.DateFontSize().ToString());
        public static string timeFontColor = SharedSettings.TimeFontColor();
        public static string dateFontColor = SharedSettings.DateFontColor();
        public static string clockBackgroundColor = SharedSettings.ClockBackgroundColor();

        public static int FrametimeValue()
        {
            //calculate frametime value using framerate setting
            int frametimeValue = 1000 / animFramerate;
            decimal.Truncate(frametimeValue);

            //limit max framerate to 60fps
            if (animFramerate <= 60)
            {
                return frametimeValue;
            }
            //if setting is above 60 or invalid then set to 30fps default
            else
            {
                frametimeValue = 33;
            }
            return frametimeValue;
        }

        //colored SolidBrushes
        private static SolidBrush cyanBrush = new SolidBrush(Color.FromArgb(4, 232, 232));
        private static SolidBrush brownBrush = new SolidBrush(Color.FromArgb(153, 76, 0));
        private static SolidBrush orangeBrush = new SolidBrush(Color.FromArgb(255, 128, 0));
        private static SolidBrush pinkBrush = new SolidBrush(Color.FromArgb(255, 0, 255));
        private static SolidBrush yellowBrush = new SolidBrush(Color.FromArgb(255, 255, 51));
        private static SolidBrush goldBrush = new SolidBrush(Color.FromArgb(255, 215, 0));
        private static SolidBrush greenBrush = new SolidBrush(Color.FromArgb(0, 153, 0));
        private static SolidBrush limeBrush = new SolidBrush(Color.FromArgb(0, 255, 0));
        private static SolidBrush blackBrush = new SolidBrush(Color.FromArgb(0, 0, 0));
        private static SolidBrush whiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
        private static SolidBrush blueBrush = new SolidBrush(Color.FromArgb(0, 102, 204));
        private static SolidBrush purpleBrush = new SolidBrush(Color.FromArgb(76, 0, 153));
        private static SolidBrush redBrush = new SolidBrush(Color.FromArgb(215, 0, 0));
        private static SolidBrush silverBrush = new SolidBrush(Color.FromArgb(192, 192, 192));
        private static SolidBrush greyBrush = new SolidBrush(Color.FromArgb(128, 128, 128));
        private static SolidBrush salmonBrush = new SolidBrush(Color.FromArgb(250, 128, 114));
        private static SolidBrush khakiBrush = new SolidBrush(Color.FromArgb(240, 230, 140));
        private static SolidBrush oliveBrush = new SolidBrush(Color.FromArgb(128, 128, 0));
        private static SolidBrush tealBrush = new SolidBrush(Color.FromArgb(0, 128, 128));
        private static SolidBrush skyBlueBrush = new SolidBrush(Color.FromArgb(135, 206, 235));
        private static SolidBrush beigeBrush = new SolidBrush(Color.FromArgb(207, 182, 155));
        private static SolidBrush mintBrush = new SolidBrush(Color.FromArgb(152, 255, 152));
        private static SolidBrush honeyDewBrush = new SolidBrush(Color.FromArgb(171, 217, 171));

        //set font color brushes
        private static SolidBrush myBrush;
        public static SolidBrush HeaderBrush1
        {
            get
            {
                SetBrush(headerFontColor1);
                return myBrush;
            }
        }
        public static SolidBrush HeaderBrush2
        {
            get
            {
                SetBrush(headerFontColor2);
                return myBrush;
            }
        }
        public static SolidBrush ValuesBrush
        {
            get
            {
                SetBrush(valuesFontColor);
                return myBrush;
            }
        }
        public static SolidBrush BackgroundBrush
        {
            get
            {
                SetBrush(backgroundFillColor);
                return myBrush;
            }
        }
        //clock settings
        public static SolidBrush TimeBrush
        {
            get
            {
                SetBrush(timeFontColor);
                return myBrush;
            }
        }
        public static SolidBrush DateBrush
        {
            get
            {
                SetBrush(dateFontColor);
                return myBrush;
            }
        }

        //assign colored brush
        private static void SetBrush(string color)
        {
            if (color.Equals("Cyan"))
            {
                myBrush = cyanBrush;
            }
            if (color.Equals("Brown"))
            {
                myBrush = brownBrush;
            }
            if (color.Equals("Orange"))
            {
                myBrush = orangeBrush;
            }
            if (color.Equals("Pink"))
            {
                myBrush = pinkBrush;
            }
            if (color.Equals("Yellow"))
            {
                myBrush = yellowBrush;
            }
            if (color.Equals("Gold"))
            {
                myBrush = goldBrush;
            }
            if (color.Equals("Green"))
            {
                myBrush = greenBrush;
            }
            if (color.Equals("Lime"))
            {
                myBrush = limeBrush;
            }
            if (color.Equals("Black"))
            {
                myBrush = blackBrush;
            }
            if (color.Equals("White"))
            {
                myBrush = whiteBrush;
            }
            if (color.Equals("Blue"))
            {
                myBrush = blueBrush;
            }
            if (color.Equals("Purple"))
            {
                myBrush = purpleBrush;
            }
            if (color.Equals("Red"))
            {
                myBrush = redBrush;
            }
            if (color.Equals("Silver"))
            {
                myBrush = silverBrush;
            }
            if (color.Equals("Grey"))
            {
                myBrush = greyBrush;
            }
            if (color.Equals("Salmon"))
            {
                myBrush = salmonBrush;
            }
            if (color.Equals("Khaki"))
            {
                myBrush = khakiBrush;
            }
            if (color.Equals("Olive"))
            {
                myBrush = oliveBrush;
            }
            if (color.Equals("Teal"))
            {
                myBrush = tealBrush;
            }
            if (color.Equals("SkyBlue"))
            {
                myBrush = skyBlueBrush;
            }
            if (color.Equals("Beige"))
            {
                myBrush = beigeBrush;
            }
            if (color.Equals("Mint"))
            {
                myBrush = mintBrush;
            }
            if (color.Equals("HoneyDew"))
            {
                myBrush = honeyDewBrush;
            }
        }

        //set font family
        public static FontFamily myFontHeader1 = LoadFontFamily(SharedSettings.fontDir + headerFont1 + ".ttf", out myFonts);
        public static FontFamily myFontHeader2 = LoadFontFamily(SharedSettings.fontDir + headerFont2 + ".ttf", out myFonts);
        public static FontFamily myFontValues = LoadFontFamily(SharedSettings.fontDir + valueFont + ".ttf", out myFonts);
        //clock settings
        public static FontFamily myFontTime = LoadFontFamily(SharedSettings.fontDir + timeFont + ".ttf", out myFonts);
        public static FontFamily myFontDate = LoadFontFamily(SharedSettings.fontDir + dateFont + ".ttf", out myFonts);

        /*  StreamDeck button layout for reference ...        
                -key locations-  
                 4  3  2  1  0    
                 9  8  7  6  5    
                 14 13 12 11 10           
       */

        //define key locations
        public static int KeyLocFps
        {
            get { return 0; }
        }
        public static int KeyLocCpuTemp
        {
            get { return 7; }
        }
        public static int KeyLocGpuTemp
        {
            get { return 12; }
        }
        public static int KeyLocCpuLoad
        {
            get { return 6; }
        }
        public static int KeyLocGpuLoad
        {
            get { return 11; }
        }
        public static int KeyLocCpuHeader
        {
            get { return 8; }
        }
        public static int KeyLocGpuHeader
        {
            get { return 13; }
        }
        public static int KeyLocTimeHeader
        {
            get { return 4; }
        }
        public static int KeyLocBgImg1
        {
            get { return 1; }
        }
        public static int KeyLocBgImg2
        {
            get { return 9; }
        }
        public static int KeyLocBgImg3
        {
            get { return 5; }
        }
        public static int KeyLocBgImg4
        {
            get { return 3; }
        }
        public static int KeyLocBgImg5
        {
            get { return 14; }
        }
        public static int KeyLocBgImg6
        {
            get { return 10; }
        }
        public static int KeyLocBgImg7
        {
            get { return 2; }
        }

        public static List<int> BgButtonList()
        {
            List<int> buttonList = new List<int>
            {
                KeyLocBgImg1,
                KeyLocBgImg2,
                KeyLocBgImg3,
                KeyLocBgImg4,
                KeyLocBgImg5,
                KeyLocBgImg6,
                KeyLocBgImg7
            };
            return buttonList;
        }

        //define template image locations
        public static string ImageLocCpu
        {
            get { return (SharedSettings.generatedDir + "cpu.png"); }
        }
        public static string ImageLocGpu
        {
            get { return (SharedSettings.generatedDir + "gpu.png"); }
        }
        public static string ImageLocFps
        {
            get { return (SharedSettings.generatedDir + "fps.png"); }
        }
        public static string ImageLocTemp
        {
            get { return (SharedSettings.generatedDir + "temp.png"); }
        }
        public static string ImageLocLoad
        {
            get { return (SharedSettings.generatedDir + "load.png"); }
        }
        public static string ImageLocTime
        {
            get { return (SharedSettings.generatedDir + "time.png"); }
        }
        public static string ImageLocColon
        {
            get { return (SharedSettings.generatedDir + "colon.png"); }
        }
        public static string ImageLocBlank
        {
            get { return (SharedSettings.generatedDir + "blank.png"); }
        }
    }
}