<script lang="ts" setup>
import { useGameStore } from '@/stores/gameStore'

const gameStore = useGameStore()
const game = gameStore.game!
const rows = game.maxGuesses
const columns = game.word.length
</script>

<template>
  <div id="board" class="space-y-1">
    <div
      v-for="row in rows"
      :key="row"
      class="grid gap-1"
      :style="{ gridTemplateColumns: `repeat(${columns}, 5rem)` }"
    >
      <div
        v-for="column in columns"
        :key="column"
        class="w-20 h-20 rounded-sm border-2 bg-border flex items-center justify-center"
      >
        <p class="text-5xl font-bold">A</p>
      </div>
    </div>
  </div>
</template>
