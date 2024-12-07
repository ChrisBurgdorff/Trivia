using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SignalRDemo.Models;
using SignalRDemo.Hubs;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Data;

namespace SignalRDemo.Controllers
{
  public class TriviaController : Controller
  {

    private IPlayerRepository _playerRepository;
    private IQuestionRepository _questionRepository;
    private IGameRepository _gameRepository;

    public TriviaController(IPlayerRepository playerRepository, IQuestionRepository questionRepository, IGameRepository gameRepository)
    {
      _playerRepository = playerRepository;
      _questionRepository = questionRepository;
      _gameRepository = gameRepository;
    }

    public async Task<IActionResult> AddPlayerToQueue([FromBody]PlayerModel player)
    {
      //player.ConnectionId = _triviaHub.Context.ConnectionId;
      player.Status = PlayerStatus.Waiting;
      _playerRepository.AddPlayer(player);
      //await _triviaHub.PlayerAddedToQueue(player);
      return Accepted(69);
    }

    public async Task<IActionResult> DeactivatePlayer(string connectionId)
    {
      PlayerModel deactivatedPlayer = _playerRepository.DeactivateByConnection(connectionId);
      return Accepted(deactivatedPlayer.Name);
    }

    public IEnumerable<PlayerModel> PlayersInQueue()
    {
      return _playerRepository.PlayersInQueue();
    }

    public IEnumerable<PlayerModel> ActivePlayers(int id)
    {
      return _playerRepository.ActivePlayers(id);
    }

    public async Task<IActionResult> DeleteAllPlayers()
    {
      _playerRepository.DeleteAllPlayers();
      return Accepted(69);
    }
    public async Task<IActionResult> AddQuestion([FromBody]QuestionModel question)
    {
      _questionRepository.AddQuestion(question);
      //await _triviaHub.PlayerAddedToQueue(player);
      return Accepted(69);
    }

    public async Task<IActionResult> IncrementPlayerScore([FromBody]PlayerScoreModel playerScore)
    {
      int newScore = _playerRepository.IncrementAndReturnScore(playerScore.PlayerId, playerScore.ScoreIncrement);
      return Accepted(newScore);
    }
    public async Task<IActionResult> AddGame([FromBody]GameModel game)
    {
      int newGameId = _gameRepository.CreateGame(game);
      return Accepted(newGameId);
    }
    public async Task<IActionResult> AddPlayersToGame([FromBody]AddPlayerModel addPlayers)
    {
      _gameRepository.AddPlayers(addPlayers.GameId, addPlayers.Players);
      return Accepted(addPlayers.GameId);
    }
  }
}