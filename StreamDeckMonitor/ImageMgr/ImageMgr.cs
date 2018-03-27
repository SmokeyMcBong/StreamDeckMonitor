using System;
using StreamDeckSharp;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Accord.Video.FFMPEG;
using System.IO;
using System.Drawing.Imaging;

namespace StreamDeckMonitor
{
    class ImageMgr
    {
        //define StreamDeck
        public static IStreamDeck deck = StreamDeck.FromHID();

        //define StreamDeck icon dimensions
        public static int dimens = deck.IconSize;

        //process header images and display
        public static void ProcessHeaderImages()
        {
            //create working dir
            Directory.CreateDirectory(SettingsMgr.generatedDir);

            //header text locations
            Single xAxis = 35;
            Single yAxis = 35;
            Single xAxis2 = 35;
            Single yAxis2 = 18;

            //start the image header creation
            CreateImage("Cpu:", SettingsMgr.ImageLocCpu, SettingsMgr.headerFontSize1, xAxis, yAxis);
            CreateImage("Gpu:", SettingsMgr.ImageLocGpu, SettingsMgr.headerFontSize1, xAxis, yAxis);
            CreateImage("F/sec", SettingsMgr.ImageLocFps, SettingsMgr.headerFontSize2, xAxis2, yAxis2);
            CreateImage("Temp", SettingsMgr.ImageLocTemp, SettingsMgr.headerFontSize2, xAxis2, yAxis2);
            CreateImage("Load", SettingsMgr.ImageLocLoad, SettingsMgr.headerFontSize2, xAxis2, yAxis2);
            CreateImage("Time", SettingsMgr.ImageLocTime, SettingsMgr.headerFontSize2, xAxis2, yAxis2);

            void CreateImage(string text, string filename, int textSize, Single x, Single y)
            {
                Font font;
                Brush myBrushText;
                PointF textLocation = new PointF(x, y);
                Bitmap bitmap = new Bitmap(dimens, dimens);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    //Some nice defaults for better quality (StreamDeckSharp.Examples.Drawing)
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                    //background fill color
                    Brush myBrushFill = SettingsMgr.BackgroundBrush;
                    graphics.FillRectangle(myBrushFill, 0, 0, dimens, dimens);

                    //create images using defined font styles in config
                    if (text == "Cpu:" || text == "Gpu:")
                    {
                        font = new Font(SettingsMgr.myFontHeader1, textSize);
                        myBrushText = SettingsMgr.HeaderBrush1;
                    }
                    else
                    {
                        font = new Font(SettingsMgr.myFontHeader2, textSize);
                        myBrushText = SettingsMgr.HeaderBrush2;
                    }

                    using (font)
                    {
                        StringFormat format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString(text, font, myBrushText, textLocation, format);
                        bitmap.Save(filename);
                    }
                }
            }
        }

        //process video frames for animation
        public static void StartAnimation()
        {
            while (true)
            {
                //create instance of video reader and open video file
                VideoFileReader vidReader = new VideoFileReader();
                string vidFile = SettingsMgr.animationImgDir + SettingsMgr.animName + ".mp4";
                vidReader.Open(vidFile);

                int frameCount = Convert.ToInt32(vidReader.FrameCount);
                int adjustedCount;

                if (frameCount >= SettingsMgr.framesToProcess)
                {
                    adjustedCount = SettingsMgr.framesToProcess;
                }
                else
                {
                    adjustedCount = frameCount;
                }

                for (int i = 0; i < adjustedCount; i++)
                {
                    using (var vidStream = new MemoryStream())
                    {
                        //resize and save frames to MemoryStream
                        Bitmap videoFrame = new Bitmap(vidReader.ReadVideoFrame(), new Size(dimens, dimens));
                        videoFrame.Save(vidStream, ImageFormat.Png);

                        //dispose the video frame
                        videoFrame.Dispose();

                        //display animation from stream
                        vidStream.Seek(0, SeekOrigin.Begin);
                        var animStream = StreamDeckKeyBitmap.FromStream(vidStream);
                        ShowAnim(animStream);
                        vidStream.Close();
                    }
                }

                vidReader.Close();
            }

            //display animation
            void ShowAnim(StreamDeckKeyBitmap animStream)
            {
                foreach (var button in SettingsMgr.BgButtonList())
                {
                    deck.SetKeyBitmap(button, animStream);
                }

                //frametime delay
                int frametime = SettingsMgr.FrametimeValue();
                System.Threading.Thread.Sleep(frametime);
            }
        }

        //set the static headers
        public static void SetStaticHeaders()
        {
            SetStaticImg("cpu", SettingsMgr.KeyLocCpuHeader);
            SetStaticImg("gpu", SettingsMgr.KeyLocGpuHeader);
        }

        //process static images and display
        public static void SetStaticImg(string headerType, int headerLocation)
        {
            string bitmapLocation;
            if (headerType == SettingsMgr.imageName)
            {
                bitmapLocation = SettingsMgr.staticImgDir + headerType + ".png";
            }
            else
            {
                bitmapLocation = SettingsMgr.generatedDir + headerType + ".png";
            }
            var staticBitmap = StreamDeckKeyBitmap.FromFile(bitmapLocation);
            deck.SetKeyBitmap(headerLocation, staticBitmap);
        }

        //process data images and display
        public static void ProcessValueImg(string dataValue, string type, int location)
        {
            if (!dataValue.Equals(null))
            {
                if (type.Equals("f"))
                {
                    ProcessImage(SettingsMgr.ImageLocFps);
                }
                if (type.Equals("t"))
                {
                    ProcessImage(SettingsMgr.ImageLocTemp);
                }
                if (type.Equals("l"))
                {
                    ProcessImage(SettingsMgr.ImageLocLoad);
                }
                if (type.Equals("ti"))
                {
                    ProcessImage(SettingsMgr.ImageLocTime);
                }

                void ProcessImage(string imagefilepath)
                {
                    PointF dataLocation = new PointF(36f, 50f);
                    String typeImage = imagefilepath;
                    Bitmap bitmap = (Bitmap)Image.FromFile(typeImage);

                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        //Some nice defaults for better quality (StreamDeckSharp.Examples.Drawing)
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        using (Font font = new Font(SettingsMgr.myFontValues, SettingsMgr.valueFontSize))
                        {
                            StringFormat format = new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            };

                            Brush myBrush = SettingsMgr.ValuesBrush;
                            graphics.DrawString(dataValue, font, myBrush, dataLocation, format);
                        }
                    }

                    using (var valuesStream = new MemoryStream())
                    {
                        bitmap.Save(valuesStream, ImageFormat.Png);
                        bitmap.Dispose();

                        //display values using stream
                        valuesStream.Seek(0, SeekOrigin.Begin);
                        var valStream = StreamDeckKeyBitmap.FromStream(valuesStream);
                        deck.SetKeyBitmap(location, valStream);
                        valuesStream.Close();
                    }
                }
            }
        }
    }
}
