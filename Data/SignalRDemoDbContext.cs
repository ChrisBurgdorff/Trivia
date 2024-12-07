using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SignalRDemo.Models;

namespace SignalRDemo.Data
{
  public class SignalRDemoDbContext : DbContext
  {
    public SignalRDemoDbContext(DbContextOptions<SignalRDemoDbContext> options)
      : base(options)
    {
      
    }

    public DbSet<ChatModel> Chats { get; set;}
    public DbSet<PlayerModel> Players {get; set;} 
    public DbSet<QuestionModel> Questions { get; set; }
    public DbSet<AnswerModel> Answers { get; set; }
    public DbSet<GameModel> Games { get; set; }
  }
}