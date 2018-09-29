using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SharedManagers
{
    class SettingsConfigurator
    {
        public static string headerFont1;
        public static string headerFont2;
        public static string valueFont;
        public static string headerFontColor1;
        public static string headerFontColor2;
        public static string valuesFontColor;
        public static string headerFont1Position;
        public static string headerFont2Position;
        public static string valuesFontPosition;
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
        public static string colonFont;
        public static string dateFont;
        public static int timeFontSize;
        public static int colonFontSize;
        public static int dateFontSize;
        public static string timeFontColor;
        public static string colonFontColor;
        public static string dateFontColor;
        public static string clockBackgroundColor;
        public static string isCompactView;
        public static string isDateShown;
        public static int timePosition;
        public static int colonPosition;
        public static int datePosition;

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
            headerFont1Position = SharedSettings.HeaderFont1Position(currentProfile);
            headerFont2Position = SharedSettings.HeaderFont2Position(currentProfile);
            valuesFontPosition = SharedSettings.ValuesFontPosition(currentProfile);
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
            colonFont = SharedSettings.ColonFontType();
            dateFont = SharedSettings.DateFontType();
            timeFontSize = int.Parse(SharedSettings.TimeFontSize().ToString());
            colonFontSize = int.Parse(SharedSettings.ColonFontSize().ToString());
            dateFontSize = int.Parse(SharedSettings.DateFontSize().ToString());
            timeFontColor = SharedSettings.TimeFontColor();
            colonFontColor = SharedSettings.ColonFontColor();
            dateFontColor = SharedSettings.DateFontColor();
            clockBackgroundColor = SharedSettings.ClockBackgroundColor();
            isCompactView = SharedSettings.IsCompactView();
            isDateShown = SharedSettings.IsDateShown();
            timePosition = int.Parse(SharedSettings.TimePosition().ToString());
            colonPosition = int.Parse(SharedSettings.ColonPosition().ToString());
            datePosition = int.Parse(SharedSettings.DatePosition().ToString());

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
}
