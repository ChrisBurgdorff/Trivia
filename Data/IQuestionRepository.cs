using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRDemo.Models;

namespace SignalRDemo.Data
{
  public interface IQuestionRepository
  {
    void AddQuestion(QuestionModel question);
  }
}