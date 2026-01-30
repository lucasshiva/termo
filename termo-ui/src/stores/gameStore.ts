import { useApi } from '@/composables/useApi'
import { GameState, type GameDto, type GuessDto } from '@/types/backend'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useGameStore = defineStore('game', () => {
  const localStorageGameIdKey = 'savedGameId'
  const api = useApi()
  const game = ref<GameDto>()
  const loading = ref(true)
  const error = ref<Error | null>(null)

  async function createGame() {
    loading.value = true
    try {
      const savedGameId = localStorage.getItem(localStorageGameIdKey)
      if (savedGameId === null) {
        console.log('No saved game ID found. Creating new game..')
        await createNewGame()
        return
      }

      try {
        const savedGame = await api.getGame(savedGameId)
        console.log('Found saved game: ', savedGame)
        if (savedGame.state !== GameState.IN_PROGRESS) {
          console.log('Game cannot be resumed. Creating new game..')
          await createNewGame()
          return
        }

        console.log('Resuming game..')
        game.value = savedGame
      } catch (e) {
        console.log('Saved game not found, creating new one')
        await createNewGame()
      }
    } catch (e) {
      error.value = e as Error
      loading.value = false
    } finally {
      loading.value = false
    }
  }

  async function createNewGame() {
    game.value = await api.createGame()
    localStorage.setItem(localStorageGameIdKey, game.value.id)
  }

  async function submitGuess(guess: string): Promise<GuessDto> {
    const updatedGame = await api.submitGuess(game.value!.id, guess)
    game.value = updatedGame
    return updatedGame.guesses[updatedGame.guesses.length - 1]!
  }

  return { game, loading, error, createGame, submitGuess }
})
