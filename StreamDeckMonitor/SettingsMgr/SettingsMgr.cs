using System.Drawing;
using System.Drawing.Text;
using System.Linq;

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
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1) System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        //define resource directory locations
        public static string fontDir = "fonts\\";
        public static string imgDir = "img\\";

        //define font settings file and store user font preferences
        static IniParser FontIni = new IniParser("FontSettings.ini");
        static PrivateFontCollection myFonts;

        //define the FontFamily
        public static FontFamily LoadFontFamily(string fileName, out PrivateFontCollection myFonts)
        {
            myFonts = new PrivateFontCollection();
            myFonts.AddFontFile(fileName);
            return myFonts.Families[0];
        }

        //store all user font settings
        public static string header_font = FontIni.Read("fontType", "Font_Headers");
        public static string value_font = FontIni.Read("fontType", "Font_Values");

        public static string FontColor_Headers
        {
            get { return FontIni.Read("fontColor", "Font_Headers"); }
        }

        public static string FontColor_Values
        {
            get { return FontIni.Read("fontColor", "Font_Values"); }
        }

        public static string BackgroundFill_Color
        {
            get { return FontIni.Read("backgroundColor", "Background_Color"); }
        }

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
        public static int Keylocation_blankimage1
        {
            get { return 1; }
        }
        public static int Keylocation_blankimage2
        {
            get { return 9; }
        }
        public static int Keylocation_blankimage3
        {
            get { return 5; }
        }
        public static int Keylocation_blankimage4
        {
            get { return 3; }
        }
        public static int Keylocation_blankimage5
        {
            get { return 14; }
        }
        public static int Keylocation_blankimage6
        {
            get { return 10; }
        }
        public static int Keylocation_blankimage7
        {
            get { return 2; }
        }

        //define template image locations
        public static string Cpu_pngLocation
        {
            get { return (imgDir + "cpu.png"); }
        }
        public static string Gpu_pngLocation
        {
            get { return (imgDir + "gpu.png"); }
        }
        public static string Fps_pngLocation
        {
            get { return (imgDir + "fps.png"); }
        }
        public static string Temp_pngLocation
        {
            get { return (imgDir + "temp.png"); }
        }
        public static string Load_pngLocation
        {
            get { return (imgDir + "load.png"); }
        }
        public static string Time_pngLocation
        {
            get { return (imgDir + "time.png"); }
        }

        //define temporary image locations
        public static string F_pngLocation
        {
            get { return (imgDir + "f.png"); }
        }
        public static string T_pngLocation
        {
            get { return (imgDir + "t.png"); }
        }
        public static string L_pngLocation
        {
            get { return (imgDir + "l.png"); }
        }
        public static string Ti_pngLocation
        {
            get { return (imgDir + "ti.png"); }
        }
    }
}