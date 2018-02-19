using System;
using StreamDeckSharp;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Accord.Video.FFMPEG;
using System.IO;
using System.Linq;

namespace StreamDeckMonitor
{
    class ImageMgr
    {
        //define StreamDeck
        public static IStreamDeck deck = StreamDeck.FromHID();

        //define StreamDeck icon dimensions
        public static int Dimens = deck.IconSize;

        //process header images and display
        public static void Process_Header_Images()
        {
            //create working dir
            Directory.CreateDirectory(SettingsMgr.generatedDir);

            //start the image header creation
            Create_Image("F/sec", SettingsMgr.Fps_pngLocation, SettingsMgr.header1_font_size, 35f, 18f);
            Create_Image("Temp", SettingsMgr.Temp_pngLocation, SettingsMgr.header1_font_size, 35f, 18f);
            Create_Image("Load", SettingsMgr.Load_pngLocation, SettingsMgr.header1_font_size, 35f, 18f);
            Create_Image("Time", SettingsMgr.Time_pngLocation, SettingsMgr.header1_font_size, 35f, 18f);
            Create_Image("Cpu:", SettingsMgr.Cpu_pngLocation, SettingsMgr.header2_font_size, 35f, 35f);
            Create_Image("Gpu:", SettingsMgr.Gpu_pngLocation, SettingsMgr.header2_font_size, 35f, 35f);

            void Create_Image(string text, string filename, int textsize, Single x, Single y)
            {
                PointF textLocation = new PointF(x, y);
                Bitmap bitmap = new Bitmap(Dimens, Dimens);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    //Some nice defaults for better quality (StreamDeckSharp.Examples.Drawing)
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                    //background fill color
                    Brush myBrush_fill = SettingsMgr.BackgroundBrush;
                    graphics.FillRectangle(myBrush_fill, 0, 0, Dimens, Dimens);

                    using (Font font = new Font(SettingsMgr.myFontFamily_Headers, textsize))
                    {
                        StringFormat format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };

                        Brush myBrush_text = SettingsMgr.HeaderBrush;
                        graphics.DrawString(text, font, myBrush_text, textLocation, format);
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
            VideoFileReader reader = new VideoFileReader();
            reader.Open(SettingsMgr.imgDir + "animation.mp4");

            //read 150 video frames out of it
            for (int i = 0; i < 150; i++)
            {
                //get, resize and save frames
                Bitmap videoFrame = new Bitmap(reader.ReadVideoFrame(), new Size(Dimens, Dimens));
                videoFrame.Save(SettingsMgr.frameDir + i + ".bmp");

                //dispose the frame when it is no longer required
                videoFrame.Dispose();
            }
            reader.Close();
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

            //get all frame images from frames dir
            var images = Directory.GetFiles(SettingsMgr.frameDir, "*.bmp");

            //set forward and reverse sequences            
            var forwardImages = images.OrderBy(d => new FileInfo(d).CreationTime);
            var reverseImages = images.OrderByDescending(d => new FileInfo(d).CreationTime);

            //start animation playback loop
            while (true)
            {
                //start image playback sequence : Forward
                foreach (var image in forwardImages)
                {
                    var forwardBitmap = StreamDeckKeyBitmap.FromFile(image);
                    ShowAnim(forwardBitmap);
                }

                //start image playback sequence : Reverse
                foreach (var image in reverseImages)
                {
                    var reverseBitmap = StreamDeckKeyBitmap.FromFile(image);
                    ShowAnim(reverseBitmap);
                }
            }

            //send images to the deck in the sequence the frames are received
            void ShowAnim(StreamDeckKeyBitmap anim)
            {
                deck.SetKeyBitmap(SettingsMgr.Keylocation_bgimage1, anim);
                deck.SetKeyBitmap(SettingsMgr.Keylocation_bgimage2, anim);
                deck.SetKeyBitmap(SettingsMgr.Keylocation_bgimage3, anim);
                deck.SetKeyBitmap(SettingsMgr.Keylocation_bgimage4, anim);
                deck.SetKeyBitmap(SettingsMgr.Keylocation_bgimage5, anim);
                deck.SetKeyBitmap(SettingsMgr.Keylocation_bgimage6, anim);
                deck.SetKeyBitmap(SettingsMgr.Keylocation_bgimage7, anim);

                //define frametime
                System.Threading.Thread.Sleep(frametime);
            }
        }

        //process static images and display
        public static void Set_Static(string header_type, int header_location)
        {
            string bitmapLocation;
            if (header_type == "image")
            {
                bitmapLocation = SettingsMgr.imgDir + header_type + ".png";
            }
            else
            {
                bitmapLocation = SettingsMgr.generatedDir + header_type + ".png";
            }
            var staticBitmap = StreamDeckKeyBitmap.FromFile(bitmapLocation);
            deck.SetKeyBitmap(header_location, staticBitmap);
            return;
        }

        //process data images and display
        public static void Set_Value(string dataValue, string type, int location)
        {
            if (!dataValue.Equals(null))
            {
                if (type.Equals("f"))
                {
                    Process_Image(SettingsMgr.Fps_pngLocation, SettingsMgr.F_pngLocation);
                }
                if (type.Equals("t"))
                {
                    Process_Image(SettingsMgr.Temp_pngLocation, SettingsMgr.T_pngLocation);
                }
                if (type.Equals("l"))
                {
                    Process_Image(SettingsMgr.Load_pngLocation, SettingsMgr.L_pngLocation);
                }
                if (type.Equals("ti"))
                {
                    Process_Image(SettingsMgr.Time_pngLocation, SettingsMgr.Ti_pngLocation);
                }

                void Process_Image(string imagefilepath, string tempimagefilepath)
                {
                    PointF dataLocation = new PointF(36f, 50f);
                    String typeimage = imagefilepath;
                    Bitmap bitmap = (Bitmap)Image.FromFile(typeimage);//load the image file

                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        //Some nice defaults for better quality (StreamDeckSharp.Examples.Drawing)
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        using (Font font = new Font(SettingsMgr.myFontFamily_Values, SettingsMgr.value_font_size))
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
