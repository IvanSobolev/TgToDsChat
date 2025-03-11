using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgToDsChat.BotsApi.Interface;

namespace TgToDsChat.BotsApi.Implementation;

public class TelegramBot : Bot
{
    private TelegramBotClient _bot;
    
    private long _chatId = 0;

    public TelegramBot(string token)
    {
        using var cts = new CancellationTokenSource();
        _bot = new TelegramBotClient(token, cancellationToken: cts.Token);
        _bot.OnMessage += OnMessage;
    }
    
    async Task OnMessage(Message msg, UpdateType type)
    {
        if (msg.Text is null) return;
        var user = msg.From;
        if (user == null) return;

        if (msg.Text == "/setchat")
        {
            _chatId = msg.Chat.Id;
            await _bot.SendMessage(msg.Chat.Id, "This chat connected to bot.");
            return;
        }

        if (ForwarderBot == null)
        {
            await _bot.SendMessage(msg.Chat.Id, "Bot forwarder is not connected.");
            return;
        }
        
        await ForwarderBot.SendMessageAsync(new MessageData(user.Username, msg.Text,
            await GetUserProfilePhotoUrl(user)));
    }

    public override async Task SendMessageAsync(MessageData message)
    {
        if (_chatId == 0)
        {
            Console.WriteLine("Chat is not connected to " + _bot.BotId);
            return;
        }

        await _bot.SendMessage(_chatId, $">`{message.DisplayUserName}`\n{message.Text}");
    }
    
    private async Task<string?> GetUserProfilePhotoUrl(User user)
    {
        var photos = await _bot.GetUserProfilePhotosAsync(user.Id);
        if (photos.TotalCount == 0)
            return null; 

        var fileId = photos.Photos[0][0].FileId;

        var file = await _bot.GetFileAsync(fileId);

        // Формируем URL файла
        string fileUrl = $"https://api.telegram.org/file/bot{_bot.Token}/{file.FilePath}";

        return fileUrl;
    }
}