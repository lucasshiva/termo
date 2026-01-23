<script lang="ts" setup>
import { LucideSettings } from 'lucide-vue-next';
import HelpDialogButton from './HelpDialogButton.vue';
import Button from './ui/button/Button.vue';
</script>

<template>
  <div class="m-4 flex items-center justify-between select-none text-center">
    <HelpDialogButton />
    <h1 class="text-3xl font-medium">TERMO</h1>
    <Button variant="outline">
      <LucideSettings />
    </Button>
  </div>
</template>
