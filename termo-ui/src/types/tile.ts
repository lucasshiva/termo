import type { LetterState } from './backend'
import type { RowState } from './rowState'

export interface Tile {
  letter: string
  state?: LetterState
  selected: boolean
  number: number // 1-based
}

export interface Row {
  tiles: Tile[]
  state: RowState
  number: number // 1-based
}
