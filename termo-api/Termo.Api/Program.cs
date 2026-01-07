using Termo.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();

var app = builder.Build();
app.MapControllers();
app.Run();
