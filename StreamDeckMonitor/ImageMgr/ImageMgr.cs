using System;
using StreamDeckSharp;
using System.Drawing;

namespace StreamDeckMonitor
{
    class ImageMgr
    {
        //define StreamDeck
        public static IStreamDeck deck = StreamDeck.FromHID();

        //process header images and display
        public static void Process_Header_Images()
        {
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
                Bitmap bitmap = new Bitmap(72, 72);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font font = new Font(SettingsMgr.myFontFamily_Headers, textsize))
                    {
                        StringFormat format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };

                        Brush myBrush = new SolidBrush(Color.FromName(SettingsMgr.FontColor_Headers.ToString()));
                        graphics.DrawString(text, font, myBrush, textLocation, format);
                        bitmap.Save(filename);//save the image file 
                    }
                }
            }
        }

        //process static images and display
        public static void Set_Static(string header_type, int header_location)
        {
            var value_bitmap = StreamDeckKeyBitmap.FromFile(SettingsMgr.imgDir + header_type + ".png");
            deck.SetKeyBitmap(header_location, value_bitmap);
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
                        using (Font font = new Font(SettingsMgr.myFontFamily_Values, SettingsMgr.value_font_size))
                        {
                            StringFormat format = new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            };

                            Brush myBrush = new SolidBrush(Color.FromName(SettingsMgr.FontColor_Values));
                            graphics.DrawString(dataValue, font, myBrush, dataLocation, format);
                        }
                    }

                    bitmap.Save(tempimagefilepath);//save the image file

                    var value_bitmap = StreamDeckKeyBitmap.FromFile(tempimagefilepath);
                    deck.SetKeyBitmap(location, value_bitmap);
                }
            }
        }
    }
}

