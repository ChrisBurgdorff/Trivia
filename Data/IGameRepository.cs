using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRDemo.Models;

namespace SignalRDemo.Data
{
  public interface IGameRepository
  {
    public int CreateGame(GameModel game);
    public void AddPlayers(int gameId, List<PlayerModel> players);
  }
}