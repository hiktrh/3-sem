let arr = [14, 18, 39];

Object.defineProperty(arr, 'sum', {
    get: function () {
        let total = 0;
        for (let i = 0; i < this.length; i++) {
            total += this[i];
        }
        return total;
    },
    enumerable: false,
    configurable: false
});

console.log(`Массив: [${arr}]`);
console.log(`Сумма элементов: ${arr.sum}`);

arr[0] = 43;
console.log(`Новая сумма: ${arr.sum}`);