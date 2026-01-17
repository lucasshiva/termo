using System.IO.Abstractions;
using Termo.Api.Data;
using Termo.Api.Guesses;
using Termo.Api.Models;
using Termo.Api.Repositories;
using Termo.Api.UseCases;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    string frontendUrl = builder.Configuration["Frontend:Url"] ?? "http://localhost:5173";
    builder.Services.AddCors(opts =>
    {
        opts.AddPolicy(
            name: "DevPolicy",
            configurePolicy: policy =>
            {
                policy
                    .WithOrigins(frontendUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }
        );
    });
}

builder.Services.AddControllers();

builder.Services.AddSingleton<IFileSystem, FileSystem>();
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();
builder.Services.AddSingleton<IGuessEvaluator, GuessEvaluator>();
builder.Services.AddSingleton<IWordRepository>(sp =>
{
    string filePath = Path.Combine(AppContext.BaseDirectory, "Data", "words.json");
    IFileSystem fileSystem = sp.GetRequiredService<IFileSystem>();
    var loader = new WordLoader(filePath, fileSystem);
    IEnumerable<Word> words = loader.LoadWords();
    return new WordRepository(words);
});
builder.Services.AddSingleton<CreateGameUseCase>();
builder.Services.AddSingleton<GetGameByIdUseCase>();
builder.Services.AddSingleton<SubmitGuessUseCase>();

WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseCors("DevPolicy");
app.UsePathBase("/api");
app.UseRouting();
app.MapControllers();
app.Run();
