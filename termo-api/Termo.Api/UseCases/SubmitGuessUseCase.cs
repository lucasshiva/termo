using Termo.Api.Dtos;
using Termo.Api.Guesses;
using Termo.Api.Models;
using Termo.Api.Repositories;

namespace Termo.Api.UseCases;

public class SubmitGuessUseCase(IGameRepository gameRepository, IGuessEvaluator guessEvaluator)
{
    public async Task<GameDto> ExecuteAsync(Guid gameId, string guess)
    {
        GameDto? game = await gameRepository.GetByIdAsync(gameId);
        if (game == null)
            throw new Exception("Game not found");

        bool canGuess = game.Guesses.Count < game.MaxGuesses;
        if (!canGuess)
            throw new Exception("No guesses remaining");

        var guessWord = new Word(guess);
        GuessDto guessDto = guessEvaluator.Evaluate(guess: guessWord, target: game.Word);
        List<GuessDto> updatedGuesses = [.. game.Guesses, guessDto];

        // Check for win/lost state
        bool isCorrectWord = guessDto.Evaluations.All(e => e.State == LetterState.Correct);
        if (isCorrectWord)
            game.State = GameState.Won;

        bool isFinalGuess = updatedGuesses.Count == game.MaxGuesses;
        if (isFinalGuess && !isCorrectWord)
            game.State = GameState.Lost;

        GameDto updatedGame = game with { Guesses = updatedGuesses };
        await gameRepository.AddAsync(updatedGame);
        return updatedGame;
    }
}
