<script lang="ts" setup>
import { useBoardStore } from '@/stores/boardStore'
import { useGameStore } from '@/stores/gameStore'
import KeyboardKey from './KeyboardKey.vue'

const keys = [
  ['Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P'],
  ['A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'DELETE'],
  ['Z', 'X', 'C', 'V', 'B', 'N', 'M', 'ENTER'],
]
const boardStore = useBoardStore()
const gameStore = useGameStore()

// TODO: This shouldn't be here, but moving things will require some refactoring.
async function inputLetter(letter: string) {
  if (letter === 'ENTER') {
    const game = gameStore.game!
    const currentGuess = boardStore.currentGuess
    if (currentGuess.length !== game.word.length) {
      // Show notification and shake row.
      console.log("Can't submit words fewer than 5 chars: ", currentGuess)
      return
    }

    console.log('Submitting guess: ', currentGuess)
    await gameStore.submitGuess(game.id, currentGuess.replace(' ', ''))
    const updatedGame = gameStore.game!
    console.log('Updated game: ', updatedGame)
    boardStore.initFromGame(updatedGame)
  }

  if (letter === 'DELETE') {
    boardStore.deleteLetter()
    return
  }

  boardStore.inputLetter(letter)
}
</script>

<template>
  <div>
    <div
      v-for="(row, index) in keys"
      :key="index"
      class="flex gap-0.5 sm:gap-1 m-0.5 sm:m-1 items-center justify-center"
    >
      <KeyboardKey v-for="letter in row" :letter="letter" @click="inputLetter(letter)" />
    </div>
  </div>
</template>
