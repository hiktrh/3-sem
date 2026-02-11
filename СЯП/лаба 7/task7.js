let circle = {
    radius: 16,

    get square() {
        return this.radius * this.radius;
    },

    get changeRadius() {
        return this.radius;
    },

    set changeRadius(value) {
        if (value > 0) {
            this.radius = value;
        } else {
            console.log("Значение не может быть отрицательным");
        }
    }
}

console.log(`Площадь круга: ${circle.square}π`);

circle.changeRadius = 10;
console.log(circle.changeRadius);