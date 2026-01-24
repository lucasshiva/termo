<script lang="ts" setup>
import { useBoardStore } from '@/stores/boardStore'
import { RowState } from '@/types/rowState'
import type { Row, Tile } from '@/types/tile'
import { onMounted, onUnmounted } from 'vue'
import BoardTile from './BoardTile.vue'

const boardStore = useBoardStore()

function onKeyDown(e: KeyboardEvent) {
  const key = e.key

  if (key === 'ArrowLeft') {
    boardStore.selectPreviousTile()
  }
  if (key === 'ArrowRight') {
    boardStore.selectNextTile()
  }

  if (key === 'Enter') {
    boardStore.inputLetter('ENTER')
    return
  }

  if (key === 'Backspace') {
    boardStore.inputLetter('DELETE')
    return
  }

  const normalized = key.normalize()

  if (/^[a-zA-Z]$/.test(normalized)) {
    boardStore.inputLetter(normalized)
  }
}

function selectTile(row: Row, tile: Tile) {
  if (row.state !== RowState.ACTIVE) {
    return
  }

  console.log('Setting active tile to', tile)
  boardStore.setActiveTile(tile.number - 1)
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
      <BoardTile
        v-for="tile in row.tiles"
        :tile="tile"
        :row="row"
        @click="selectTile(row, tile)"
        class="w-[8.5vh] h-[8.5vh] text-[5vh]"
      />
    </div>
  </div>
</template>
