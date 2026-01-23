<script lang="ts" setup>
import { useBoardStore } from '@/stores/boardStore'
import { onMounted, onUnmounted } from 'vue'
import BoardTile from './BoardTile.vue'

const boardStore = useBoardStore()

function onKeyDown(e: KeyboardEvent) {
  if (e.key === 'ArrowLeft') {
    boardStore.selectPreviousTile()
  }
  if (e.key === 'ArrowRight') {
    boardStore.selectNextTile()
  }
}

onMounted(() => {
  window.addEventListener('keydown', onKeyDown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', onKeyDown)
})
</script>

<template>
  <div id="board" class="space-y-1">
    <div v-for="row in boardStore.rows" class="grid gap-1 grid-cols-5">
      <BoardTile v-for="tile in row.tiles" :tile="tile" :row="row" class="w-[8.5vh] h-[8.5vh] text-[5vh] flex items-center justify-center" />
    </div>
  </div>
</template>
