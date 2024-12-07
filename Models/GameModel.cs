namespace SignalRDemo.Models
{
  public class GameModel
  {
    public int Id { get; set; }
    public List<PlayerModel> Players { get; set; }
    public List<QuestionModel> Questions { get; set; }
  }
}