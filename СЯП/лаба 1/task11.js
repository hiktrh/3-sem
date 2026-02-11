let a = parseFloat(prompt("Ввидете ширину фигуры:",""));
let b = parseFloat(prompt("Ввидете высоту фигуры:",""));

//Function Expression:

let params = function(a, b){
    if (a == b){
        return a * 4;
    }
    return a * b;
}

//Function Declaration:

 function params(a, b){
    if (a == b){
         return a * 4;
     }
     return a * b;
 } 

 //функция стрелка:

let params = (a,b) => {
     if (a == b){
         return a * 4;
     }
     return a * b;
 } 

alert(params(a,b));