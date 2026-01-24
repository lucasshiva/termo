import { GameState, LetterState, type GameDto } from '@/types/backend'
import { RowState } from '@/types/rowState'
import type { Row, Tile } from '@/types/tile'
import { defineStore, storeToRefs } from 'pinia'
import { computed, ref } from 'vue'
import { useGameStore } from './gameStore'

export const useBoardStore = defineStore('board', () => {
  const gameStore = useGameStore()
  const { game } = storeToRefs(gameStore)

  const submittedRows = computed<Row[]>(() => createSubmittedRows(game.value!))
  const emptyRows = computed<Row[]>(() => createEmptyRows(game.value!))
  const activeRow = computed<Row | undefined>(() => createActiveRow(game.value!))

  const rows = computed<Row[]>(() => {
    if (activeRow.value) {
      return [...submittedRows.value, activeRow.value, ...emptyRows.value]
    }

    return [...submittedRows.value, ...emptyRows.value]
  })
  const letterStates = computed<Map<string, LetterState>>(() => {
    if (!game.value) return new Map()

    const map = new Map<string, LetterState>()

    for (const guess of game.value.guesses) {
      for (const evaluation of guess.evaluations) {
        const letter = evaluation.letter.toLocaleUpperCase()
        const existing = map.get(letter)

        if (evaluation.state === LetterState.CORRECT) {
          map.set(letter, LetterState.CORRECT)
          continue
        }

        if (evaluation.state === LetterState.PRESENT && existing !== LetterState.CORRECT) {
          map.set(letter, LetterState.PRESENT)
          continue
        }

        if (!existing) {
          map.set(letter, LetterState.ABSENT)
        }
      }
    }

    return map
  })

  const rowsLoaded = computed(() => rows.value.length > 0)
  const activeTileIndex = ref(0)
  const activeRowLetters = ref<Map<number, string>>(new Map())
  const currentLetter = computed(() => activeRowLetters.value.get(activeTileIndex.value))

  const activeTile = computed<Tile | undefined>(() => {
    return activeRow.value?.tiles.find((tile) => tile.selected)
  })

  const currentGuess = computed<string>(() => {
    const row = activeRow.value
    if (!row) return ''

    return row.tiles
      .map((tile) => tile.letter)
      .join('')
      .trim()
      .normalize()
  })

  function createSubmittedRows(game: GameDto): Row[] {
    return game.guesses.map((guess, index) => {
      return {
        state: RowState.SUBMITTED,
        number: index + 1,
        tiles: guess.evaluations.map((e, colIndex) => ({
          letter: e.letter,
          state: e.state,
          selected: false,
          number: colIndex + 1,
        })),
      }
    })
  }

  function createEmptyRows(game: GameDto): Row[] {
    const activeRowIndex = submittedRows.value.length
    let emptyRowIndex = activeRowIndex
    if (game.state === GameState.IN_PROGRESS) {
      emptyRowIndex++
    }

    const rows: Row[] = []
    while (emptyRowIndex <= game.word.length) {
      const row = {
        state: RowState.INACTIVE,
        number: emptyRowIndex,
        tiles: Array.from({ length: game.word.length }, (_, colIndex) => ({
          letter: '',
          state: undefined,
          selected: false,
          number: colIndex + 1,
        })),
      }
      rows.push(row)
      emptyRowIndex = emptyRowIndex + 1
    }

    return rows
  }

  function createActiveRow(game: GameDto): Row | undefined {
    if (game.state !== GameState.IN_PROGRESS) {
      return
    }

    const activeRowIndex = submittedRows.value.length
    return {
      state: RowState.ACTIVE,
      number: activeRowIndex + 1,
      tiles: Array.from({ length: game.word.length }, (_, colIndex) => ({
        letter: getLetterForTileIndex(colIndex),
        state: undefined,
        selected: colIndex === activeTileIndex.value,
        number: colIndex + 1,
      })),
    }
  }

  function getLetterForTileIndex(tileIndex: number): string {
    return activeRowLetters.value.get(tileIndex) ?? ''
  }

  function setActiveTile(tileIndex: number) {
    activeTileIndex.value = tileIndex
  }

  function selectNextTile() {
    setActiveTile(Math.min(game.value!.word.length - 1, activeTileIndex.value + 1))
  }

  function selectNextEmptyTile() {
    // No empty tiles, keep selection as is
    const hasEmptyTiles = currentGuess.value.length < 5
    if (!hasEmptyTiles) {
      return
    }

    const length = game.value!.word.length
    const nextIndexes = getIndexesAfter(activeTileIndex.value, length)

    for (const index of nextIndexes) {
      const isEmpty = activeRowLetters.value.get(index) === undefined
      if (isEmpty) {
        setActiveTile(index)
        return
      }
    }
  }

  function getIndexesAfter(start: number, length: number): number[] {
    if (length <= 1) return []

    return Array.from({ length: length - 1 }, (_, i) => {
      const idx = start + 1 + i
      return idx >= length ? idx - length : idx
    })
  }

  function selectPreviousTile() {
    setActiveTile(Math.max(0, activeTileIndex.value - 1))
  }

  async function inputLetter(letter: string) {
    if (letter === 'ENTER') {
      await submitGuess()
      return
    }

    if (letter === 'DELETE') {
      deleteLetter()
      return
    }

    activeRowLetters.value.set(activeTileIndex.value, letter.toLocaleUpperCase())
    selectNextEmptyTile()
  }

  function deleteLetter() {
    const shouldDeleteCurrent = currentLetter.value != undefined
    if (shouldDeleteCurrent) {
      activeRowLetters.value.delete(activeTileIndex.value)
      selectPreviousTile()
      return
    }

    // If the current tile is empty but the previous tile is not, we delete the letter from
    // the previous tile.
    const previousTileIndex = activeTileIndex.value - 1
    const previousTileLetter = activeRowLetters.value.get(previousTileIndex)
    const canDeletePrevious = previousTileIndex >= 0 && previousTileLetter !== undefined

    if (canDeletePrevious) {
      selectPreviousTile()
      activeRowLetters.value.delete(activeTileIndex.value)
      return
    }

    selectPreviousTile()
  }

  async function submitGuess() {
    if (currentGuess.value.length !== 5) {
      console.error("Can't submit guesses of incorrect length.")
      return
    }

    console.log('Submitting guess: ', currentGuess.value)
    await gameStore.submitGuess(currentGuess.value)
    activeRowLetters.value.clear()
    activeTileIndex.value = 0
  }

  function getStateForLetter(letter: string): LetterState | undefined {
    return letterStates.value.get(letter)
  }

  return {
    rows,
    rowsLoaded,
    activeRow,
    activeTile,
    currentGuess,
    selectNextTile,
    selectPreviousTile,
    setActiveTile,
    inputLetter,
    deleteLetter,
    getStateForLetter,
  }
})
