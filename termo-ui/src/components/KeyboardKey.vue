<script setup lang="ts">
import { LetterState } from '@/types/backend'
import { Delete } from 'lucide-vue-next'
import { computed } from 'vue'

const props = defineProps<{ letter: string; state?: LetterState }>()
const isSpecial = computed(() => props.letter === 'ENTER' || props.letter === 'DELETE')
const isEnter = computed(() => props.letter === 'ENTER')
</script>

<template>
  <div
    class="p-4 letter cursor-pointer hover:bg-primary min-w-[6.5vh] h-[6.5vh] text-[2.5vh]"
    :class="{
      'px-10': isEnter,
      'ml-2 bg-background': isSpecial,
      'bg-background': state === undefined,
      'bg-green-600': state === LetterState.CORRECT,
      'bg-yellow-600': state === LetterState.PRESENT,
      'bg-none border': state === LetterState.ABSENT,
    }"
  >
    <Delete v-if="letter === 'DELETE'" :size="30" />
    <span v-else>{{ letter }}</span>
  </div>
</template>
