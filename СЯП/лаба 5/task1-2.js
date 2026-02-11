let currentCount = 1;
function makeCounter() {
return function() {
return currentCount++;
};
}

let counter = makeCounter();
let counter2 = makeCounter();

alert( counter() );
alert( counter() );
alert( counter2() );
alert( counter2() );