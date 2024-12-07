namespace SignalRDemo.Models;

public class ChatModel
{
    public int Id { get; set; }
    public string? UserId { get; set; }

    public string Message { get; set; } = "";
}
