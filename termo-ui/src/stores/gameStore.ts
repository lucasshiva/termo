import { useApi } from '@/composables/useApi'
import { type GameDto } from '@/types/backend'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useGameStore = defineStore('game', () => {
  const api = useApi()
  const game = ref<GameDto>()
  const loading = ref(true)
  const error = ref(false)

  async function createGame() {
    loading.value = true
    try {
      game.value = await api.createGame()
    } catch (e) {
      error.value = true
    } finally {
      loading.value = false
    }
  }

  return { game, loading, error, createGame }
})
