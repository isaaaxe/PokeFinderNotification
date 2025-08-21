using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
class Program
{
    static void Main(string[] args)
    {
        for (int i = 1; i < 6; i++)
        { // Define the area of the screen to capture
          //TODO native screen size
            Thread.Sleep(5000);
            Rectangle captureArea = new Rectangle(2310, 1190, 170, 170);

            // Create a bitmap to store the screenshot
            using Bitmap bmp = new Bitmap(captureArea.Width, captureArea.Height);

            // Create graphics from the bitmap and copy from screen

            using Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);

            //Save the screenshot to a file
            string filePath = $"screenshot{i}.png";
            bmp.Save(filePath, ImageFormat.Png);
            bool hasWhitePixel = false;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixel  = bmp.GetPixel(x, y);
                    if (pixel.R >= 240 && pixel.G >= 240 && pixel.B >= 240)
                    {
                        hasWhitePixel = true;
                        break;
                    }
                }
                if (hasWhitePixel) break;
            }

            if (hasWhitePixel)
            {
                Console.WriteLine("white pixel detected!!!");
            }
            else
            {
                Console.WriteLine("no white pixel detected");
            }

            // Open the screenshot using default image viewer
            //Process.Start(new ProcessStartInfo
            //{
            //    FileName = filePath,
            //    UseShellExecute = true
            //});

            //Detecting white pixel
        }
    }
}
 