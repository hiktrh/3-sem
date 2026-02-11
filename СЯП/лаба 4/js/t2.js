function addStudent(set, value){
set.add(value)
console.log('Студент успешно добавлен!');

}

function deleteValue(set, value){
    for(let student of set){
        if(student.numOfZachetka==value){
            set.delete(student);
            console.log('Удален успешно!');
        }   
    }
}

function filterGroup(set, value){
    let resultSet= new Set();
    for(let student of set){    
        if(student.group==value){
           addStudent(resultSet, student);
        }
    }
}

function sortZachetka(set){
     let arr=Array.from(set);
    set.clear();
    arr=arr.sort((el1, el2) => el1.numOfZachetka - el2.numOfZachetka);
    for(const elem of arr){
        set.add(elem);
    }
}

let students= new Set();
let st1={
    numOfZachetka: 23,
    group:10,
    name:'Мао Мао Пупсовна'
}
let st2={
    numOfZachetka: 22,
    group:10,
    name:'Аркадий Паровозов Потешкин'
}
let st3={
    numOfZachetka: 13,
    group:8,
    name:'Викторианская Битва Викторовна'
}
let st4={
    numOfZachetka: 1833,
    group:8,
    name:'Фишер Салли Гизмович'
}
addStudent(students, st1);
addStudent(students, st2);
addStudent(students, st3);
console.log(students);
deleteValue(students,13);
filterGroup(students, 10);
console.log(students);
sortZachetka(students);
console.log(students);