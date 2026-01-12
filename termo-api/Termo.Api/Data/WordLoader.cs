using System.IO.Abstractions;
using System.Text.Json;
using Termo.Api.Models;

namespace Termo.Api.Data;

public interface IWordLoader
{
    public IEnumerable<Word> LoadWords();
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
        string json = _fileSystem.File.ReadAllText(_filePath);
        try
        {
            WordsModel? model = JsonSerializer.Deserialize<WordsModel>(
                json: json,
                options: new JsonSerializerOptions { PropertyNameCaseInsensitive = false }
            );
            return model!
                // Avoids throwing an exception for words with wrong length.
                .Words.Where(w => w.Length == Word.DefaultLength)
                .Select(entry => new Word(
                    value: entry,
                    displayText: model.AccentMapping.GetValueOrDefault(entry)
                ));
        }
        catch (ArgumentNullException e)
        {
            throw new InvalidDataException(
                message: "Failed to load words from file: ",
                innerException: e
            );
        }
    }
}
