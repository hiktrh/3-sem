function addValue(map,key,value){
map.set(key, value);
console.log('товар успешно добавлен!');

}

function delete_map(map,key){
    map.delete(key);
}

function deleteProdByName(map, name) {
 for (const [key, product] of map.entries()) {
    if (product.name === name) {
        map.delete(key);
    }
}
}

function updateQuantity(map, key, sizepp) {
   if (map.has(key)) {
     map.get(key).count =sizepp;
   }
}

function updatePrice(map,id, newPrice) {
  if (map.has(id)) {
        map.get(id).price = newPrice;
    }
}

function countItems(map) {
  return map.size;
}

function calculateTotalPrice() {
    let total = 0;
       for (const product of products.values()) {
       total += product.count * product.price;
    }
    return total;
}

let products= new Map();
let pr1={
    name:'соса сола',
    count: 12,
    price: 4
}
let pr2={
    name:'гранатовый сок',
    count: 228,
    price: 12
}
let pr3={
    name:'сырные снеки',
    count: 17,
    price: 35
}
let pr4={
    name:'сосиска',
    count: 5,
    price: 2
}

addValue(products,1,pr1);
addValue(products,2,pr2);
addValue(products,3,pr3);
addValue(products,4,pr4);
delete_map(products, 4);
console.log(products);
deleteProdByName(products,'соса сола');
updateQuantity(products, 3, 123);
updatePrice(products, 2, 1234);
console.log(products);
console.log(countItems(products));
