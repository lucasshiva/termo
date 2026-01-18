<script lang="ts" setup>
import { LucideSettings } from 'lucide-vue-next';
import Button from './ui/button/Button.vue';
</script>

<template>
  <div class="m-4 flex items-center justify-between select-none text-center">
    <Button variant="outline">
      <p class="text-xl">?</p>
    </Button>
    <h1 class="text-3xl font-medium">TERMO</h1>
    <Button variant="outline">
      <LucideSettings />
    </Button>
  </div>
</template>
