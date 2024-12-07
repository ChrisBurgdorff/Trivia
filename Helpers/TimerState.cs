using SignalRDemo.Models;

namespace SignalRDemo.Helpers
{
  public class TimerState
  {
    public int Counter { get; set; } = 0;
    public GameModel Game { get; set; }
  }
}