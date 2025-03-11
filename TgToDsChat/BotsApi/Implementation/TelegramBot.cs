using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgToDsChat.BotsApi.Interface;

namespace TgToDsChat.BotsApi.Implementation;

public class TelegramBot : IBot
{
    private TelegramBotClient _bot;
    //private IBot _forwarderBot;

    public async Task InitialBot(string token)
    {
        using var cts = new CancellationTokenSource();
        _bot = new TelegramBotClient(token, cancellationToken: cts.Token);
        var me = await _bot.GetMe(cancellationToken: cts.Token);
        _bot.OnMessage += OnMessage;
    }
    
    async Task OnMessage(Message msg, UpdateType type)
    {
        if (msg.Text is null) return;
        var user = msg.From;
        if (user == null)
        {
            return;
        }

        await _bot.SendMessage(user.Id, await GetUserProfilePhotoUrl(user));
        //await _forwarderBot.SendMessageAsync(new MessageData(user.Username, msg.Text,
        //    await GetUserProfilePhotoUrl(user)));
    }
    
    async Task<string?> GetUserProfilePhotoUrl(User user)
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

    public Task SendMessageAsync(MessageData message)
    {
        throw new NotImplementedException();
    }
}