<script setup lang="ts">
import { useGameStore } from '@/stores/gameStore'
import { onMounted } from 'vue'
import GameScreen from './components/GameScreen.vue'
import { useBoardStore } from './stores/boardStore'

const gameStore = useGameStore()
const boardStore = useBoardStore()

onMounted(async () => {
  await gameStore.createGame()
  if (gameStore.game !== undefined) {
    boardStore.initFromGame(gameStore.game)
    console.log('Created board with rows: ', boardStore.rows)
  }
})
</script>

<template>
  <div class="h-full dark">
    <div
      v-if="gameStore.loading || !boardStore.rowsLoaded"
      class="flex items-center justify-center"
    >
      Loading...
    </div>
    <div v-else-if="gameStore.error" class="flex items-center justify-center">
      Error creating game: {{ gameStore.error.message }}
    </div>
    <GameScreen v-else class="h-full" />
  </div>
</template>
