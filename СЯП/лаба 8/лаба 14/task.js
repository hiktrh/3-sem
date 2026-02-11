class Sudoku {
    constructor() {
        this.board = this.createEmptyBoard();
    }

    createEmptyBoard() {
        return Array(9).fill().map(function() {
            return Array(9).fill(0);
        });
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
            [8, 5, 9, 7, 6, 1, 4, 2, 3],
            [4, 2, 6, 8, 5, 3, 7, 9, 1],
            [7, 1, 3, 9, 2, 4, 8, 5, 6],
            [9, 6, 1, 5, 3, 7, 2, 8, 4],
            [2, 8, 7, 4, 1, 9, 6, 3, 5],
            [3, 4, 5, 2, 8, 6, 1, 7, 9]
        ];
    }
   printBoard() {
    console.log("Поле Судоку:\n");
    for (let i = 0; i < 9; i++) {
        console.log(this.board[i].join(" "));
    }
}
    }


let sudoku = new Sudoku();
const boardElement = document.getElementById("sudoku-board");

function renderBoard() {
  boardElement.innerHTML = "";
  for (let i = 0; i < 9; i++) {
    const row = document.createElement("tr");
    for (let j = 0; j < 9; j++) {
      const cell = document.createElement("td");
      const input = document.createElement("input");
      input.type = "text";
      input.maxLength = 1;
      input.value = sudoku.board[i][j] === 0 ? "" : sudoku.board[i][j];
      input.addEventListener("input", () => {
        const val = parseInt(input.value);
        sudoku.board[i][j] = isNaN(val) ? 0 : val;
      });
      cell.appendChild(input);
      row.appendChild(cell);
    }
    boardElement.appendChild(row);
  }
}

function highlightErrors() {
  document.querySelectorAll("td").forEach(td => {
    td.classList.remove("error", "success");
  });

  let hasError = false;

  for (let i = 0; i < 9; i++) {
    let seen = new Set();
    for (let j = 0; j < 9; j++) {
      let num = sudoku.board[i][j];
      if (num !== 0 && seen.has(num)) {
        hasError = true;
       for (let c = 0; c < 9; c++) {
      boardElement.rows[i].cells[c].classList.add("error");
       }
      }
      seen.add(num);
    }
  }

  for (let j = 0; j < 9; j++) {
    let seen = new Set();
    for (let i = 0; i < 9; i++) {
      let num = sudoku.board[i][j];
      if (num !== 0 && seen.has(num)) {
        hasError = true;
        for (let r = 0; r < 9; r++) {
          boardElement.rows[r].cells[j].classList.add("error");
        }
      }
      seen.add(num);
    }
  }

  for (let boxRow = 0; boxRow < 3; boxRow++) {
    for (let boxCol = 0; boxCol < 3; boxCol++) {
      let seen = new Set();
      for (let i = 0; i < 3; i++) {
        for (let j = 0; j < 3; j++) {
          let row = boxRow * 3 + i;
          let col = boxCol * 3 + j;
          let num = sudoku.board[row][col];
          if (num !== 0 && seen.has(num)) {
            hasError = true;
            for (let ii = 0; ii < 3; ii++) {
              for (let jj = 0; jj < 3; jj++) {
                boardElement.rows[boxRow*3+ii].cells[boxCol*3+jj].classList.add("error");
              }
            }
          }
          seen.add(num);
        }
      }
    }
  }

  if (!hasError) {
    let filled = sudoku.board.every(row => row.every(num => num !== 0));
    if (filled) {
      document.querySelectorAll("td").forEach(td => td.classList.add("success"));
    }
  }
}



document.getElementById("puzzleBoard").addEventListener("click", () => {
    sudoku.generatePuzzle();
    renderBoard();
});

document.getElementById("checkBoard").addEventListener("click", () => {
  highlightErrors();
});

document.getElementById("solvedBoard").addEventListener("click", () => {
  sudoku.generateSolvedBoard();
  renderBoard();
});

renderBoard();
