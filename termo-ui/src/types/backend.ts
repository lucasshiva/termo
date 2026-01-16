export interface GameDto {
  id: string
  word: Word
  maxGuesses: number
  guesses: GuessDto[]
  state: GameState
}

export interface Word {
  value: string
  displayText: string
  length: number
}

export interface GuessDto {
  value: string
  evaluations: LetterEvaluation[]
}

export interface LetterEvaluation {
  letter: string
  state: LetterState
}

export enum LetterState {
  CORRECT = 'correct',
  PRESENT = 'present',
  ABSENT = 'absent',
}

export enum GameState {
  IN_PROGRESS = 'in_progress',
  WON = 'won',
  LOST = 'lost',
}
