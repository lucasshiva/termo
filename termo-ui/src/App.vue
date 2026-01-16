<script setup lang="ts">
import { onMounted } from 'vue'
import { useGameStore } from './stores/gameStore'

const gameStore = useGameStore()

onMounted(async () => {
  await gameStore.createGame()
})
</script>

<template>
  <div class="h-full flex items-center justify-center">
    <div v-if="gameStore.loading">Loading...</div>
    <div v-else-if="gameStore.error">Error creating game!</div>
    <div v-else>Created game: {{ gameStore.game }}</div>
  </div>
</template>
