function makeCounter() {
    let currentCount = 1; 
     return function() {
        return currentCount++;
     };
}

let counter = makeCounter();

alert( counter() );
alert( counter() );
alert( counter() );

let counter2 = makeCounter();
alert( counter2() );