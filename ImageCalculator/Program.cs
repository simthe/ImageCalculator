using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageCalculator
{
    public class RGBAverage
    {
        public long Red { get; set; }
        public long Green { get; set; }
        public long Blue { get; set; }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            //Update path to file with location with image to test
            string pathToFile = "D:\\Media\\Downloads\\101_ObjectCategories\\101_ObjectCategories\\accordion\\image_0001.jpg";

            Bitmap img = new Bitmap(pathToFile);

            var rgbAverage = CalculateAvg(img);

            var dividedImages = DivideImage(img);

            List<RGBAverage> averages = new List<RGBAverage>();
            foreach (var image in dividedImages)
            {
                averages.Add(CalculateAvg(image));
            }




        }


        public static Bitmap[] DivideImage(Bitmap img)
        {
            var images = new Bitmap[10];
            var width = img.Width;
            var height = img.Height;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    var index = i * 5 + j;
                    images[index] = new Bitmap(width, height);
                    var graphics = Graphics.FromImage(images[index]);
                    graphics.DrawImage(img, new Rectangle(0, 0, width, height), new Rectangle(i * width, j * height, width, height), GraphicsUnit.Pixel);
                    graphics.Dispose();
                }
            }

            return images;
        }

        public static RGBAverage CalculateAvg(Bitmap img)
        {
            BitmapData srcData = img.LockBits(
            new Rectangle(0, 0, img.Width, img.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);

            int stride = srcData.Stride;

            IntPtr Scan0 = srcData.Scan0;

            long[] totals = new long[] { 0, 0, 0 };

            int width = img.Width;
            int height = img.Height;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int color = 0; color < 3; color++)
                        {
                            int idx = (y * stride) + x * 4 + color;

                            totals[color] += p[idx];
                        }
                    }
                }
            }

            long blue = totals[0] / (width * height);
            long green = totals[1] / (width * height);
            long red = totals[2] / (width * height);

            return new RGBAverage { Blue = blue, Green = green, Red = red};
        }

        public static Color GetDominantColor(Bitmap bmp)
        {

            //Used for tally
            int red = 0;
            int green = 0;
            int blue = 0;

            int total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);

                    red += clr.R;
                    green += clr.G;
                    blue += clr.B;

                    total++;
                }
            }

            //Calculate average
            red /= total;
            green /= total;
            blue /= total;

            return Color.FromArgb(red, green, blue);
        }
    }
}
