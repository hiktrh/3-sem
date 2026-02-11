class Sudoku {
    constructor() {
        this.board = this.createEmptyBoard();
    }

    createEmptyBoard() {
        return Array(9).fill().map(() => Array(9).fill(0));
    }

    checkRows() {
        for (let i = 0; i < 9; i++) {
            let seen = new Set();
            for (let j = 0; j < 9; j++) {
                let num = this.board[i][j];
                if (num !== 0 && seen.has(num)) {
                    console.log(`Ошибка в строке ${i + 1}`);
                    break;
                }
                seen.add(num);
            }
        }
    }

    checkColumns() {
        for (let j = 0; j < 9; j++) {
            let seen = new Set();
            for (let i = 0; i < 9; i++) {
                let num = this.board[i][j];
                if (num !== 0 && seen.has(num)) {
                    console.log(`Ошибка в столбце ${j + 1}`);
                    break;
                }
                seen.add(num);
            }
        }
    }

    checkBoxes() {
        for (let boxRow = 0; boxRow < 3; boxRow++) {
            for (let boxCol = 0; boxCol < 3; boxCol++) {
                let seen = new Set();
                for (let i = 0; i < 3; i++) {
                    for (let j = 0; j < 3; j++) {
                        let num = this.board[boxRow * 3 + i][boxCol * 3 + j];
                        if (num !== 0 && seen.has(num)) {
                            console.log(`Ошибка в квадрате ${boxRow * 3 + boxCol + 1}`);
                            break;
                        }
                        seen.add(num);
                    }
                }
            }
        }
    }

    resetBoard() {
        this.board = this.createEmptyBoard();
    }

    checkBoard() {
        this.checkRows();
        this.checkColumns();
        this.checkBoxes();
    }

    generateSolvedBoard() {
        this.board = [
            [5, 3, 4, 6, 7, 8, 9, 1, 2],
            [6, 7, 2, 1, 9, 5, 3, 4, 8],
            [1, 9, 8, 3, 4, 2, 5, 6, 7],
            [8, 5, 9, 7, 1, 1, 4, 2, 3],
            [4, 2, 6, 8, 5, 3, 7, 9, 1],
            [7, 1, 3, 9, 2, 4, 8, 5, 6],
            [9, 6, 1, 5, 3, 7, 2, 8, 4],
            [2, 8, 7, 4, 1, 9, 6, 3, 5],
            [3, 4, 5, 2, 8, 6, 1, 7, 9]
        ];
    }
}

let sudoku = new Sudoku();
sudoku.generateSolvedBoard();
sudoku.checkBoard(); 
sudoku.resetBoard();