using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeFinderNotification
{
    public class MyAppSettings
    {
        public required PixelScannerSettings PixelScannerSettings { get; set; }
        public required TelegramBotSettings TelegramBotSettings { get; set; }

    }

    public class PixelScannerSettings
    {
        public int ScanInterval { get; set; }
    }

    public class TelegramBotSettings
    {
        public required string API_KEY { get; set; } 
    }
}
