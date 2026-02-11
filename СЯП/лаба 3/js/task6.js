function piramid(stages){
  for (let i = 1; i <= stages; i++) {
    let row = '';

    for (let j = 1; j <= stages - i; j++) { 
      row += ' ';
    }
    for (let k = 1; k <= 2 * i - 1; k++) { 
      row += '*';
    }
    console.log(row);
  }
}

piramid(5);