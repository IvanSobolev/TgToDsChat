using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgToDsChat.BotsApi.Implementation;
using TgToDsChat.BotsApi.Interface;

namespace TgToDsChat;

class Program
{
    static async Task Main()
    {
        string token1 = File.ReadLines("../../../tg.token").First();
        string token2 = File.ReadLines("../../../tg2.token").First();
        Bot bot1 = new TelegramBot(token1);
        Bot bot2 = new TelegramBot(token2);
        bot1.SetForwarder(bot2);
        bot2.SetForwarder(bot1);
        Console.ReadLine();
    }
}