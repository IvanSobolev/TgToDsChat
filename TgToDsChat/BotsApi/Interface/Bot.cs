namespace TgToDsChat.BotsApi.Interface;

public abstract class Bot
{
    protected Bot? ForwarderBot { get; private set; }
    public abstract Task SendMessageAsync(MessageData message);
    public void SetForwarder(Bot forwarder)
    {
        ForwarderBot = forwarder;
    }
}