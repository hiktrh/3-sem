let cache = new WeakMap();

function cachData(obj) {
    if (!cache.has(obj)) {
        console.log("Добавляю в кэш данные")
        let result = multipleValue(obj);
        cache.set(obj, result);

        return result;
    }

    console.log("Беру данные из кэша");
    return cache.get(obj);
}

function multipleValue(obj) {
    return obj.value * 2;
}

let objValue = { value: 29 };   

let result1 = cashData(objValue);
console.log(result1);

let result2 = cashData(objValue);
console.log(result2);

objValue = null;