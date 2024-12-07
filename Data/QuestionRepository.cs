using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRDemo.Models;

namespace SignalRDemo.Data
{
  public class QuestionRepository : IQuestionRepository
  {
    private readonly SignalRDemoDbContext _appDbContext;
    public QuestionRepository(SignalRDemoDbContext appDbContext)
    {
      _appDbContext = appDbContext;
    }
    public void AddQuestion(QuestionModel question)
    {
      _appDbContext.Questions.Add(question);
      _appDbContext.SaveChanges();
    }
  }
}