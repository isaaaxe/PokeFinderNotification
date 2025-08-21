using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeFinderNotification.Interface
{
    public interface ITelegramNotifier
    {
        Task SendNotification(long chatId, string message);
    }
}
