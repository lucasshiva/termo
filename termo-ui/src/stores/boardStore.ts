import { GameState, LetterState, type GameDto, type GuessDto } from '@/types/backend'
import { RowState } from '@/types/rowState'
import type { Row, Tile } from '@/types/tile'
import { defineStore, storeToRefs } from 'pinia'
import { computed, ref, watch } from 'vue'
import { useGameStore } from './gameStore'

export const useBoardStore = defineStore('board', () => {
  const gameStore = useGameStore()
  const { game } = storeToRefs(gameStore)

  const rows = ref<Row[]>([])
  const submittedRows = computed(() => rows.value.filter((r) => r.state === RowState.SUBMITTED))
  const inactiveRows = computed(() => rows.value.filter((r) => r.state === RowState.INACTIVE))
  const rowsLoaded = ref(false)
  const activeRow = ref<Row>()

  // Helps the UI animate guesses
  const isRevealing = ref(false)
  const lastGuess = ref<GuessDto | undefined>()

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

  const currentLetter = computed(() => {
    if (!activeTile.value) return

    return activeTile.value.letter
  })

  const activeTileIndex = ref(0)

  // Update selection when index changes
  watch(
    activeTileIndex,
    (index) => {
      const row = activeRow.value
      if (!row) return
      row.tiles.map((t) => {
        if (t.number === index + 1) {
          t.selected = true
        } else {
          t.selected = false
        }
      })
    },
    { immediate: true },
  )

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

  function initRowsFromGame(game: GameDto) {
    const submittedRows = game.guesses.map((guess, index): Row => {
      return {
        state: RowState.SUBMITTED,
        number: index + 1, // 1-based,
        tiles: guess.evaluations.map(
          (e, colIndex): Tile => ({
            letter: e.state === LetterState.CORRECT ? e.display : e.letter,
            state: e.state,
            selected: false,
            number: colIndex + 1,
          }),
        ),
      }
    })

    const activeRowIndex = submittedRows.length
    let active: Row
    // No active row if a game is won or lost. Add an empty one instead.
    if (game.state !== GameState.IN_PROGRESS) {
      active = {
        number: activeRowIndex + 1,
        state: RowState.INACTIVE,
        tiles: Array.from(
          { length: game.word.length },
          (_, colIndex): Tile => ({
            letter: '',
            selected: false,
            number: colIndex + 1,
          }),
        ),
      }
    } else {
      active = {
        number: activeRowIndex + 1,
        state: RowState.ACTIVE,
        tiles: Array.from(
          { length: game.word.length },
          (_, colIndex): Tile => ({
            letter: '',
            selected: colIndex === 0,
            number: colIndex + 1,
          }),
        ),
      }
    }

    let emptyRoxIndex = activeRowIndex + 1
    const emptyRows: Row[] = []
    while (emptyRoxIndex <= game.word.length) {
      emptyRows.push({
        number: emptyRoxIndex + 1,
        state: RowState.INACTIVE,
        tiles: Array.from(
          { length: game.word.length },
          (_, colIndex): Tile => ({
            letter: '',
            selected: false,
            number: colIndex + 1,
          }),
        ),
      })
      emptyRoxIndex++
    }

    rowsLoaded.value = true
    rows.value = [...submittedRows, active, ...emptyRows]

    // This way we can manipulate the activeRow without updating the UI, which will let us
    // animate the results of a guess before updating the UI.
    activeRow.value = active
  }

  function setActiveTile(tileIndex: number) {
    activeTileIndex.value = tileIndex
  }

  function selectNextTile() {
    const nextTileIndex = Math.min(game.value!.word.length - 1, activeTileIndex.value + 1)
    setActiveTile(nextTileIndex)
  }

  function selectNextEmptyTile() {
    // Game is not in a state where we can select tiles.
    if (!activeTile.value || !activeRow.value) return

    // No empty tiles, keep selection as is
    const hasEmptyTiles = currentGuess.value.length < 5
    if (!hasEmptyTiles) {
      return
    }

    const length = game.value!.word.length
    const nextIndexes = getIndexesAfter(activeTileIndex.value, length)

    for (const index of nextIndexes) {
      const tileAtIndex = activeRow.value.tiles[index]
      const isEmpty = tileAtIndex === undefined || tileAtIndex.letter === ''
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
    if (!activeTile.value) return

    const previousTileIndex = Math.max(0, activeTileIndex.value - 1)
    setActiveTile(previousTileIndex)
  }

  async function inputLetter(letter: string) {
    if (!activeTile.value) return

    if (letter === 'ENTER') {
      await submitGuess()
      return
    }

    if (letter === 'DELETE') {
      deleteLetter()
      return
    }

    activeTile.value.letter = letter
    selectNextEmptyTile()
  }

  function deleteLetter() {
    if (!activeTile.value || !activeRow.value) return

    const shouldDeleteCurrent = currentLetter.value !== undefined && currentLetter.value !== ''
    console.log('current', shouldDeleteCurrent)
    if (shouldDeleteCurrent) {
      activeTile.value.letter = ''
      selectPreviousTile()
      return
    }

    // If the current tile is empty but the previous tile is not, we delete the letter from
    // the previous tile.
    const previousTileIndex = activeTileIndex.value - 1
    const previousTile = activeRow.value.tiles[previousTileIndex]
    const canDeletePrevious = previousTileIndex >= 0 && previousTile?.letter !== ''

    console.log('previous', canDeletePrevious)
    if (canDeletePrevious) {
      // Selecting then deleting the letter from the activeTile doesn't seem to work.
      // Maybe updates are delayed.
      previousTile!.letter = ''
      selectPreviousTile()
      return
    }

    selectPreviousTile()
  }

  async function submitGuess() {
    const row = activeRow.value
    if (!row) return

    if (currentGuess.value.length !== 5) {
      // TODO: Show error in the UI.
      console.error("Can't submit guesses of incorrect length.")
      return
    }

    console.log('Submitting guess:', currentGuess.value)
    try {
      isRevealing.value = true
      const result = await gameStore.submitGuess(currentGuess.value)
      lastGuess.value = result
    } catch (error) {
      console.error(error)
    }
  }

  function setIsRevealing(value: boolean) {
    isRevealing.value = value
  }

  function getStateForLetter(letter: string): LetterState | undefined {
    return letterStates.value.get(letter)
  }

  return {
    game,
    rows,
    rowsLoaded,
    activeRow,
    activeTile,
    currentGuess,
    isRevealing,
    lastGuess,
    selectNextTile,
    selectPreviousTile,
    setActiveTile,
    inputLetter,
    deleteLetter,
    getStateForLetter,
    initRowsFromGame,
    setIsRevealing,
  }
})
