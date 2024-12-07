using Microsoft.EntityFrameworkCore;
using SignalRDemo.Data;
using SignalRDemo.Hubs;
using SignalRDemo.Helpers;

var builder = WebApplication.CreateBuilder(args);


//Get Configuration and Environment
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
//builder.Services.AddScoped<TriviaHub>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddSingleton<IServiceScopeFactory, ServiceScopeFactory>();
//builder.Services.AddSingleton<IServiceScopeFactory>();
//builder.Services.AddScoped<IPieRepository, PieRepository>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//Add Database Stuff
builder.Services.AddDbContextPool<SignalRDemoDbContext>(options => {
    options.UseSqlServer(configuration.GetConnectionString("SignalRDemoDb"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SignalRDemo.Hubs.TriviaHub>("/triviahub");
});

//app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
