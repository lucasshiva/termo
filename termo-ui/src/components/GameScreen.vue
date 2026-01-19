<script lang="ts" setup>
import { useGameStore } from '@/stores/gameStore'
import Board from './Board.vue'
import GameHeader from './GameHeader.vue'
import Keyboard from './Keyboard.vue'

const gameStore = useGameStore()
</script>

<template>
  <div class="bg-accent h-screen overflow-hidden text-accent-foreground grid place-items-center">
    <div class="h-full grid grid-rows-[auto_1fr]">
      <GameHeader />

      <div
        class="min-h-0 grid overflow-hidden grid-rows-[1fr_auto_1fr_auto] place-items-center mt-4"
      >
        <!-- top spacer -->

        <!-- Display word for debug purposes -->
        <div>WORD: {{ gameStore.game!.word.displayText }}</div>

        <Board />

        <!-- bottom spacer -->
        <div />

        <Keyboard class="mb-4" />
      </div>
    </div>
  </div>
</template>
