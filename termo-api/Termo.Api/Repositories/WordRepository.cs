using Termo.Api.Models;

namespace Termo.Api.Repositories;

public interface IWordRepository
{
    public Word GetRandomWord();
    public Word? FindWord(string input);
}

public class WordRepository : IWordRepository
{
    private readonly Dictionary<string, Word> _inputToWord;
    private readonly Word[] _wordsArray;

    public WordRepository(IEnumerable<Word> words)
    {
        _inputToWord = words.ToDictionary(keySelector: w => w.Value, elementSelector: w => w);
        _wordsArray = _inputToWord.Values.ToArray();
    }

    public Word GetRandomWord()
    {
        return _wordsArray[Random.Shared.Next(_wordsArray.Length)];
    }

    public Word? FindWord(string input)
    {
        return _inputToWord.GetValueOrDefault(input.ToLowerInvariant());
    }
}
