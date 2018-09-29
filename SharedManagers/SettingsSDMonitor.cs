using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace SharedManagers
{
    class SettingsSDMonitor
    {
        //check for Layout key count
        private static readonly int deckKeyCount = ImageManager.deck.Keys.Count();
        public static string CheckForLayout()
        {
            string deckLayout = "";

            if (deckKeyCount == 6)
            {
                deckLayout = "Mini";
            }

            if (deckKeyCount == 15)
            {
                deckLayout = "Standard";
            }

            return deckLayout;
        }

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
        private static readonly string headerFont1 = SharedSettings.HeaderFont1(currentProfile);
        private static readonly string headerFont2 = SharedSettings.HeaderFont2(currentProfile);
        private static readonly string valueFont = SharedSettings.ValueFont(currentProfile);
        private static readonly string headerFontColor1 = SharedSettings.HeaderFontColor1(currentProfile);
        private static readonly string headerFontColor2 = SharedSettings.HeaderFontColor2(currentProfile);
        private static readonly string valuesFontColor = SharedSettings.ValuesFontColor(currentProfile);
        public static string headerFont1Position = SharedSettings.HeaderFont1Position(currentProfile);
        public static string headerFont2Position = SharedSettings.HeaderFont2Position(currentProfile);
        public static string valuesFontPosition = SharedSettings.ValuesFontPosition(currentProfile);
        public static int headerFontSize1 = int.Parse(SharedSettings.HeaderFontSize1(currentProfile).ToString());
        public static int headerFontSize2 = int.Parse(SharedSettings.HeaderFontSize2(currentProfile).ToString());
        public static int valueFontSize = int.Parse(SharedSettings.ValueFontSize(currentProfile).ToString());
        private static readonly string backgroundFillColor = SharedSettings.BackgroundFillColor(currentProfile);
        private static readonly string isAnimationEnabled = SharedSettings.IsAnimationEnabled(currentProfile);
        private static readonly int animFramerate = int.Parse(SharedSettings.AnimFramerate(currentProfile).ToString());
        public static int displayBrightness = int.Parse(SharedSettings.DisplayBrightness(currentProfile).ToString());
        public static int framesToProcess = int.Parse(SharedSettings.FramesToProcess(currentProfile).ToString());
        public static string imageName = SharedSettings.ImageName(currentProfile);
        public static string animName = SharedSettings.AnimName(currentProfile);

        //clock settings
        public static string timeFont = SharedSettings.TimeFontType();
        public static string colonFont = SharedSettings.ColonFontType();
        public static string dateFont = SharedSettings.DateFontType();
        public static int timeFontSize = int.Parse(SharedSettings.TimeFontSize().ToString());
        public static int colonFontSize = int.Parse(SharedSettings.ColonFontSize().ToString());
        public static int dateFontSize = int.Parse(SharedSettings.DateFontSize().ToString());
        public static string timeFontColor = SharedSettings.TimeFontColor();
        public static string colonFontColor = SharedSettings.ColonFontColor();
        public static string dateFontColor = SharedSettings.DateFontColor();
        public static string clockBackgroundColor = SharedSettings.ClockBackgroundColor();
        public static int timePosition = int.Parse(SharedSettings.TimePosition().ToString());
        public static int colonPosition = int.Parse(SharedSettings.ColonPosition().ToString());
        public static int datePosition = int.Parse(SharedSettings.DatePosition().ToString());

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

        //mini settings
        public static string isFpsCounter = SharedSettings.IsFpsCounter();

        //colored SolidBrushes
        private static readonly SolidBrush cyanBrush = new SolidBrush(Color.FromArgb(4, 232, 232));
        private static readonly SolidBrush brownBrush = new SolidBrush(Color.FromArgb(153, 76, 0));
        private static readonly SolidBrush orangeBrush = new SolidBrush(Color.FromArgb(255, 128, 0));
        private static readonly SolidBrush pinkBrush = new SolidBrush(Color.FromArgb(255, 0, 255));
        private static readonly SolidBrush yellowBrush = new SolidBrush(Color.FromArgb(255, 255, 51));
        private static readonly SolidBrush goldBrush = new SolidBrush(Color.FromArgb(255, 215, 0));
        private static readonly SolidBrush greenBrush = new SolidBrush(Color.FromArgb(0, 153, 0));
        private static readonly SolidBrush limeBrush = new SolidBrush(Color.FromArgb(0, 255, 0));
        private static readonly SolidBrush blackBrush = new SolidBrush(Color.FromArgb(0, 0, 0));
        private static readonly SolidBrush whiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
        private static readonly SolidBrush blueBrush = new SolidBrush(Color.FromArgb(0, 102, 204));
        private static readonly SolidBrush purpleBrush = new SolidBrush(Color.FromArgb(76, 0, 153));
        private static readonly SolidBrush redBrush = new SolidBrush(Color.FromArgb(215, 0, 0));
        private static readonly SolidBrush silverBrush = new SolidBrush(Color.FromArgb(192, 192, 192));
        private static readonly SolidBrush greyBrush = new SolidBrush(Color.FromArgb(128, 128, 128));
        private static readonly SolidBrush salmonBrush = new SolidBrush(Color.FromArgb(250, 128, 114));
        private static readonly SolidBrush khakiBrush = new SolidBrush(Color.FromArgb(240, 230, 140));
        private static readonly SolidBrush oliveBrush = new SolidBrush(Color.FromArgb(128, 128, 0));
        private static readonly SolidBrush tealBrush = new SolidBrush(Color.FromArgb(0, 128, 128));
        private static readonly SolidBrush skyBlueBrush = new SolidBrush(Color.FromArgb(135, 206, 235));
        private static readonly SolidBrush beigeBrush = new SolidBrush(Color.FromArgb(207, 182, 155));
        private static readonly SolidBrush mintBrush = new SolidBrush(Color.FromArgb(152, 255, 152));
        private static readonly SolidBrush honeyDewBrush = new SolidBrush(Color.FromArgb(171, 217, 171));

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

        public static SolidBrush ColonBrush
        {
            get
            {
                SetBrush(colonFontColor);
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
        public static FontFamily myFontColon = LoadFontFamily(SharedSettings.fontDir + colonFont + ".ttf", out myFonts);
        public static FontFamily myFontDate = LoadFontFamily(SharedSettings.fontDir + dateFont + ".ttf", out myFonts);

        /*  StreamDeck button layout for reference ...     
         *  
           -key locations(Standard)-  
              0   1   2   3   4
              5   6   7   8   9
             10  11  12  13  14       
             
            -key locations(Mini)-  
                 0   1   2 
                 3   4   5    
       */

        //define Mini key locations        
        public static int KeyLocFpsMini
        {
            get { return 3; }
        }

        public static int KeyLocCpuHeaderMini
        {
            get { return 0; }
        }

        public static int KeyLocGpuHeaderMini
        {
            get { return 3; }
        }

        public static int KeyLocCpuTempMini
        {
            get { return 1; }
        }

        public static int KeyLocGpuTempMini
        {
            get { return 4; }
        }

        public static int KeyLocCpuLoadMini
        {
            get { return 2; }
        }

        public static int KeyLocGpuLoadMini
        {
            get { return 5; }
        }

        //define key locations
        public static int KeyLocFps
        {
            get { return 4; }
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
            get { return 8; }
        }

        public static int KeyLocGpuLoad
        {
            get { return 13; }
        }

        public static int KeyLocCpuHeader
        {
            get { return 6; }
        }

        public static int KeyLocGpuHeader
        {
            get { return 11; }
        }

        public static int KeyLocTimeHeader
        {
            get { return 0; }
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

        //static and animated button list
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
