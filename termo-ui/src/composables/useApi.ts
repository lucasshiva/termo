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

  async function getGame(id: string): Promise<GameDto> {
    const response = await fetch(`${baseURL}/games/${id}`)
    if (!response.ok) throw new Error('Game not found')
    const json = await response.json()
    console.log(json)
    return json
  }

  return { createGame, getGame }
}
