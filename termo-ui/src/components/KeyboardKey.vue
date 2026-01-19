<script setup lang="ts">
import { Delete } from 'lucide-vue-next';
import { computed } from 'vue';

const props = defineProps<{ letter: string }>()
const isSpecial = computed(() => props.letter === 'ENTER' || props.letter === 'DELETE')
const isEnter = computed(() => props.letter === 'ENTER')
</script>

<template>
  <div
    class="bg-background rounded-sm font-medium p-4 flex items-center justify-center cursor-pointer hover:bg-primary"
    :class="{
      'px-10': isEnter,
      'ml-2': isSpecial,
    }"
    style="min-width: var(--key); height: var(--key); font-size: var(--font-key)"
  >
    <Delete v-if="letter === 'DELETE'" :size="30" />
    <span v-else>{{ letter }}</span>
  </div>
</template>
