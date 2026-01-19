import type { GameDto } from '@/types/backend'
import { RowState } from '@/types/rowState'
import type { Row, Tile } from '@/types/tile'
import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

export const useBoardStore = defineStore('board', () => {
  const rows = ref<Row[]>([])
  const rowsLoaded = ref(false)

  const activeRow = computed<Row | undefined>(() => {
    return rows.value.find((row) => row.state === RowState.ACTIVE)
  })

  const activeTile = computed<Tile | undefined>(() => {
    return activeRow.value?.tiles.find((tile) => tile.selected)
  })

  const activeTileNumber = computed<number | undefined>(() => {
    return activeTile?.value?.number
  })

  function initFromGame(game: GameDto) {
    if (game.guesses.length === 0) {
      rows.value = createDefaultRows(game.maxGuesses, game.word.length)
      rowsLoaded.value = true
      return
    }

    rows.value = Array.from({ length: game.maxGuesses }, (_, rowIndex): Row => {
      const guess = game.guesses[rowIndex]

      // Submitted row
      if (guess) {
        return {
          state: RowState.SUBMITTED,
          number: rowIndex + 1,
          tiles: guess.evaluations.map((e, colIndex) => ({
            letter: e.letter,
            letterState: e.state,
            selected: false,
            number: colIndex + 1,
          })),
        }
      }

      // Active row (first row after last guess)
      if (rowIndex === game.guesses.length) {
        return {
          state: RowState.ACTIVE,
          number: rowIndex + 1,
          tiles: Array.from({ length: game.word.length }, (_, colIndex) => ({
            letter: '',
            letterState: undefined,
            selected: colIndex === 0,
            number: colIndex + 1,
          })),
        }
      }

      // Inactive row
      return {
        state: RowState.INACTIVE,
        number: rowIndex + 1,
        tiles: Array.from({ length: game.word.length }, (_, colIndex) => ({
          letter: '',
          letterState: undefined,
          selected: false,
          number: colIndex + 1,
        })),
      }
    })
    rowsLoaded.value = true
  }

  function createDefaultRows(rowCount: number, columnCount: number): Row[] {
    return Array.from({ length: rowCount }, (_, rowIndex) => ({
      state: rowIndex === 0 ? RowState.ACTIVE : RowState.INACTIVE,
      number: rowIndex + 1,
      tiles: Array.from({ length: columnCount }, (_, colIndex) => ({
        letter: '',
        letterState: undefined,
        selected: rowIndex === 0 && colIndex === 0,
        number: colIndex + 1,
      })),
    }))
  }

  function setActiveTile(tileNumber: number) {
    const row = activeRow.value
    if (!row) return

    row.tiles.forEach((tile, i) => {
      tile.selected = i + 1 === tileNumber
    })
  }

  function selectNextTile() {
    if (!activeTileNumber.value) return
    setActiveTile(Math.min(5, activeTileNumber.value + 1))
  }

  function selectPreviousTile() {
    if (!activeTileNumber.value) return
    setActiveTile(Math.max(1, activeTileNumber.value - 1))
  }

  function inputLetter(letter: string) {
    if (letter === 'DELETE') {
      deleteLetter()
      return
    }

    if (letter === 'ENTER') {
      // TODO: submit guess
      return
    }

    const row = activeRow.value
    const tile = activeTile.value
    if (!row || !tile) return
    if (tile.letter) return // Prevent overwrite

    tile.letter = letter.toLocaleUpperCase()
    selectNextTile()
  }

  function deleteLetter() {
    const row = activeRow.value
    const tile = activeTile.value
    if (!row || !tile) return

    tile.letter = ''
    selectPreviousTile()
  }

  return {
    rows,
    rowsLoaded,
    activeRow,
    activeTile,
    initFromGame,
    selectNextTile,
    selectPreviousTile,
    inputLetter,
    deleteLetter,
  }
})
