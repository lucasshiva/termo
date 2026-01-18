<script setup lang="ts">
import { Delete } from 'lucide-vue-next';
import { computed } from 'vue';

const props = defineProps<{ letter: string }>()
const isSpecial = computed(() => props.letter === 'ENTER' || props.letter === 'DELETE')
</script>

<template>
  <div
    class="bg-background rounded-sm text-3xl min-w-16 p-4 font-medium flex items-center justify-center"
    :class="isSpecial ? 'ml-2' : 'ml-0'"
  >
    <Delete v-if="letter === 'DELETE'" :size="40" />
    <span v-else>{{ letter }}</span>
  </div>
</template>
