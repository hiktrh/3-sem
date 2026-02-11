function extendObject(...args) { 

    let extendedList = Object.assign({}, ...args); 
    return extendedList;
}

let obj1 = { a: 1 };
let obj2 = { b: 2 };
let obj3 = { c: 3 };
let obj4 = { d: 4 };


console.log(extendObject(obj1, obj2, obj3, obj4));