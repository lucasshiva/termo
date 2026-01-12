using System.IO.Abstractions;
using Termo.Api.Data;
using Termo.Api.Guesses;
using Termo.Api.Models;
using Termo.Api.Repositories;
using Termo.Api.UseCases;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddSingleton<IFileSystem, FileSystem>();
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();
builder.Services.AddSingleton<IGuessEvaluator, GuessEvaluator>();
builder.Services.AddSingleton<IWordRepository>(sp =>
{
    IWebHostEnvironment env = sp.GetRequiredService<IWebHostEnvironment>();
    string filePath = Path.Combine(path1: env.ContentRootPath, path2: "Data", path3: "words.json");
    IFileSystem fileSystem = sp.GetRequiredService<IFileSystem>();
    var loader = new WordLoader(filePath: filePath, fileSystem: fileSystem);
    IEnumerable<Word> words = loader.LoadWords();
    return new WordRepository(words);
});
builder.Services.AddSingleton<CreateGameUseCase>();
builder.Services.AddSingleton<GetGameByIdUseCase>();
builder.Services.AddSingleton<SubmitGuessUseCase>();

WebApplication app = builder.Build();
app.MapControllers();
app.Run();
