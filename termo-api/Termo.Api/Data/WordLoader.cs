using System.Globalization;
using System.IO.Abstractions;
using System.Text;
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
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (model is null)
            {
                throw new InvalidDataException("Failed to load words from file");
            }

            List<Word> words = [];
            foreach (string entry in model.Words)
            {
                if (entry.Length != Word.DefaultLength)
                    continue;
                string normalizedEntry = RemoveAccents(entry);
                var word = new Word(
                    value: normalizedEntry,
                    displayText: model.AccentMapping.GetValueOrDefault(normalizedEntry)
                );
                words.Add(word);
            }

            return words;
        }
        catch (ArgumentNullException e)
        {
            throw new InvalidDataException("Failed to load words from file: ", e);
        }
    }

    private static string RemoveAccents(string text)
    {
        string normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (char c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
