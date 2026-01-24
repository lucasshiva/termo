<script lang="ts" setup>
import { useGameStore } from '@/stores/gameStore'
import { Eye, EyeOff } from 'lucide-vue-next'
import { ref } from 'vue'
import Board from './Board.vue'
import GameHeader from './GameHeader.vue'
import Keyboard from './Keyboard.vue'
import { Button } from './ui/button'

const gameStore = useGameStore()
const showWord = ref(false)
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
        <div class="flex items-center gap-2">
          <div v-if="showWord">WORD: {{ gameStore.game!.word.displayText }}</div>

          <Button variant="outline" @click="showWord = !showWord">
            <Eye v-if="!showWord" />
            <EyeOff v-else />
          </Button>
        </div>

        <Board />

        <!-- bottom spacer -->
        <div />

        <Keyboard class="mb-4" />
      </div>
    </div>
  </div>
</template>
