namespace SignalRDemo.Models;

public class ChatMessageModel
{
    public string Name { get; set; }
    public string Message { get; set; } = "";
    public DateTime Timestamp { get; set; }
}
