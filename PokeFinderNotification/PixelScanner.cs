using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PokeFinderNotification.Interface;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PokeFinderNotification
{
    public class PixelScanner : BackgroundService
    {
        private readonly PixelScannerSettings _scannerSettings;
        private readonly ITelegramNotifier _notifier;
        //private int Count;
        private string filePath = "users/userID.txt";

        public PixelScanner(IOptions<MyAppSettings> appSettings, ITelegramNotifier notifier)
        {
            _scannerSettings = appSettings.Value.PixelScannerSettings;
            _notifier = notifier;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/PokeFinderScannerLogs.txt").CreateLogger();
            //Count = 1;
            Log.Information($"PokeFinder Service starting...  polling at interval of {_scannerSettings.ScanInterval}");
        }
        protected override async Task ExecuteAsync(CancellationToken cancellation)
        {
            
            while (!cancellation.IsCancellationRequested)
            {
                Rectangle captureArea = new Rectangle(2310, 1190, 170, 170);

                // Create a bitmap to store the screenshot
                using Bitmap bmp = new Bitmap(captureArea.Width, captureArea.Height);

                // Create graphics from the bitmap and copy from screen

                using Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen(captureArea.Location, Point.Empty, captureArea.Size);

                //Save the screenshot to a file
                //string filePath = $"screenshot{Count}.png";
                //bmp.Save(filePath, ImageFormat.Png);
                bool hasWhitePixel = false;

                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        System.Drawing.Color pixel = bmp.GetPixel(x, y);
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
                    //Log.Information($"Count: {Count}.. white pixel detected!");
                    string[] userids = File.ReadAllLines(filePath);
                    foreach (string userid in userids)
                    {
                        await _notifier.SendNotification(long.Parse(userid), "Pokemon detected in PokeFinder!");
                    }
                }
                await Task.Delay(_scannerSettings.ScanInterval, cancellation);
            }
        }
    }
}
