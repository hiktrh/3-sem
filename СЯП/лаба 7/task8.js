let car = {};

Object.defineProperties(car, {
    make: {
        value: "Renault",
        writable: true,
        configurable: true,
        enumerable: true
    },
    model: {
        value: "Captur",
        writable: true,
        configurable: true,
        enumerable: true
    },
    year: {
        value: "2016",
        writable: true,
        configurable: true,
        enumerable: true
    }
});

console.log(`Начальные параметры:\n${car.make}, ${car.model}, ${car.year}`);

car.make = "Ferrari";
car.model = "laItalia";
car.year = 2018;

console.log(`Измененные параметры:\n${car.make}, ${car.model}, ${car.year}`);

Object.defineProperties(car, {
    make: {
        value: car.make,
        writable: false,
        configurable: false,
        enumerable: true
    },
    model: {
        value: car.model,
        writable: false,
        configurable: false,
        enumerable: true
    },
    year: {
        value: car.year,
        writable: false,
        configurable: false,
        enumerable: true
    }
});

car.make = "Renault";
car.model = "Captur";
car.year = 2016;

console.log(`Итоговые параметры (не изменяемые):\n${car.make}, ${car.model}, ${car.year}`);