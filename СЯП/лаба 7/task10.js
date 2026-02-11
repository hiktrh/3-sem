let rectangle = {
    width: 40,
    height: 12,

    get square() {
        return this.width * this.height;
    },

    get changeWidth() {
        return this.width;
    },

    get changeHeight() {
        return this.height;
    },

    set changeWidth(value) {
        if (value > 0) {
            this.width = value;
        } else {
            console.log("Значение не может быть отрицательным");
        }
    },

    set changeHeight(value) {
        if (value > 0) {
            this.height = value;
        } else {
            console.log("Значение не может быть отрицательным");
        }
    },
}

console.log(`Площадь прямоугольника: ${rectangle.square}`);

rectangle.changeWidth = 10;
rectangle.changeHeight = 20;
console.log(`Новая площадь прямоугольника: ${rectangle.square}`);