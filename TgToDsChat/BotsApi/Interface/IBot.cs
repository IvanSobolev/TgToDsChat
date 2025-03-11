namespace TgToDsChat.BotsApi.Interface;

public interface IBot
{
    Task InitialBot(string token);
    Task SendMessageAsync(MessageData message);
}