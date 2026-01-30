<script setup lang="ts">
import { useGameStore } from '@/stores/gameStore'
import { LucideCircleX } from 'lucide-vue-next'
import { onMounted } from 'vue'
import GameScreen from './components/GameScreen.vue'
import { Button } from './components/ui/button'
import { Empty, EmptyDescription, EmptyHeader, EmptyMedia, EmptyTitle } from './components/ui/empty'
import EmptyContent from './components/ui/empty/EmptyContent.vue'
import { useBoardStore } from './stores/boardStore'

const gameStore = useGameStore()
const boardStore = useBoardStore()

async function init() {
  await gameStore.createGame()
  boardStore.initRowsFromGame(gameStore.game!)
}

onMounted(async () => {
  await init()
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

    <Empty v-else-if="gameStore.error" class="h-full">
      <EmptyHeader>
        <EmptyMedia variant="icon" class="bg-background">
          <LucideCircleX />
        </EmptyMedia>
        <EmptyTitle>Error loading/creating game</EmptyTitle>
        <EmptyDescription>
          {{ gameStore.error.message }}
        </EmptyDescription>
      </EmptyHeader>
      <EmptyContent>
        <Button variant="default" class="bg-background" @click="init()">Try again</Button>
      </EmptyContent>
    </Empty>
    <GameScreen v-else class="h-full" />
  </div>
</template>
