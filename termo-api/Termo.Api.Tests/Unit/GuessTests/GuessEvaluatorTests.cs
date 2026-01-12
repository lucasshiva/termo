using Shouldly;
using Termo.Api.Dtos;
using Termo.Api.Guesses;
using Termo.Api.Models;

namespace Termo.Api.Tests.Unit.GuessTests;

public class GuessEvaluatorTests
{
    private static readonly GuessEvaluationCase PlacaCasal = new(
        Target: new Word("placa"),
        Guess: new Word("casal"),
        ExpectedStates:
        [
            LetterState.Present,
            LetterState.Present,
            LetterState.Absent,
            LetterState.Present,
            LetterState.Present,
        ]
    );

    private static readonly GuessEvaluationCase CasalPausa = new(
        Target: new Word("casal"),
        Guess: new Word("pausa"),
        ExpectedStates:
        [
            LetterState.Absent,
            LetterState.Correct,
            LetterState.Absent,
            LetterState.Present,
            LetterState.Present,
        ]
    );

    private static readonly GuessEvaluationCase FogaoPavao = new(
        Target: new Word(value: "fogao", displayText: "FOGÃO"),
        Guess: new Word(value: "pavao", displayText: "PAVÃO"),
        ExpectedStates:
        [
            LetterState.Absent,
            LetterState.Absent, // Next 'a' is correct, so this has to be absent.
            LetterState.Absent,
            LetterState.Correct,
            LetterState.Correct,
        ]
    );

    [Test]
    public void Evaluate_WithAllLettersCorrect_ReturnsAllCorrect()
    {
        // Arrange
        var guess = new Word("casal");
        var target = new Word("casal");
        var evaluator = new GuessEvaluator();

        // Act
        GuessDto guessDto = evaluator.Evaluate(guess: guess, target: target);

        // Assert
        guessDto.Value.ShouldBe(guess.Value);
        guessDto.Evaluations.Count.ShouldBe(guess.Length);
        guessDto
            .Evaluations.Select(e => e.State)
            .ShouldAllBe(state => state == LetterState.Correct);
    }

    [Test]
    public void Evaluate_WithAllLettersWrong_ReturnsAllAbsent()
    {
        // Arrange
        var guess = new Word("reino");
        var target = new Word("mudas");
        var evaluator = new GuessEvaluator();

        // Act
        GuessDto guessDto = evaluator.Evaluate(guess: guess, target: target);

        // Assert
        guessDto.Value.ShouldBe(guess.Value);
        guessDto.Evaluations.Count.ShouldBe(guess.Length);
        guessDto.Evaluations.Select(e => e.State).ShouldAllBe(state => state == LetterState.Absent);
    }

    [Test]
    [CombinedDataSources]
    public void Evaluate_ReturnsExpectedResult(
        [MethodDataSource(nameof(GetTestCases))] GuessEvaluationCase testCase
    )
    {
        // Arrange
        Word target = testCase.Target;
        Word guess = testCase.Guess;
        var evaluator = new GuessEvaluator();

        // Act
        GuessDto guessDto = evaluator.Evaluate(guess: guess, target: target);

        // Assert
        guessDto.Value.ShouldBe(guess.Value);
        guessDto.Evaluations.Count.ShouldBe(guess.Length);
        guessDto.Evaluations.Select(e => e.State).ShouldBe(testCase.ExpectedStates);
    }

    public static IEnumerable<GuessEvaluationCase> GetTestCases()
    {
        yield return PlacaCasal;
        yield return CasalPausa;
        yield return FogaoPavao;
    }

    public record GuessEvaluationCase(
        Word Target,
        Word Guess,
        IReadOnlyList<LetterState> ExpectedStates
    );
}
