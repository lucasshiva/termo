using Termo.Api.Repositories;
using Termo.Api.UseCases;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();
builder.Services.AddSingleton<CreateGameUseCase>();
builder.Services.AddSingleton<GetGameByIdUseCase>();

var app = builder.Build();
app.MapControllers();
app.Run();
