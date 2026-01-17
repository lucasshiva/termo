<script setup lang="ts">
import { useGameStore } from '@/stores/gameStore'
import { onMounted } from 'vue'

const gameStore = useGameStore()

onMounted(async () => {
  await gameStore.createGame()
})
</script>

<template>
  <div class="h-full flex items-center justify-center">
    <div v-if="gameStore.loading">Loading...</div>
    <div v-else-if="gameStore.error">Error creating game: {{ gameStore.error.message }}</div>
    <div v-else>Created game: {{ gameStore.game }}</div>
  </div>
</template>
