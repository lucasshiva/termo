<script lang="ts" setup>
import { useBoardStore } from '@/stores/boardStore'
import { GameState, LetterState } from '@/types/backend'
import { RowState } from '@/types/rowState'
import type { Row, Tile } from '@/types/tile'
import { ref } from 'vue'

const props = defineProps<{
  tile: Tile
  row: Row
}>()

const boardStore = useBoardStore()
const revealed = ref(false)

function updateTileFromEvaluation() {
  revealed.value = true

  const guess = boardStore.lastGuess
  if (!guess) return

  const tileIndex = props.tile.number - 1
  const evaluation = guess.evaluations[tileIndex]!

  props.tile.letter =
    evaluation.state === LetterState.CORRECT ? evaluation.display : evaluation.letter
  props.tile.selected = false
  props.tile.state = evaluation.state

  const isLastTile = props.tile.number === 5
  if (isLastTile) {
    props.row.state = RowState.SUBMITTED
    boardStore.setIsRevealing(false)

    const game = boardStore.game!
    const canContinue = game.state === GameState.IN_PROGRESS && props.row.number < game.maxGuesses
    if (canContinue) {
      const nextRow = boardStore.rows[props.row.number]!
      nextRow.state = RowState.ACTIVE
      boardStore.activeRow = nextRow
      boardStore.setActiveTile(0)
    }
  }
}
</script>

<template>
  <span
    class="letter"
    :class="{
      'border-2': row.state === RowState.INACTIVE && tile.state !== LetterState.ABSENT,
      'bg-none border-5 border-ring cursor-pointer':
        row.state === RowState.ACTIVE && !boardStore.isRevealing,
      'border-b-14': tile.selected && row.state === RowState.ACTIVE && !boardStore.isRevealing,
      'bg-background': tile.state === LetterState.ABSENT,
      'bg-green-600': tile.state === LetterState.CORRECT,
      'bg-yellow-600': tile.state === LetterState.PRESENT,
      'flip border-2': boardStore.isRevealing && !revealed && row.state === RowState.ACTIVE,
    }"
    :style="{ animationDelay: `${(tile.number - 1) * 360}ms` }"
    @animationend="updateTileFromEvaluation()"
  >
    {{ tile.letter.toLocaleUpperCase() }}
  </span>
</template>
