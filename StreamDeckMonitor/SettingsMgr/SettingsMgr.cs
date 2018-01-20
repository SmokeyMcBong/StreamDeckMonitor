namespace StreamDeckMonitor
{
    class SettingsMgr
    {
        /*  StreamDeck button layout for reference ...        
               -key locations-  
                4  3  2  1  0    
                9  8  7  6  5    
                14 13 12 11 10           
        */

        //define font settings file and store user font preferences
        static IniParser FontIni = new IniParser("FontSettings.ini");
        public static string FontTypeHeaders
        {
            get { return FontIni.Read("fontType", "Font_Headers"); }
        }
        public static string FontColorHeaders
        {
            get { return FontIni.Read("fontColor", "Font_Headers"); }
        }
        public static string FontTypeValues
        {
            get { return FontIni.Read("fontType", "Font_Values"); }
        }
        public static string FontColorValues
        {
            get { return FontIni.Read("fontColor", "Font_Values"); }
        }

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
            get { return ("img\\cpu.png"); }
        }
        public static string Gpu_pngLocation
        {
            get { return ("img\\gpu.png"); }
        }
        public static string Fps_pngLocation
        {
            get { return ("img\\fps.png"); }
        }
        public static string Temp_pngLocation
        {
            get { return ("img\\temp.png"); }
        }
        public static string Load_pngLocation
        {
            get { return ("img\\load.png"); }
        }
        public static string Time_pngLocation
        {
            get { return ("img\\time.png"); }
        }

        //define temporary image locations
        public static string F_pngLocation
        {
            get { return ("img\\f.png"); }
        }
        public static string T_pngLocation
        {
            get { return ("img\\t.png"); }
        }
        public static string L_pngLocation
        {
            get { return ("img\\l.png"); }
        }
        public static string Ti_pngLocation
        {
            get { return ("img\\ti.png"); }
        }
    }
}
