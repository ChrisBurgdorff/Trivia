using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SignalRDemo.Models;
using SignalRDemo.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo.Controllers;

[Route("[controller]")]
public class ChatController : Controller
{
    //private readonly IHubContext<TriviaHub> _triviaHub;
    //private TriviaHub _realTriviaHub;

    public ChatController()
    {

    }

    [HttpPost]
    public async Task<IActionResult> SendChat([FromBody]ChatModel chat)
    {
      var myChat = new ChatModel();
      myChat.Message = "HEHE";
      myChat.UserId = "lsdfnlskdfKGKGKGKG";
      //await _triviaHub.Clients.All.SendAsync("DisplayMessage", chat.Message);
      //await _realTriviaHub.SendMessage(chat);
      return Accepted(69);
    }

  
}
