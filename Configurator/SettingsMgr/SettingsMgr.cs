using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Configurator
{
    class SettingsMgr
    {
        //define resource directory locations
        private static string absoluteRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).ToString();
        private static string fontDir = absoluteRoot + "/fonts/";
        private static string imgDir = absoluteRoot + "/img/";
        private static string frameDir = absoluteRoot + "/img/_/frames/";
        private static string generatedDir = absoluteRoot + "/img/_/generated/";

        //define settings file
        public static ConfigParser config = new ConfigParser("sdm.cfg");

        public static string headerFont;
        public static string valueFont;
        public static string fontColorHeaders;
        public static string fontColorValues;
        public static string backgroundFillColor;
        public static string animFramerate;
        private static string isAnimationEnabled;

        public static int isEnabled;
        public static int header1FontSize;
        public static int header2FontSize;
        public static int valueFontSize;

        public static List<string> fontList;
        public static List<string> colorList;

        public static void LoadValues()
        {
            headerFont = config.Read("fontType", "Font_Headers");
            valueFont = config.Read("fontType", "Font_Values");
            fontColorHeaders = config.Read("fontColor", "Font_Headers");
            fontColorValues = config.Read("fontColor", "Font_Values");
            backgroundFillColor = config.Read("backgroundColor", "Background_Color");
            header1FontSize = int.Parse(config.Read("fontSizeHeader_1", "Font_Sizes"));
            header2FontSize = int.Parse(config.Read("fontSizeHeader_2", "Font_Sizes"));
            valueFontSize = int.Parse(config.Read("fontSizeValues", "Font_Sizes"));
            animFramerate = int.Parse(config.Read("animationFramerate", "Animated_Keys")).ToString();
            isAnimationEnabled = config.Read("animationEnabled", "Animated_Keys");

            isEnabled = 0;
            if (isAnimationEnabled == "True" || isAnimationEnabled == "true")
            {
                isEnabled = 1;
            }

            //create file list of fonts in fontDir
            fontList = new List<string> { };
            foreach (var font in Directory.GetFiles(fontDir))
            {
                fontList.Add((Path.GetFileNameWithoutExtension(font)));
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
}
