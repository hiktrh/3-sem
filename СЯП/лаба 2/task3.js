function arithmeticOfTheArray (array) {
    let sum = 0;
    for(i = 0; i < array.length; i++){
        sum += (array[i]);
    }
    let result = sum / array.length;
    return result;
}

let numbers = arithmeticOfTheArray([3, 6, 5, 2, 4]);
console.log(numbers);