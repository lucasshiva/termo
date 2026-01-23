<script setup lang="ts">
import type { LetterState } from '@/types/backend';

  const props = defineProps<{
    letter: string,
    state?: LetterState
  }>()
</script>

<template>
  <div class="tile flex items-center justify-center rounded-sm">
    <slot />
  </div>
</template>
