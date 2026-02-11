let user = {
    firstName: 'Thomas',
    lastName: 'Anderson',

    get fullName() {
        return `${this.firstName} ${this.lastName}`;
    },

    set fullName(value) {
        [this.firstName, this.lastName] = value.split(' ');
    }
};

console.log(user.fullName);

user.fullName = 'John Smith';

console.log(user.fullName);