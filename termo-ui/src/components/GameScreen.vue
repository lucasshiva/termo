<script lang="ts" setup>
import Board from './Board.vue';
import GameHeader from './GameHeader.vue';
</script>

<template>
  <div class="bg-accent flex items-center justify-center min-h-screen text-accent-foreground">
    <div class="border-2 border-amber-50 h-full w-5xl">
      <GameHeader />

      <main class="flex items-center justify-center h-full">
        <Board />
      </main>
    </div>
  </div>
</template>
