using Termo.Api.Models;
using Termo.Api.Repositories;

namespace Termo.Api.Tests.Integration;

public class FakeWordRepository : IWordRepository
{
    public static Word TargetWord => new("placa");
    public static Word WrongWord => new("termo");

    public Word GetRandomWord()
    {
        return TargetWord;
    }

    public Word? FindWord(string input)
    {
        return string.Equals(a: input, b: TargetWord.Value) ? TargetWord : null;
    }
}
