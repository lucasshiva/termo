<script lang="ts" setup>
import Board from './Board.vue';
import GameHeader from './GameHeader.vue';
import Keyboard from './Keyboard.vue';
</script>

<template>
  <div
    class="bg-accent flex items-center justify-center h-screen overflow-hidden text-accent-foreground"
  >
    <div class="h-full w-3xl flex flex-col">
      <GameHeader />

      <main class="flex items-center justify-center mt-12 flex-1">
        <Board />
      </main>

      <div class="mb-4">
        <Keyboard />
      </div>
    </div>
  </div>
</template>
