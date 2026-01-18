<script setup lang="ts">
import { useGameStore } from '@/stores/gameStore'
import { onMounted } from 'vue'
import GameScreen from './components/GameScreen.vue'

const gameStore = useGameStore()

onMounted(async () => {
  await gameStore.createGame()
})
</script>

<template>
  <div class="h-full dark">
    <div v-if="gameStore.loading" class="flex items-center justify-center">Loading...</div>
    <div v-else-if="gameStore.error" class="flex items-center justify-center">
      Error creating game: {{ gameStore.error.message }}
    </div>
    <GameScreen v-else class="h-full" />
  </div>
</template>
