const counter = (function () {
    return {
        count: 0,

        increment: function () {
            this.count++;
            return this.count;
        },

        decrement: function () {
            this.count--;
            return this.count;
        },

        getCount: function () {
            return this.count;
        }
    };
})();

console.log(counter.increment());
console.log(counter.increment());
console.log(counter.decrement());
console.log(counter.getCount());