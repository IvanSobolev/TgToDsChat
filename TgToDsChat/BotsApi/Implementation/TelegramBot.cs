using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgToDsChat.BotsApi.Interface;

namespace TgToDsChat.BotsApi.Implementation;

public class TelegramBot : Bot
{
    private readonly TelegramBotClient _bot;
    private readonly long _adminId;
    private long _connectChatId;

    public TelegramBot(string connectionString, long adminId = 0)
    {
        using var cts = new CancellationTokenSource();
        _bot = new TelegramBotClient(connectionString, cancellationToken: cts.Token);
        _bot.OnMessage += OnMessage;
        _adminId = adminId;
    }
    
    async Task OnMessage(Message msg, UpdateType type)
    {
        if (msg.Text is null) return;
        if (msg.From == null) return;
        var user = msg.From;

        if (msg.Text == "/setchat" && user.Id == _adminId)
        {
            _connectChatId = msg.Chat.Id;
            await _bot.SendMessage(msg.Chat.Id, "This chat connected to bot.");
            return;
        }
        
        await SendMessageToForwarderAsync(new MessageData(user.FirstName, msg.Text,
            await GetUserProfilePhotoUrl(user)));
    }

    protected override async Task SendMessageAsync(MessageData message)
    {
        if (_connectChatId == 0)
        {
            await _bot.SendMessage(_adminId, "Bot is not connected to chat");
            return;
        }

        await _bot.SendMessage(_connectChatId, $">`{message.DisplayUserName}`\n{message.Text}", parseMode: ParseMode.MarkdownV2);
    }
    
    private async Task<string> GetUserProfilePhotoUrl(User user)
    {
        var photos = await _bot.GetUserProfilePhotosAsync(user.Id);
        if (photos.TotalCount == 0)
            return "https://upload.wikimedia.org/wikipedia/commons/thumb/5/59/Minecraft_missing_texture_block.svg/2048px-Minecraft_missing_texture_block.svg.png"; 

        var fileId = photos.Photos[0][0].FileId;

        var file = await _bot.GetFileAsync(fileId);

        string fileUrl = $"https://api.telegram.org/file/bot{_bot.Token}/{file.FilePath}";

        return fileUrl;
    }
}