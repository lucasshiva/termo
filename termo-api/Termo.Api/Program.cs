using System.IO.Abstractions;
using Termo.Api.Data;
using Termo.Api.Repositories;
using Termo.Api.UseCases;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddSingleton<IFileSystem, FileSystem>();
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();
builder.Services.AddSingleton<IWordRepository>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    var filePath = Path.Combine(env.ContentRootPath, "Data", "words.json");
    var fileSystem = sp.GetRequiredService<IFileSystem>();
    var loader = new WordLoader(filePath, fileSystem);
    var words = loader.LoadWords();
    return new WordRepository(words);
});
builder.Services.AddSingleton<CreateGameUseCase>();
builder.Services.AddSingleton<GetGameByIdUseCase>();

var app = builder.Build();
app.MapControllers();
app.Run();
