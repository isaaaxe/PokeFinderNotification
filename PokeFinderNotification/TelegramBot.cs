using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PokeFinderNotification.Interface;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PokeFinderNotification
{
    public class TelegramBot : BackgroundService
    {
        private string filePath = "users/userID.txt";
        private readonly ITelegramBotClient _bot;
        public TelegramBot(ITelegramBotClient bot)
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .WriteTo.File("logs/TelegramBotLogs.txt").CreateLogger();

            //reset subscribed users
            File.WriteAllText(filePath, string.Empty);
            _bot = bot;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellation)
        {
            Log.Information("Telegram Bot Service starting...");
            _bot.StartReceiving(updateHandler: HandleUpdateAsync, errorHandler: HandleErrorAsync ,cancellationToken: cancellation);
            await Task.Delay(Timeout.Infinite, cancellation);
        }
        private async Task HandleUpdateAsync(ITelegramBotClient bot ,Update update, CancellationToken cancellation)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }
            var message = update.Message;
            if (message?.Text == "/start")
            {
                File.AppendAllText(filePath, message.Chat.Id.ToString() + Environment.NewLine);
                await bot.SendMessage(message.Chat.Id, $"Subscribing to PokeFinder events!");
            }

        }
        private Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage = exception switch
            {
                ApiRequestException apiEx => $"Telegram API Error:\n[{apiEx.ErrorCode}]\n{apiEx.Message}",
                _ => exception.ToString()
            };

            Log.Information(errorMessage);
            return Task.CompletedTask;
        }

    }
}
