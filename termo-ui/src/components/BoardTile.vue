<script lang="ts" setup>
import { LetterState } from '@/types/backend'
import { RowState } from '@/types/rowState'
import type { Row, Tile } from '@/types/tile'

const props = defineProps<{
  tile: Tile
  row: Row
}>()
</script>

<template>
  <span
    class="letter"
    :class="{
      'border-2': row.state === RowState.INACTIVE && tile.state !== LetterState.ABSENT,
      'bg-none border-5 border-ring cursor-pointer': row.state === RowState.ACTIVE,
      'border-b-14': tile.selected && row.state === RowState.ACTIVE,
      'bg-background': tile.state === LetterState.ABSENT,
      'bg-green-600': tile.state === LetterState.CORRECT,
      'bg-yellow-600': tile.state === LetterState.PRESENT,
    }"
  >
    {{ tile.letter.toLocaleUpperCase() }}
  </span>
</template>
