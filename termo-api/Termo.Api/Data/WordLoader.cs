using System.IO.Abstractions;
using System.Text.Json;
using Termo.Api.Models;

namespace Termo.Api.Data;

public interface IWordLoader
{
    IEnumerable<Word> LoadWords();
}

public record WordsModel(IEnumerable<string> Words, Dictionary<string, string> AccentMapping);

public class WordLoader : IWordLoader
{
    private readonly string _filePath;
    private readonly IFileSystem _fileSystem;

    public WordLoader(string filePath, IFileSystem fileSystem)
    {
        _filePath = filePath;
        _fileSystem = fileSystem;
    }

    public IEnumerable<Word> LoadWords()
    {
        var json = _fileSystem.File.ReadAllText(_filePath);
        try
        {
            var model = JsonSerializer.Deserialize<WordsModel>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = false }
            );
            return model!.Words.Select(entry => new Word(
                entry,
                model.AccentMapping.GetValueOrDefault(entry)
            ));
        }
        catch (ArgumentNullException e)
        {
            throw new InvalidDataException("Failed to load words from file: ", e);
        }
    }
}
