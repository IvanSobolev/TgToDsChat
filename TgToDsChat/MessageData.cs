namespace TgToDsChat;

public struct MessageData(string username, string text, string? photourl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/59/Minecraft_missing_texture_block.svg/2048px-Minecraft_missing_texture_block.svg.png")
{
    public string DisplayUserName { get; private set; } = username;
    public string? DisplayUserPhotoUrl { get; private set; } = photourl;
    public string Text { get; private set; } = text;
}