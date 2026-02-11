function addSet(set, value)
{
set.add(value)
console.log('Товар был успешно добавлен!');

}

function deleteValue(set,value){
    if(set.delete(value)){
        console.log('элемент удален!');
    }
}

function hasValue(set,value){
    if(set.has(value)){
        console.log('элемент был успешно найден');
    }
    else
        console.log('элемент не обнаружен');
}

let set= new Set();
addSet(set, 'pizza');
addSet(set, 'chips');
addSet(set, 'grape');
deleteValue(set, 'chips');
hasValue(set,'chips');
console.log('размер сета:'+ set.size);

