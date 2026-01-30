<script setup lang="ts">
import { useGameStore } from '@/stores/gameStore'
import { onMounted } from 'vue'
import GameScreen from './components/GameScreen.vue'
import { useBoardStore } from './stores/boardStore'

const gameStore = useGameStore()
const boardStore = useBoardStore()

onMounted(async () => {
  await gameStore.createGame()
  boardStore.initRowsFromGame(gameStore.game!)
})
</script>

<template>
  <div class="h-full dark bg-secondary text-foreground">
    <div
      v-if="gameStore.loading && !boardStore.rowsLoaded"
      class="flex items-center justify-center h-full"
    >
      Loading...
    </div>
    <div v-else-if="gameStore.error" class="flex items-center justify-center h-full">
      Error creating game: {{ gameStore.error.message }}
    </div>
    <GameScreen v-else class="h-full" />
  </div>
</template>
