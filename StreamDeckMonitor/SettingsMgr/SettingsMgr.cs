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

        /*  StreamDeck button layout for reference ...        
                -key locations-  
                 4  3  2  1  0    
                 9  8  7  6  5    
                 14 13 12 11 10           
       */

        //check for duplicate instances
        public static void CheckForTwins()
        {
            if (System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count() > 1) System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        //define resource directory locations
        public static string absoluteRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).ToString();
        public static string fontDir = absoluteRoot + "/fonts/";
        public static string imgDir = absoluteRoot + "/img/";
        public static string frameDir = absoluteRoot + "/img/frames/";
        public static string generatedDir = absoluteRoot + "/img/generated/";

        //define settings file
        static IniParser FontIni = new IniParser("Settings.ini");
        static PrivateFontCollection myFonts;

        //define the FontFamily
        public static FontFamily LoadFontFamily(string fileName, out PrivateFontCollection myFonts)
        {
            myFonts = new PrivateFontCollection();
            myFonts.AddFontFile(fileName);
            return myFonts.Families[0];
        }

        //store settings from ini file
        static string header_font = FontIni.Read("fontType", "Font_Headers");
        static string value_font = FontIni.Read("fontType", "Font_Values");
        static string FontColor_Headers = FontIni.Read("fontColor", "Font_Headers");
        static string FontColor_Values = FontIni.Read("fontColor", "Font_Values");
        static string BackgroundFill_Color = FontIni.Read("backgroundColor", "Background_Color");
        static string isAnimationEnabled = FontIni.Read("animationEnabled", "Animated_Keys");

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

        //colored SolidBrushes
        static SolidBrush CyanBrush = new SolidBrush(Color.FromArgb(4, 232, 232));
        static SolidBrush BrownBrush = new SolidBrush(Color.FromArgb(153, 76, 0));
        static SolidBrush OrangeBrush = new SolidBrush(Color.FromArgb(255, 128, 0));
        static SolidBrush PinkBrush = new SolidBrush(Color.FromArgb(255, 0, 255));
        static SolidBrush YellowBrush = new SolidBrush(Color.FromArgb(255, 255, 51));
        static SolidBrush GoldBrush = new SolidBrush(Color.FromArgb(255, 215, 0));
        static SolidBrush GreenBrush = new SolidBrush(Color.FromArgb(0, 153, 0));
        static SolidBrush LimeBrush = new SolidBrush(Color.FromArgb(0, 255, 0));
        static SolidBrush BlackBrush = new SolidBrush(Color.FromArgb(0, 0, 0));
        static SolidBrush WhiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
        static SolidBrush BlueBrush = new SolidBrush(Color.FromArgb(0, 102, 204));
        static SolidBrush PurpleBrush = new SolidBrush(Color.FromArgb(76, 0, 153));
        static SolidBrush RedBrush = new SolidBrush(Color.FromArgb(215, 0, 0));
        static SolidBrush SilverBrush = new SolidBrush(Color.FromArgb(192, 192, 192));
        static SolidBrush GreyBrush = new SolidBrush(Color.FromArgb(128, 128, 128));
        static SolidBrush SalmonBrush = new SolidBrush(Color.FromArgb(250, 128, 114));
        static SolidBrush KhakiBrush = new SolidBrush(Color.FromArgb(240, 230, 140));
        static SolidBrush OliveBrush = new SolidBrush(Color.FromArgb(128, 128, 0));
        static SolidBrush TealBrush = new SolidBrush(Color.FromArgb(0, 128, 128));
        static SolidBrush SkyBlueBrush = new SolidBrush(Color.FromArgb(135, 206, 235));
        static SolidBrush BeigeBrush = new SolidBrush(Color.FromArgb(245, 245, 220));
        static SolidBrush MintBrush = new SolidBrush(Color.FromArgb(245, 255, 250));
        static SolidBrush HoneyDewBrush = new SolidBrush(Color.FromArgb(240, 255, 240));

        //set font color brushes
        static SolidBrush myBrush;
        public static SolidBrush HeaderBrush
        {
            get
            {
                SetBrush(FontColor_Headers);
                return myBrush;
            }
        }
        public static SolidBrush ValuesBrush
        {
            get
            {
                SetBrush(FontColor_Values);
                return myBrush;
            }
        }
        public static SolidBrush BackgroundBrush
        {
            get
            {
                SetBrush(BackgroundFill_Color);
                return myBrush;
            }
        }

        //assign colored brush
        static void SetBrush(string color)
        {
            if (color.Equals("Cyan"))
            {
                myBrush = CyanBrush;
            }
            if (color.Equals("Brown"))
            {
                myBrush = BrownBrush;
            }
            if (color.Equals("Orange"))
            {
                myBrush = OrangeBrush;
            }
            if (color.Equals("Pink"))
            {
                myBrush = PinkBrush;
            }
            if (color.Equals("Yellow"))
            {
                myBrush = YellowBrush;
            }
            if (color.Equals("Gold"))
            {
                myBrush = GoldBrush;
            }
            if (color.Equals("Green"))
            {
                myBrush = GreenBrush;
            }
            if (color.Equals("Lime"))
            {
                myBrush = LimeBrush;
            }
            if (color.Equals("Black"))
            {
                myBrush = BlackBrush;
            }
            if (color.Equals("White"))
            {
                myBrush = WhiteBrush;
            }
            if (color.Equals("Blue"))
            {
                myBrush = BlueBrush;
            }
            if (color.Equals("Purple"))
            {
                myBrush = PurpleBrush;
            }
            if (color.Equals("Red"))
            {
                myBrush = RedBrush;
            }
            if (color.Equals("Silver"))
            {
                myBrush = SilverBrush;
            }
            if (color.Equals("Grey"))
            {
                myBrush = GreyBrush;
            }
            if (color.Equals("Salmon"))
            {
                myBrush = SalmonBrush;
            }
            if (color.Equals("Khaki"))
            {
                myBrush = KhakiBrush;
            }
            if (color.Equals("Olive"))
            {
                myBrush = OliveBrush;
            }
            if (color.Equals("Teal"))
            {
                myBrush = TealBrush;
            }
            if (color.Equals("SkyBlue"))
            {
                myBrush = SkyBlueBrush;
            }
            if (color.Equals("Beige"))
            {
                myBrush = BeigeBrush;
            }
            if (color.Equals("Mint"))
            {
                myBrush = MintBrush;
            }
            if (color.Equals("HoneyDew"))
            {
                myBrush = HoneyDewBrush;
            }
        }

        //store font size from ini settings file
        public static int header1_font_size = int.Parse(FontIni.Read("fontSizeHeader_1", "Font_Sizes"));
        public static int header2_font_size = int.Parse(FontIni.Read("fontSizeHeader_2", "Font_Sizes"));
        public static int value_font_size = int.Parse(FontIni.Read("fontSizeValues", "Font_Sizes"));

        //set font family
        public static FontFamily myFontFamily_Headers = LoadFontFamily(fontDir + header_font + ".ttf", out myFonts);
        public static FontFamily myFontFamily_Values = LoadFontFamily(fontDir + value_font + ".ttf", out myFonts);

        //define key locations
        public static int Keylocation_framerate
        {
            get { return 0; }
        }
        public static int Keylocation_cpu_temp
        {
            get { return 7; }
        }
        public static int Keylocation_gpu_temp
        {
            get { return 12; }
        }
        public static int Keylocation_cpu_load
        {
            get { return 6; }
        }
        public static int Keylocation_gpu_load
        {
            get { return 11; }
        }
        public static int Keylocation_cpu_header
        {
            get { return 8; }
        }
        public static int Keylocation_gpu_header
        {
            get { return 13; }
        }
        public static int Keylocation_time_header
        {
            get { return 4; }
        }
        public static int Keylocation_bgimage1
        {
            get { return 1; }
        }
        public static int Keylocation_bgimage2
        {
            get { return 9; }
        }
        public static int Keylocation_bgimage3
        {
            get { return 5; }
        }
        public static int Keylocation_bgimage4
        {
            get { return 3; }
        }
        public static int Keylocation_bgimage5
        {
            get { return 14; }
        }
        public static int Keylocation_bgimage6
        {
            get { return 10; }
        }
        public static int Keylocation_bgimage7
        {
            get { return 2; }
        }

        //define template image locations
        public static string Cpu_pngLocation
        {
            get { return (generatedDir + "cpu.png"); }
        }
        public static string Gpu_pngLocation
        {
            get { return (generatedDir + "gpu.png"); }
        }
        public static string Fps_pngLocation
        {
            get { return (generatedDir + "fps.png"); }
        }
        public static string Temp_pngLocation
        {
            get { return (generatedDir + "temp.png"); }
        }
        public static string Load_pngLocation
        {
            get { return (generatedDir + "load.png"); }
        }
        public static string Time_pngLocation
        {
            get { return (generatedDir + "time.png"); }
        }

        //define temporary image locations
        public static string F_pngLocation
        {
            get { return (generatedDir + "f.png"); }
        }
        public static string T_pngLocation
        {
            get { return (generatedDir + "t.png"); }
        }
        public static string L_pngLocation
        {
            get { return (generatedDir + "l.png"); }
        }
        public static string Ti_pngLocation
        {
            get { return (generatedDir + "ti.png"); }
        }
    }
}
