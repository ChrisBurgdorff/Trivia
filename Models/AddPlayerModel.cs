namespace SignalRDemo.Models
{
  public class AddPlayerModel
  {
    public int GameId { get; set; }
    public List<PlayerModel> Players { get; set; }
  }
}