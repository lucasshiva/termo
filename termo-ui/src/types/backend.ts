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
  CORRECT = 'Correct',
  PRESENT = 'Present',
  ABSENT = 'Absent',
}

export enum GameState {
  IN_PROGRESS = 'InProgress',
  WON = 'Won',
  LOST = 'Lost',
}

export interface SubmitGuessRequest {
  guess: string
}
