using System.Diagnostics;
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
        public static int DeckDevice()
        {
            return int.Parse(config.Read("selectedDevice", "StreamDeck_Device"));
        }

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

        public static string HeaderFont1Position(string currentProfile)
        {
            return config.Read("headerFontPosition_1", currentProfile);
        }

        public static string HeaderFont2Position(string currentProfile)
        {
            return config.Read("headerFontPosition_2", currentProfile);
        }

        public static string ValuesFontPosition(string currentProfile)
        {
            return config.Read("valuesFontPosition", currentProfile);
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

        public static string ColonFontType()
        {
            return config.Read("colonFontType", "Clock_Settings");
        }

        public static string DateFontType()
        {
            return config.Read("dateFontType", "Clock_Settings");
        }

        public static int TimeFontSize()
        {
            return int.Parse(config.Read("timeFontSize", "Clock_Settings"));
        }

        public static int ColonFontSize()
        {
            return int.Parse(config.Read("colonFontSize", "Clock_Settings"));
        }

        public static int DateFontSize()
        {
            return int.Parse(config.Read("dateFontSize", "Clock_Settings"));
        }

        public static string TimeFontColor()
        {
            return config.Read("timeFontColor", "Clock_Settings");
        }

        public static string ColonFontColor()
        {
            return config.Read("colonFontColor", "Clock_Settings");
        }

        public static string DateFontColor()
        {
            return config.Read("dateFontColor", "Clock_Settings");
        }

        public static string TimePosition()
        {
            return config.Read("timeFontPosition", "Clock_Settings");
        }

        public static string ColonPosition()
        {
            return config.Read("colonFontPosition", "Clock_Settings");
        }

        public static string DatePosition()
        {
            return config.Read("dateFontPosition", "Clock_Settings");
        }

        public static string IsCompactView()
        {
            return config.Read("compactView", "Clock_Settings");
        }

        public static string IsDateShown()
        {
            return config.Read("showDate", "Clock_Settings");
        }

        //mini settings
        public static string IsFpsCounter()
        {
            return config.Read("showFpsCounter", "Mini_Settings");
        }
    }
}