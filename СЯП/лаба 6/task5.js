function sumValues(x, y, z) {
    return x + y + z;
}

let arr = [1, 2, 3];

let result = sumValues(...arr);
console.log(result);