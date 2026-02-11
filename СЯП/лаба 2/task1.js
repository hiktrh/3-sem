
 let basicOperation = function(operation,value1,value2)

{
    let result;
    if(operation == "+")
        result = value1 + value2;
    else if(operation == "-")
        result = value1 - value2;
    else if(operation == "*")
        result = value1 * value2;
    else if(operation == "/")
        result = value1 / value2;
    return result;
}
console.log(basicOperation("+",5,10));
console.log(basicOperation("-",5,10));
console.log(basicOperation("*",5,10));
console.log(basicOperation("/",5,10));