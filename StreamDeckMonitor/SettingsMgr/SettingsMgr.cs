using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StreamDeckMonitor
{
    class SettingsMgr
    {
        /*  when starting this app using the StreamDeck, occasionally the button press can be registered more than
            once in a very short amount of time, resulting in multiple instances of StreamDeckMonitor all starting and running at the same time.
            the CheckForTwins() method makes sure that only one instance of StreamDeckMonitor runs
        */

        //check for duplicate instances
        public static void CheckForTwins()
        {
            if (System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count() > 1) System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        //define resource directory locations
        private static string absoluteRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).ToString();
        private static string fontDir = absoluteRoot + "/fonts/";
        public static string imgDir = absoluteRoot + "/img/";
        public static string frameDir = absoluteRoot + "/img/_/frames/";
        public static string generatedDir = absoluteRoot + "/img/_/generated/";

        //define settings file
        private static IniParser fontIni = new IniParser("Settings.ini");
        private static PrivateFontCollection myFonts;

        //define the FontFamily
        private static FontFamily LoadFontFamily(string fileName, out PrivateFontCollection myFonts)
        {
            myFonts = new PrivateFontCollection();
            myFonts.AddFontFile(fileName);
            return myFonts.Families[0];
        }

        //store settings from ini file
        private static string headerFont = fontIni.Read("fontType", "Font_Headers");
        private static string valueFont = fontIni.Read("fontType", "Font_Values");
        private static string fontColorHeaders = fontIni.Read("fontColor", "Font_Headers");
        private static string fontColorValues = fontIni.Read("fontColor", "Font_Values");
        private static string backgroundFillColor = fontIni.Read("backgroundColor", "Background_Color");
        private static string isAnimationEnabled = fontIni.Read("animationEnabled", "Animated_Keys");
        private static int animFramerate = int.Parse(fontIni.Read("animationFramerate", "Animated_Keys"));

        //check if animations are enabled
        public static int AnimCheck()
        {
            int isEnabled = 0;
            if (isAnimationEnabled == "True" || isAnimationEnabled == "true")
            {
                isEnabled = 1;
            }
            return isEnabled;
        }

        //check for animation framerate setting
        public static int FrametimeValue()
        {
            //calculate frametime value using framerate setting
            int frametimeValue = 1000 / animFramerate;
            decimal.Truncate(frametimeValue);

            //limit max framerate to 100fps
            if (animFramerate <= 100)
            {
                return frametimeValue;
            }
            //if setting is above 100 or invalid then set to 30fps default
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
        private static SolidBrush beigeBrush = new SolidBrush(Color.FromArgb(245, 245, 220));
        private static SolidBrush mintBrush = new SolidBrush(Color.FromArgb(245, 255, 250));
        private static SolidBrush honeyDewBrush = new SolidBrush(Color.FromArgb(240, 255, 240));

        //set font color brushes
        private static SolidBrush myBrush;
        public static SolidBrush HeaderBrush
        {
            get
            {
                SetBrush(fontColorHeaders);
                return myBrush;
            }
        }
        public static SolidBrush ValuesBrush
        {
            get
            {
                SetBrush(fontColorValues);
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

        //store font size from ini settings file
        public static int header1FontSize = int.Parse(fontIni.Read("fontSizeHeader_1", "Font_Sizes"));
        public static int header2FontSize = int.Parse(fontIni.Read("fontSizeHeader_2", "Font_Sizes"));
        public static int valueFontSize = int.Parse(fontIni.Read("fontSizeValues", "Font_Sizes"));

        //set font family
        public static FontFamily myFontHeaders = LoadFontFamily(fontDir + headerFont + ".ttf", out myFonts);
        public static FontFamily myFontValues = LoadFontFamily(fontDir + valueFont + ".ttf", out myFonts);

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

        //define template image locations
        public static string ImageLocCpu
        {
            get { return (generatedDir + "cpu.png"); }
        }
        public static string ImageLocGpu
        {
            get { return (generatedDir + "gpu.png"); }
        }
        public static string ImageLocFps
        {
            get { return (generatedDir + "fps.png"); }
        }
        public static string ImageLocTemp
        {
            get { return (generatedDir + "temp.png"); }
        }
        public static string ImageLocLoad
        {
            get { return (generatedDir + "load.png"); }
        }
        public static string ImageLocTime
        {
            get { return (generatedDir + "time.png"); }
        }

        //define temporary image locations
        public static string TempImageLocFps
        {
            get { return (generatedDir + "f.png"); }
        }
        public static string TempImageLocTemp
        {
            get { return (generatedDir + "t.png"); }
        }
        public static string TempImageLocLoad
        {
            get { return (generatedDir + "l.png"); }
        }
        public static string TempImageLocTime
        {
            get { return (generatedDir + "ti.png"); }
        }
    }
}
