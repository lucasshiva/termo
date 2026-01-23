<script lang="ts" setup>
import { LetterState } from '@/types/backend';
import { RowState } from '@/types/rowState';
import type { Row, Tile } from '@/types/tile';

const props = defineProps<{
  tile: Tile
  row: Row
}>()
</script>

<template>
  <span
    class="inline-flex items-center justify-center rounded-sm transition-[border,transform] duration-75 ease-out font-bold"
    :class="{
      'border-2': row.state === RowState.INACTIVE && tile.state !== LetterState.ABSENT,
      'bg-none border-5 border-ring': row.state === RowState.ACTIVE,
      'border-b-14': tile.selected && row.state === RowState.ACTIVE,
      'bg-background': row.state === RowState.SUBMITTED || tile.state === LetterState.ABSENT,
      'bg-green-600': tile.state === LetterState.CORRECT,
      'bg-yellow-500': tile.state === LetterState.PRESENT,
    }"
  >
    {{ tile.letter.toLocaleUpperCase() }}
  </span>
</template>
