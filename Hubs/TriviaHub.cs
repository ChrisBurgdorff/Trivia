using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Models;
using SignalRDemo.Data;
using SignalRDemo.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace SignalRDemo.Hubs
{
  public class TriviaHub : Hub
  {
    //private readonly IServiceScopeFactory _scopeFactory;
    private readonly SignalRDemoDbContext _appDbContext;
    public TriviaHub(SignalRDemoDbContext appDbContext)
    {
      //_scopeFactory = scopeFactory;
      _appDbContext = appDbContext;
    }
    public async Task AskQuestion()
    {
      await Clients.All.SendAsync("QuestionAsked");
    }

    public async Task SendMessage(ChatModel chat)
    {
      //Get Player Name from Database
      PlayerModel player = _appDbContext.Players.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
      //await Clients.All.SendAsync("DisplayMessage", Context.ConnectionId + ": " + chat.Message);
      ChatMessageModel chatMessage = new ChatMessageModel();
      chatMessage.Name = player.Name;
      chatMessage.Message = chat.Message;
      chatMessage.Timestamp = DateTime.Now;
      await Clients.All.SendAsync("DisplayMessage", chatMessage);
    }

    public async Task PlayerAddedToQueue(PlayerModel player)
    {
      //Here is where I save to database
      await Clients.All.SendAsync("UpdatePlayerList");
      ChatMessageModel chatMessage = new ChatMessageModel();
      chatMessage.Name = "Notice";
      chatMessage.Message = player.Name + " IS READY TO PLAY!";
      chatMessage.Timestamp = DateTime.Now;
      await Clients.All.SendAsync("DisplayMessage", chatMessage);
    }

    public async Task PlayerRemovedFromQueue(PlayerModel player)
    {
      await Clients.All.SendAsync("DisplayMessage", player.Name + "HAS LEFT!");
    }

    public override async Task OnConnectedAsync()
    {
      await Clients.All.SendAsync("NewUser", Context.ConnectionId);
      await Clients.Client(Context.ConnectionId).SendAsync("Connected", Context.ConnectionId);
      //return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
      PlayerModel player = _appDbContext.Players.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
      player.Status = PlayerStatus.Disconnected;
      _appDbContext.SaveChanges();
      ChatMessageModel chatMessage = new ChatMessageModel();
      chatMessage.Name = "Notice";
      chatMessage.Message = player.Name + " HAS BAILED!";
      chatMessage.Timestamp = DateTime.Now;
      await Clients.All.SendAsync("DisplayMessage", chatMessage);
      await Clients.All.SendAsync("UpdatePlayerList");
      //return base.OnDisconnectedAsync(exception);
    }

    public Task JoinRoom(string connectionId, string roomName)
    {
      return Groups.AddToGroupAsync(connectionId, roomName);
    }
    public Task LeaveRoom(string connectionId, string roomName)
    {
      return Groups.RemoveFromGroupAsync(connectionId, roomName);
    }

    public async Task ReceiveQuestion(string connectionId, int questionNumber, bool correct)
    {
      PlayerModel player = _appDbContext.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
      if (correct)
      {
        player.Score += 100;
        player.CurrentQuestion = questionNumber + 1;
        _appDbContext.SaveChanges();
      } 
      else
      {
        player.CurrentQuestion = questionNumber + 1;
        _appDbContext.SaveChanges();
      }
      //Find current Game
      //Check if all active players in that game are on the next question 
      GameModel game = _appDbContext.Games.Include(g => g.Players).FirstOrDefault(g => g.Players.Any(p => p.Id == player.Id));
      bool movingOn = true;
      foreach(PlayerModel gamePlayer in game.Players)
      {
        if (gamePlayer.Status == PlayerStatus.Active && gamePlayer.CurrentQuestion == questionNumber)
        {
          movingOn = false;
        }
      }
      //FIX THIS LEADERBOARD
      await Clients.Group("Room" + game.Id.ToString()).SendAsync("UpdateLeaderboard", game.Id);
      if (movingOn)
      {
        if (questionNumber < 10)//NOT SUREEEEEEEEEEEEEE
        {
          SendQuestion(game.Id, questionNumber); //Not questionNumber + 1 because SendQuestion uses a 0 index
        }
        else
        {
          //End the game. Set all players back to waiting, then tell clients and remove from room
          _appDbContext.Database.ExecuteSqlRaw("UPDATE Players SET Status = 0 WHERE GameModelId = " + game.Id.ToString());
          await Clients.Group("Room"  + game.Id.ToString()).SendAsync("GameEnd");
          foreach(PlayerModel gamePlayer in game.Players)
          {
            LeaveRoom(gamePlayer.ConnectionId, "Room" + game.Id.ToString());
          }
        }
      }
    }

    public async Task SendQuestion(int gameId, int questionIndex)
    {
      GameModel game = _appDbContext.Games.Include(g => g.Players).Include(g => g.Questions).ThenInclude(q => q.IncorrectAnswers).FirstOrDefault(g => g.Id == gameId);

      GameQuestionModel decodedQuestion = new GameQuestionModel();
      decodedQuestion.Question = HttpUtility.HtmlDecode(game.Questions[questionIndex].Question);
      decodedQuestion.CorrectAnswer = HttpUtility.HtmlDecode(game.Questions[questionIndex].CorrectAnswer);
      decodedQuestion.IncorrectAnswers = new List<AnswerModel>();
      decodedQuestion.QuestionNumber = questionIndex + 1;

      foreach(AnswerModel answer in game.Questions[questionIndex].IncorrectAnswers)
      {
        var newIncorrectAnswer = new AnswerModel();
        newIncorrectAnswer.Answer = HttpUtility.HtmlDecode(answer.Answer);
        decodedQuestion.IncorrectAnswers.Add(newIncorrectAnswer);
      }
      await Clients.Group("Room" + gameId.ToString()).SendAsync("Question", decodedQuestion);
    }

    public async Task RunGame(int gameId)
    {
      await Clients.Group("Room" + gameId.ToString()).SendAsync("UpdatePlayerList");
      await Clients.Group("Room" + gameId.ToString()).SendAsync("GameStart");
      GameModel game = _appDbContext.Games.Include(g => g.Players).Include(g => g.Questions).ThenInclude(q => q.IncorrectAnswers).FirstOrDefault(g => g.Id == gameId);
      _appDbContext.Database.ExecuteSqlRaw("UPDATE Players SET Status = 1, Score = 0, CurrentQuestion = 1 WHERE Status = 0 AND GameModelId = " + gameId.ToString());
      
      Task.Delay(4000).Wait();
      
      SendQuestion(game.Id, 0);
      //CALL WHEN GAME IS OVER, ALSO RESET DATABASE SHIT!
      //await Clients.Group("Room" + gameId.ToString()).SendAsync("GameEnd");
    }
    
  }
}