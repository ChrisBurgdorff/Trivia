using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRDemo.Models;

namespace SignalRDemo.Data
{
  public interface IPlayerRepository
  {
    IEnumerable<PlayerModel> AllPlayers { get; }
    void AddPlayer(PlayerModel player);
    void DeletePlayer(int playerId);
    void DeleteAllPlayers();
    public PlayerModel DeactivateByConnection(string connectionId);
    IEnumerable<PlayerModel> PlayersInQueue();
    public IEnumerable<PlayerModel> ActivePlayers(int gameId);
    public PlayerModel GetPlayerById(int id);
    public int IncrementAndReturnScore(int playerId, int scoreIncrement);
    public int SetScore(int playerId, int newScore);
  }
}