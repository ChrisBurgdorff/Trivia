namespace SignalRDemo.Models
{
  public enum PlayerStatus
  {
    Waiting,
    Active,
    Disconnected
  }
  public class PlayerModel
  {
    public int Id {get; set; }
    public string ConnectionId {get; set;}
    public string Name { get; set; }
    public int Score {get; set; }
    public PlayerStatus Status {get; set; }
    public int CurrentQuestion { get; set; }
  }
}