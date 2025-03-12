namespace TgToDsChat.BotsApi.Interface;

public abstract class Bot
{
    private Bot? _forwarderBot;

    protected abstract Task SendMessageAsync(MessageData message);
    
    protected async Task SendMessageToForwarderAsync(MessageData message)
    {
        if (_forwarderBot == null)
        {
            await SendMessageAsync(new MessageData("Error", "Bot forwarder is not connected."));
            return;
        }
        
        await _forwarderBot.SendMessageAsync(message);
    }
    public void SetForwarder(Bot forwarder)
    {
        _forwarderBot = forwarder;
    }
}