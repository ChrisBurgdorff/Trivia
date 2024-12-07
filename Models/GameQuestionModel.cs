namespace SignalRDemo.Models
{
  public class GameQuestionModel
  {
    public int Id {get; set; }
    public string Question { get; set; }
    public string CorrectAnswer { get; set; }
    public List<AnswerModel> IncorrectAnswers {get; set;}
    public string Category {get; set;}
    public int QuestionNumber { get; set; }
  }
}