let item = {
    price: 20,
}

Object.defineProperty(item, "price", {
    writable: true,
    configurable: true,
    enumerable: true
});

console.log(item.price);

item.price = 30;
console.log(item.price);

delete item.price;
console.log(item.price);

Object.defineProperty(item, "price", {
    value: 20,
    writable: false,
    configurable: false,
    enumerable: true
});

console.log(item.price);

item.price = 30;
console.log(item.price);

delete item.price;
console.log(item.price);