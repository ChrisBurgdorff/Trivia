//HELPER FUNCTIONS
function updatePlayerQueue() {
  fetch("/trivia/playersinqueue", {
    method: "GET",
    headers: {
      'content-type': 'application/json'
    }
  }).then(response => response.json()).then(data => {
    console.log(data);
    //console.log(data[0]);
    var playerList = document.getElementById("playerQueue");
    playerList.innerHTML = "";
    data.sort(function (a, b) {
      return a.name.toLowerCase().localeCompare(b.name.toLowerCase());
    }).forEach(player => {
      $("#playerQueue").append(`<li class="list-group-item">${player.name}</li>`);
    });
    
  }).catch(err => {
    console.log(err);
  });
}

function playWhoosh() {
  var audio = new Audio("/sounds/whoosh.mp3");
  audio.loop = false;
  audio.play();
}
function playCorrect() {
  var audio = new Audio("/sounds/correct-choice.mp3");
  audio.loop = false;
  audio.play();
}
function playWrong() {
  var audio = new Audio("/sounds/wrong-answer.mp3");
  audio.volume = 0.3;
  audio.loop = false;
  audio.play();
}
function playTick() {
  var audio = new Audio("/sounds/clock-close.mp3");
  audio.loop = false;
  audio.play();
}
function playStartComputer() {
  var audio = new Audio("/sounds/start-computer.mp3");
  audio.loop = false;
  audio.play();
}

//SignalR Stuff
setupConnection = () => {
  connection = new signalR.HubConnectionBuilder()
    .withUrl("/triviahub")
    .build();

  connection.on("DisplayMessage", (chatMessage) => {
    let chatDate = new Date(chatMessage.timestamp);
    let timestampText = chatDate.toLocaleTimeString();
    $("#chatBox").append(`<div class="group-rom">
      <div class="first-part">${chatMessage.name}</div>
      <div class="second-part">${chatMessage.message}</div>
      <div class="third-part">${timestampText}</div>
    </div>`);
    let newScroll = $("#chatBox")[0].scrollHeight;
    $("#chatBox").animate({scrollTop: newScroll}, 500);
    playWhoosh();
  });

  connection.on("NewUser", (userId) => {
    updatePlayerQueue();
    console.log(userId + " has joined!");
  });

  connection.on("Connected", (userId) => {
    connection.connectionId = userId;
  });

  connection.on("UserLeft", (userId) => {
  });

  connection.on("UpdatePlayerList", () => {
    updatePlayerQueue();
  });

  connection.on("UpdateLeaderboard", (gameId) => {
    console.log(gameId);
    fetch("/trivia/activeplayers/" + gameId, {
      method: "GET",
      headers: {
        'content-type': 'application/json'
      }
    }).then(response => response.json()).then(data => {
      console.log(data);
      //console.log(data[0]);
      var playerList = document.getElementById("playerQueue");
      playerList.innerHTML = "";
      data.sort(function (a, b) {
        return (a.score < b.score) ;
      }).forEach(player => {
        $("#playerQueue").append(`<li class="list-group-item">${player.name}<span class="score">${player.score}</span></li>`);
      });
      
    }).catch(err => {
      console.log(err);
    });
  });

  connection.on("GameStart", () => {
    playTick();
    $("#userListHeading").text("Leaderboard");
    $("#questionNumberHeading").text("GAME STARTING...");
    $("#joinBody").hide();
    $("#waitingBody").hide();
    $("#gameOverBody").hide();
    //$("#questionBody").show();
    $("#startGame").hide();
    $("#startGame").prop("disabled", true);
  });

  connection.on("GameEnd", () => {
    playStartComputer();
    console.log("GAME OVER MAAAAAANNN");
    $("#questionBody").hide();
    $("#answerResultBody").hide();
    $("#gameOverBody").show();
    $("#startGame").prop("disabled", false);
    $("#startGame").show();
    //Change :Question 10
    $("#questionNumberHeading").text("Play Again?");
  });

  connection.on("Question", (question) => {
    playStartComputer();
    localStorage.setItem("submitted", "false");
    localStorage.setItem("correct", "false");
    $("#answerResultBody").hide();
    $("#questionBody").show();
    localStorage.setItem("questionNumber", question.questionNumber);
    var questionElement = document.getElementById("question");
    var answerList = document.getElementById("answers");
    $(questionElement).empty();
    $(answerList).empty();
    $("#questionNumberHeading").text("Question " + question.questionNumber);    
    questionElement.appendChild(document.createTextNode(question.question));
    var allAnswers = [];
    question.incorrectAnswers.forEach(answer => { allAnswers.push(answer.answer) });
    allAnswers.push(question.correctAnswer);
    allAnswers.sort(function (a, b) {
      return b.toLowerCase().localeCompare(a.toLowerCase());
    }).forEach( (answer, index) => {
      $(answerList).append(`<li class="list-group-item"><button class="btn btn-light" id="answer_${index}">${answer}</button></li>`);
      if (answer == question.correctAnswer) {
        localStorage.setItem("correctAnswer", index);
      }
    });
    //Start timer and Submit after 20 SECONDS
    let secondsRemaining = 20;
    const startTime = new Date().getTime();
    var playedSound = false;
    var questionTimer = setInterval(function() {      
      const now = new Date().getTime();
      var timeLeft = 20 - Math.floor(( (now - startTime) % (1000 * 60)) / 1000);
      $("#countdown").text((timeLeft < 0 ? 0 : timeLeft) + " Seconds Remaining");
      if (localStorage.getItem("submitted") === "true") {
        $("#answerResultBody").show();
        $("#questionBody").hide();
        if (localStorage.getItem("correct") === "true") {
          if (!playedSound) {
            playCorrect();
            playedSound = true;
          }          
          $("#wrongReveal").hide();
          $("#correctReveal").show();
        } else {
          if (!playedSound) {
            playWrong();
            playedSound = true;
          }          
          $("#wrongReveal").show();
          $("#correctReveal").hide();
        }
        $("#correctAnswerReveal").text(question.correctAnswer);
        $("#timeRemainingReveal").text((timeLeft < 0 ? 0 : timeLeft) + " Seconds");
      }
      if (timeLeft < 5 && timeLeft >= 4 && localStorage.getItem("submitted") == "false") {
        playTick();
      }
      if (timeLeft < 0) {
        sendAnswer();
        clearInterval(questionTimer);
      }
    }, 1000);
  });

  connection.start()
    .catch(err => console.error(err.toString()));
}

setupConnection();

function sendAnswer() {
  let correct = (localStorage.getItem("correct") === "true")
  let questionNumber = parseInt(localStorage.getItem("questionNumber"));
  
  connection.invoke("ReceiveQuestion", connection.connectionId, questionNumber, correct);
}

$(document).ready(function() {
  $("#sendChatForm").submit(function(e) {
    e.preventDefault();
    const message = document.getElementById("chatMessage").value;
    var chat = {};
    chat.Message = message;
    $("#chatMessage").val("");
    connection.invoke("SendMessage", chat);
  });
  $("#timerTest").click(function(e) {
    e.preventDefault();
    connection.invoke("RunGame");
  });
  $("#submitAnswer").click(function(e) {
    e.preventDefault();
    //sendAnswer();
    localStorage.setItem("submitted", "true");
    
  });
  $("#enterPlayerForm").submit(function(e) {
    e.preventDefault();
    const playerName = document.getElementById("playerName").value;
    
    fetch("/trivia/addplayertoqueue", {
      method: "POST",
      body: JSON.stringify({Name: playerName, ConnectionId: connection.connectionId, Score: 0, CurrentQuestion: 0}),
      headers: {
        'content-type': 'application/json'
      }
    }).then(response => response.text()).then(id => {
      $("#playerName").val("");
      //INVOKE Player added to queue
      var newPlayer = {};
      newPlayer.Name = playerName;
      newPlayer.ConnectionId = connection.connectionId;
      $("#joinBody").hide();
      $("#waitingBody").show();
      $("#questionNumberHeading").text("THANK YOU!");
      $("#startGame").show();
      $("#startGame").prop('disabled', false);
      $("#sendChat").prop('disabled', false);
      connection.invoke("PlayerAddedToQueue", newPlayer);
    });
  });
  $("#startGame").click(function(e) {
    e.preventDefault();
    $("#gameOverBody").hide();
    //Get Questions
    fetch("https://opentdb.com/api.php?amount=10", {
      method: "GET"
    }).then(response => response.json()).then(response => {
      const questions = response.results;
      fetch("/trivia/playersinqueue", {
        method: "GET",
        headers: {
          'content-type': 'application/json'
        }
      }).then(playerResponse => playerResponse.json()).then(playerResponse => {
        console.log(playerResponse[0]);
        var newGame = {};
        newGame.Players = [];
        newGame.Questions = [];
        questions.forEach(question => {
          var wrongAnswers = [];
          question.incorrect_answers.forEach(wrongAnswer => {
            var newAnswer = {};
            newAnswer.Answer = wrongAnswer;
            wrongAnswers.push(newAnswer);
          });
          var newQuestion = {};
          newQuestion.Question = question.question;
          newQuestion.CorrectAnswer = question.correct_answer;
          newQuestion.IncorrectAnswers = wrongAnswers;
          newQuestion.Category = question.category;
          newGame.Questions.push(newQuestion);
        });
        fetch("/trivia/addgame", {
          method: "POST",
          body: JSON.stringify(newGame),
          headers: {
            'content-type': 'application/json'
          }
        }).then(response => response.text()).then(id => {
          //AddPlayers with 
          var playersToAdd = [];
          playerResponse.forEach(player => {
            var newPlayer = {};
            newPlayer.Id = player.id;
            newPlayer.ConnectionId = player.connectionId;
            newPlayer.Name = player.name;
            newPlayer.Score = player.score;
            newPlayer.Status = player.status;
            playersToAdd.push(newPlayer);
          });
          console.log(JSON.stringify({
            GameId: id,
            Players: playersToAdd
          }));
          //Change this add the players manually like above.
          fetch("/trivia/addplayerstogame", {
            method: "POST",
            body: JSON.stringify({
              GameId: id,
              Players: playersToAdd
            }),
            headers: {
              'content-type': 'application/json'
            }
          }).then(response => response.text()).then(id => {
            //Set up Room for Game...
            const newRoom = "Room" + id;
            playersToAdd.forEach(player => {
              connection.invoke("JoinRoom", player.ConnectionId, newRoom);
            });
            connection.invoke("RunGame", parseInt(id));
            
          });        
        });
      });      
    });
  });
  $(document).on("click", '[id^="answer_"]', function(){ 
    var index = $(this).attr('id').split("_")[1];
    if (localStorage.getItem("correctAnswer") == index) {
      localStorage.setItem("correct", true);
    } else {
      localStorage.setItem("correct", false);
    }
    $("#answers").find("li").each((answer) => {
      if (answer == index) {
        $("#answer_" + index).addClass("btn-danger");
        $("#answer_" + index).removeClass("btn-light");
      } else {
        $("#answer_" + answer).removeClass("btn-danger");
        $("#answer_" + answer).addClass("btn-light");
      }
    });
  });
});
