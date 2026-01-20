<script lang="ts" setup>
import { useBoardStore } from '@/stores/boardStore';
import KeyboardKey from './KeyboardKey.vue';

const keys = [
  ['Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P'],
  ['A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'DELETE'],
  ['Z', 'X', 'C', 'V', 'B', 'N', 'M', 'ENTER'],
]
const boardStore = useBoardStore()
</script>

<template>
  <div>
    <div
      v-for="(row, index) in keys"
      :key="index"
      class="flex gap-0.5 sm:gap-1 m-0.5 sm:m-1 items-center justify-center"
    >
      <KeyboardKey
        v-for="letter in row"
        :letter="letter"
        :state="boardStore.getStateForLetter(letter)"
        @click="boardStore.inputLetter(letter)"
      />
    </div>
  </div>
</template>
