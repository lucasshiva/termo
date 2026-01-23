<script setup lang="ts">
import { LetterState } from '@/types/backend';
import { RowState } from '@/types/rowState';
import type { Row, Tile } from '@/types/tile';
import BoardTile from './BoardTile.vue';
import Button from './ui/button/Button.vue';
import { Dialog, DialogContent, DialogTrigger } from './ui/dialog';

const correctTile: Tile = {letter: "T", number: 1, selected: false, state: LetterState.CORRECT}
const rowWithCorrectLetter: Row = {
  number: 1,
  state: RowState.INACTIVE,
  tiles: [
    correctTile,
    {letter: "U", number: 2, selected: false},
    {letter: "R", number: 3, selected: false},
    {letter: "M", number: 4, selected: false},
    {letter: "A", number: 5, selected: false}
  ]
}

const presentTile: Tile = {letter: "O", number: 3, selected: false, state: LetterState.PRESENT}
const rowWithLetterInWrongPosition: Row = {
  number: 1,
  state: RowState.INACTIVE,
  tiles: [
    {letter: "V", number: 1, selected: false},
    {letter: "I", number: 2, selected: false},
    presentTile,
    {letter: "L", number: 4, selected: false},
    {letter: "A", number: 5, selected: false}
  ]
}
const absentTile: Tile = {letter: "G", number: 4, selected: false, state: LetterState.ABSENT}
const rowWithAbsentWord: Row = {
  number: 1,
  state: RowState.INACTIVE,
  tiles: [
    {letter: "P", number: 1, selected: false},
    {letter: "U", number: 2, selected: false},
    {letter: "L", number: 3, selected: false},
    absentTile,
    {letter: "A", number: 5, selected: false}
  ]
}
</script>

<template>
  <Dialog>
    <DialogTrigger>
      <Button variant="outline" class="text-1xl">?</Button>
    </DialogTrigger>
    <DialogContent class="dark text-foreground bg-secondary overflow-y-auto max-h-[80vh]">
      <p>Find the correct word in 6 tries. After each try, the tiles show how close you are from getting it right.</p>


      <div class="flex gap-1">
        <BoardTile v-for="tile in rowWithCorrectLetter.tiles" :tile="tile" :row="rowWithCorrectLetter" class="w-12 h-12 text-xl" />
      </div>
      <p>The letter <BoardTile :tile="correctTile" :row="rowWithCorrectLetter" class="w-8 h-8"/> is part of the word and it is in the correct position.</p>


      <div class="flex gap-1">
        <BoardTile v-for="tile in rowWithLetterInWrongPosition.tiles" :tile="tile" :row="rowWithLetterInWrongPosition" class="w-12 h-12 text-xl"/>
      </div>
      <p>The letter <BoardTile :tile="presentTile" :row="rowWithLetterInWrongPosition" class="w-8 h-8"/> is part of the word but in another position.</p>


      <div class="flex gap-1">
        <BoardTile v-for="tile in rowWithAbsentWord.tiles" :tile="tile" :row="rowWithAbsentWord" class="w-12 h-12 text-xl" />
      </div>
      <p>The letter <BoardTile :tile="absentTile" :row="rowWithAbsentWord" class="w-8 h-8"/> is not part of the word.</p>

      <p>Words can have repeated letters.</p>

    </DialogContent>
  </Dialog>

</template>
