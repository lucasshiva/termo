using Termo.Api.Dtos;
using Termo.Api.Models;

namespace Termo.Api.Guesses;

public interface IGuessEvaluator
{
    public GuessDto Evaluate(Word guess, Word target);
}

public class GuessEvaluator : IGuessEvaluator
{
    public GuessDto Evaluate(Word guess, Word target)
    {
        var evaluations = new LetterEvaluation[guess.Length];

        // Track remaining letters in target
        var remaining = new Dictionary<char, int>();
        for (var i = 0; i < guess.Length; i++)
        {
            char t = target.Value[i];
            if (!remaining.TryAdd(key: t, value: 1))
                remaining[t]++;
        }

        // First pass: correct letters
        for (var i = 0; i < guess.Length; i++)
        {
            char g = guess.Value[i];
            char t = target.Value[i];

            if (g != t)
                continue;

            evaluations[i] = new LetterEvaluation(Letter: g, State: LetterState.Correct);
            remaining[g]--;
        }

        // Second pass: present / absent
        for (var i = 0; i < guess.Length; i++)
        {
            if (evaluations.ElementAtOrDefault(i) != null)
                continue;

            char g = guess.Value[i];

            if (remaining.TryGetValue(key: g, value: out int count) && count > 0)
            {
                evaluations[i] = new LetterEvaluation(Letter: g, State: LetterState.Present);
                remaining[g]--;
            }
            else
            {
                evaluations[i] = new LetterEvaluation(Letter: g, State: LetterState.Absent);
            }
        }

        return new GuessDto { Value = guess.Value, Evaluations = evaluations.ToList() };
    }
}
