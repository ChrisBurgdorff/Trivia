using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRDemo.Models;

namespace SignalRDemo.Data
{
  public class GameRepository: IGameRepository
  {
    private readonly SignalRDemoDbContext _appDbContext;
    public GameRepository(SignalRDemoDbContext appDbContext)
    {
      _appDbContext = appDbContext;
    }
    public int CreateGame(GameModel game)
    {
      _appDbContext.Games.Add(game);
      _appDbContext.SaveChanges();
      return game.Id;
    }
    public void AddPlayers(int gameId, List<PlayerModel> players)
    {
      GameModel game = _appDbContext.Games.FirstOrDefault(g => g.Id == gameId);
      game.Players = players;
      _appDbContext.Update(game);
      _appDbContext.SaveChanges();
    }
  }
}