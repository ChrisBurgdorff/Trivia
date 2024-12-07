using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace SignalRDemo.Data
{
  public class PlayerRepository : IPlayerRepository
  {
    private readonly SignalRDemoDbContext _appDbContext;
    public PlayerRepository(SignalRDemoDbContext appDbContext)
    {
      _appDbContext = appDbContext;
    }
    public IEnumerable<PlayerModel> AllPlayers
    {
      get 
      {
        return _appDbContext.Players;
      }
      
    }
    public void AddPlayer(PlayerModel player)
    {
      _appDbContext.Players.Add(player);
      _appDbContext.SaveChanges();
    }
    public void DeletePlayer(int playerId)
    {

    }
    public void DeleteAllPlayers()
    {
      _appDbContext.Players.RemoveRange(_appDbContext.Players);
      _appDbContext.SaveChanges();
    }
    public PlayerModel DeactivateByConnection(string connectionId)
    {
      PlayerModel player = _appDbContext.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
      player.Status = PlayerStatus.Disconnected;
      _appDbContext.SaveChanges();
      return player;
    }

    public IEnumerable<PlayerModel> PlayersInQueue()
    {
        return _appDbContext.Players.Where(p => p.Status == PlayerStatus.Waiting);
    }
    public IEnumerable<PlayerModel> ActivePlayers(int gameId)
    {
      GameModel game = _appDbContext.Games.Include(g => g.Players).FirstOrDefault(g => g.Id == gameId);
      List<PlayerModel> players = game.Players.Where(p => p.Status == PlayerStatus.Active).ToList();
      return players;
    }
    public PlayerModel GetPlayerById(int id)
    {
      return _appDbContext.Players.FirstOrDefault(p => p.Id == id);
    }
    public int IncrementAndReturnScore(int playerId, int scoreIncrement)
    {
      PlayerModel player = _appDbContext.Players.FirstOrDefault(p => p.Id == playerId);
      player.Score += scoreIncrement;
      _appDbContext.Update(player);
      _appDbContext.SaveChanges();
      return player.Score;
    }
    public int SetScore(int playerId, int newScore)
    {
      PlayerModel player = _appDbContext.Players.FirstOrDefault(p => p.Id == playerId);
      player.Score = newScore;
      _appDbContext.Update(player);
      _appDbContext.SaveChanges();
      return player.Score;
    }
  }
}