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
        private static string customizeDir = absoluteRoot + "/customize/";
        private static string frameDir = absoluteRoot + "/_/frames/";
        private static string generatedDir = absoluteRoot + "/_/generated/";

        //define settings file
        public static ConfigParser config = new ConfigParser("sdm.cfg");

        public static string headerFont;
        public static string valueFont;
        public static string fontColorHeaders;
        public static string fontColorValues;
        public static string backgroundFillColor;
        public static string animFramerate;
        public static string imageName;
        public static string animName;
        private static string isAnimationEnabled;

        public static int isEnabled;
        public static int header1FontSize;
        public static int header2FontSize;
        public static int valueFontSize;
        public static int framesToProcess;
        public static int displayBrightness;

        public static List<string> fontList;
        public static List<string> colorList;
        public static List<string> imageList;
        public static List<string> animList;
        public static List<string> profileList;

        public static void LoadValues(string currentProfile)
        {
            headerFont = config.Read("headersFontType", currentProfile);
            valueFont = config.Read("valuesFontType", currentProfile);
            fontColorHeaders = config.Read("headersfontColor", currentProfile);
            fontColorValues = config.Read("valuesFontColor", currentProfile);
            backgroundFillColor = config.Read("backgroundColor", currentProfile);
            header1FontSize = int.Parse(config.Read("headersFontSize_1", currentProfile));
            header2FontSize = int.Parse(config.Read("headersFontSize_2", currentProfile));
            valueFontSize = int.Parse(config.Read("valuesFontSize", currentProfile));
            displayBrightness = int.Parse(config.Read("displayBrightness", currentProfile));
            framesToProcess = int.Parse(config.Read("framesToProcess", currentProfile));
            animFramerate = int.Parse(config.Read("animationFramerate", currentProfile)).ToString();
            imageName = config.Read("imageName", currentProfile);
            animName = config.Read("animName", currentProfile);
            isAnimationEnabled = config.Read("animationEnabled", currentProfile);

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

            //create file list of images in customizeDir
            imageList = new List<string> { };
            foreach (var img in Directory.GetFiles(customizeDir))
            {
                if (img.Contains(".png"))
                {
                    imageList.Add((Path.GetFileNameWithoutExtension(img)));
                }
            }

            //create file list of videos in customizeDir
            animList = new List<string> { };
            foreach (var vid in Directory.GetFiles(customizeDir))
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
}
