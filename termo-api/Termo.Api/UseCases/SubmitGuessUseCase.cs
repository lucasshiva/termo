using Termo.Api.Dtos;
using Termo.Api.Guesses;
using Termo.Api.Models;
using Termo.Api.Repositories;

namespace Termo.Api.UseCases;

public class SubmitGuessUseCase(IGameRepository gameRepository, IGuessEvaluator guessEvaluator)
{
    public async Task<GameDto> ExecuteAsync(Guid gameId, string guess)
    {
        var game = await gameRepository.GetByIdAsync(gameId);
        if (game == null)
            throw new Exception("Game not found");

        var canGuess = game.Guesses.Count < game.MaxGuesses;
        if (!canGuess)
            throw new Exception("No guesses remaining");

        var guessWord = new Word(guess);
        var guessDto = guessEvaluator.Evaluate(guessWord, game.Word);
        List<GuessDto> updatedGuesses = [.. game.Guesses, guessDto];

        // Check for win/lost state
        var isCorrectWord = guessDto.Evaluations.All(e => e.State == LetterState.Correct);
        if (isCorrectWord)
            game.State = GameState.Won;

        var isFinalGuess = updatedGuesses.Count == game.MaxGuesses;
        if (isFinalGuess && !isCorrectWord)
            game.State = GameState.Lost;

        var updatedGame = game with { Guesses = updatedGuesses };
        await gameRepository.AddAsync(updatedGame);
        return updatedGame;
    }
}
