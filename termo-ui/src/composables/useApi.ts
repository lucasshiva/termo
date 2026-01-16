import type { GameDto } from '@/types/backend'

// composables/useApi.ts
export function useApi() {
  const baseURL = '/api'

  async function createGame(): Promise<GameDto> {
    const response = await fetch(`${baseURL}/games`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
    })
    if (!response.ok) throw new Error('Failed to create game')
    return response.json()
  }

  return { createGame }
}
