using PokeFinderNotification.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace PokeFinderNotification
{
    public class TelegramNotifier : ITelegramNotifier
    {
        private readonly ITelegramBotClient _bot;
        
        public TelegramNotifier(ITelegramBotClient bot)
        {
            _bot = bot;
        }
        public async Task SendNotification(long chatId, string message)
        {
            await _bot.SendMessage(chatId, message);
        }
    }
}
