let person = {
    name: "Ulyana",
    age: 18,

    greet(){
        alert(`Привет, ${this.name}`);
    },

    ageAfterYears(years){
        const futureAge = this.age + years;
        alert(`Через ${years} лет вам будет ${futureAge}`);
    }
}

let years = 5;

person.greet();
person.ageAfterYears(years);