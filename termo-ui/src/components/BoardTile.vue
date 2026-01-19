<script lang="ts" setup>
import { RowState } from '@/types/rowState';
import type { Row, Tile } from '@/types/tile';

const props = defineProps<{
  tile: Tile
  row: Row
}>()
</script>

<template>
  <div
    class="rounded-sm flex items-center justify-center transition-[border,transform] duration-75 ease-out"
    style="width: var(--tile); height: var(--tile); font-size: var(--font)"
    :class="{
      'border-2': row.state === RowState.INACTIVE,
      'bg-none border-3 border-ring': row.state === RowState.ACTIVE,
      'border-b-12': tile.selected && row.state === RowState.ACTIVE,
    }"
  >
    <p class="font-bold">{{ tile.letter }}</p>
  </div>
</template>
