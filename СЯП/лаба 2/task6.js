function compare(arr1, arr2) {
    let arr3 = arr1.filter(n => !arr2.includes(n));
    return arr3;
}

let arr1 = ['hi', 'morning', 'lemon', 'plant'];
let arr2 = ['cup', 'hi', 'plant', 'lime', 'fruit'];
console.log(compare(arr1, arr2));