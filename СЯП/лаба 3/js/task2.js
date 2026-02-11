let arr2 = [1, 2, [3, 4, [5, 6, [7, 8, [9, 10]]]]];

let summ = arr2.flat(Infinity).reduce((summ, current) => summ + current, 0);

console.log(summ);