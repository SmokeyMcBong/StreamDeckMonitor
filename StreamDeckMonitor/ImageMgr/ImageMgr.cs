using System;
using StreamDeckSharp;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Accord.Video.FFMPEG;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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

            //start the image header creation
            CreateImage("F/sec", SettingsMgr.ImageLocFps, SettingsMgr.header1FontSize, 35f, 18f);
            CreateImage("Temp", SettingsMgr.ImageLocTemp, SettingsMgr.header1FontSize, 35f, 18f);
            CreateImage("Load", SettingsMgr.ImageLocLoad, SettingsMgr.header1FontSize, 35f, 18f);
            CreateImage("Time", SettingsMgr.ImageLocTime, SettingsMgr.header1FontSize, 35f, 18f);
            CreateImage("Cpu:", SettingsMgr.ImageLocCpu, SettingsMgr.header2FontSize, 35f, 35f);
            CreateImage("Gpu:", SettingsMgr.ImageLocGpu, SettingsMgr.header2FontSize, 35f, 35f);

            void CreateImage(string text, string filename, int textsize, Single x, Single y)
            {
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

                    using (Font font = new Font(SettingsMgr.myFontHeaders, textsize))
                    {
                        StringFormat format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };

                        Brush myBrushText = SettingsMgr.HeaderBrush;
                        graphics.DrawString(text, font, myBrushText, textLocation, format);
                        bitmap.Save(filename);//save the image file 
                    }
                }
            }
        }

        //extract and resize video frames for animation
        public static void ProcessFrames()
        {
            //create working dir
            Directory.CreateDirectory(SettingsMgr.frameDir);

            //create instance of video reader and open video file
            VideoFileReader vidReader = new VideoFileReader();
            vidReader.Open(SettingsMgr.imgDir + "animation.mp4");

            //read 150 video frames out of it
            for (int i = 0; i < 150; i++)
            {
                //get, resize and save frames
                Bitmap videoFrame = new Bitmap(vidReader.ReadVideoFrame(), new Size(dimens, dimens));
                videoFrame.Save(SettingsMgr.frameDir + i + ".bmp");

                //dispose the frame when it is no longer required
                videoFrame.Dispose();
            }
            vidReader.Close();
        }

        //start the animation process using the stored frames
        public static void StartAnimation()
        {
            /*  Common framerates based on frametimes (milisecond delay between sending each frame) ...        
                    15fps = 66
                    25fps = 40
                    30fps = 33
                    60fps = 16
                    120fps = 8
            */

            //frametime delay
            int frametime = 33;

            //create ordered file list of images in frameDir
            List<string> frameCollection = new List<string> { };
            foreach (var capturedFrame in Directory.GetFiles(SettingsMgr.frameDir))
            {
                frameCollection.Add(capturedFrame);
            }

            //playback sequence : Forward
            var seqForward = frameCollection.OrderBy(x => x.Length);

            //playback sequence : Reverse
            var seqReverse = seqForward.Reverse();

            //start animation playback loop
            while (true)
            {
                //start image playback sequence : Forward
                foreach (var frame in seqForward)
                {
                    var forwardFrames = StreamDeckKeyBitmap.FromFile(frame);
                    ShowAnim(forwardFrames);
                }

                //start image playback sequence : Reverse
                foreach (var frame in seqReverse)
                {
                    var reverseFrames = StreamDeckKeyBitmap.FromFile(frame);
                    ShowAnim(reverseFrames);
                }
            }

            //send images to the deck in the sequence the frames are received
            void ShowAnim(StreamDeckKeyBitmap anim)
            {
                deck.SetKeyBitmap(SettingsMgr.KeyLocBgImg1, anim);
                deck.SetKeyBitmap(SettingsMgr.KeyLocBgImg2, anim);
                deck.SetKeyBitmap(SettingsMgr.KeyLocBgImg3, anim);
                deck.SetKeyBitmap(SettingsMgr.KeyLocBgImg4, anim);
                deck.SetKeyBitmap(SettingsMgr.KeyLocBgImg5, anim);
                deck.SetKeyBitmap(SettingsMgr.KeyLocBgImg6, anim);
                deck.SetKeyBitmap(SettingsMgr.KeyLocBgImg7, anim);

                //define frametime
                System.Threading.Thread.Sleep(frametime);
            }
        }

        //process static images and display
        public static void SetStaticImg(string headerType, int headerLocation)
        {
            string bitmapLocation;
            if (headerType == "image")
            {
                bitmapLocation = SettingsMgr.imgDir + headerType + ".png";
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
                    ProcessImage(SettingsMgr.ImageLocFps, SettingsMgr.TempImageLocFps);
                }
                if (type.Equals("t"))
                {
                    ProcessImage(SettingsMgr.ImageLocTemp, SettingsMgr.TempImageLocTemp);
                }
                if (type.Equals("l"))
                {
                    ProcessImage(SettingsMgr.ImageLocLoad, SettingsMgr.TempImageLocLoad);
                }
                if (type.Equals("ti"))
                {
                    ProcessImage(SettingsMgr.ImageLocTime, SettingsMgr.TempImageLocTime);
                }

                void ProcessImage(string imagefilepath, string tempimagefilepath)
                {
                    PointF dataLocation = new PointF(36f, 50f);
                    String typeImage = imagefilepath;
                    Bitmap bitmap = (Bitmap)Image.FromFile(typeImage);//load the image file

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

                    bitmap.Save(tempimagefilepath);//save the image file

                    var tempBitmap = StreamDeckKeyBitmap.FromFile(tempimagefilepath);
                    deck.SetKeyBitmap(location, tempBitmap);
                }
            }
        }
    }
}
