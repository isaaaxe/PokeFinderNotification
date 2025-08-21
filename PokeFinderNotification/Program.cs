using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PokeFinderNotification.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace PokeFinderNotification
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Host.CreateDefaultBuilder(args).UseWindowsService().ConfigureServices((hostContext, services) =>
            {
                IConfiguration config = hostContext.Configuration;

                string botApiKey = config.GetSection("AppSettings").GetSection("TelegramBotSettings").GetValue<string>("API_KEY");

                services.AddSingleton<ITelegramBotClient>(sp => new TelegramBotClient(botApiKey));
                services.AddSingleton<ITelegramNotifier, TelegramNotifier>();
                services.AddHostedService<PixelScanner>();
                services.AddHostedService<TelegramBot>();
                services.Configure<MyAppSettings>(config.GetSection("AppSettings"));
            }).Build().Run();
        }
    }
}
