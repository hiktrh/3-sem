console.log(" =======1задание======= ");
const Figure = {
    color: null,
    size: null,
    shape: null,
    lines: null,
    getProt(){
        return `Цвет: ${this.color}, размер: ${this.size}, форма: ${this.shape}, линии: ${this.lines}`;
    }
}

const Circle = Object.create(Figure);
Circle.color = "white";
Circle.size = "normal";
Circle.shape = "Circle";

const greenCircle = Object.create(Circle);
greenCircle.color = "green";    

const Kvadrat = Object.create(Figure);
Kvadrat.color = "yellow";
Kvadrat.shape = "Kvadrat";
Kvadrat.size = "normal";

const smallKvadrat = Object.create(Kvadrat);
smallKvadrat.size = "small";

const Treangle = Object.create(Figure);
Treangle.color = "white";
Treangle.lines = "1";
Treangle.size = "normal";
Treangle.shape = "Treangle";

const Treanglelines = Object.create(Treangle);
Treanglelines.lines = "3";

function getDist(child, parent){
    const did = {};
    for (const key of Object.keys(parent)){
        if (child[key] !== parent[key]){
            did[key] = child[key];
        }
    }
    return did;
}

console.log(" =======свойства======= ");
const dis1 = getDist(greenCircle, Circle);
console.log("Разница зеленого круга: ", dis1);

const dis2 = Treanglelines.getProt();
console.log("Свойства треугольника с 3 линиями: ", dis2);

if (smallKvadrat.hasOwnProperty('color')) {
    console.log("У маленького квадрата есть собственное свойство color, значение:", smallKvadrat.color);
} else {
    console.log("У маленького квадрата нет собственного свойства color, значение унаследовано:", smallKvadrat.color);
}

console.log(" =======2задание======= ");

class Human {
    constructor(firstName, lastName, birthYear, address) {
        this._firstName = firstName;
        this._lastName = lastName;
        this._birthYear = birthYear;
        this._address = address;
    }

    get age() {
        const currentYear = new Date().getFullYear();
        return currentYear - this._birthYear;
    }

    set age(newAge) {
        const currentYear = new Date().getFullYear();
        this._birthYear = currentYear - newAge;
    }

    changeAddress(newAddress) {
        this._address = newAddress;
    }

    getAddress() {
        return this._address;
    }

    get birthYear() {
        return this._birthYear;
    }
}

class Student extends Human {
    constructor(firstName, lastName, birthYear, address, faculty, course, group, recordBookNumber) {
        super(firstName, lastName, birthYear, address);
        this._faculty = faculty;
        this._course = course;
        this._group = group;
        this._recordBookNumber = recordBookNumber;
    }

    getFullName() {
        return `${this._firstName} ${this._lastName}`;
    }

    changeCourse(newCourse) {
        this._course = newCourse;
    }

    changeGroup(newGroup) {
        this._group = newGroup;
    }

    getSpecialty() {
        if (!this._recordBookNumber || this._recordBookNumber.length < 8) {
            return 'Неизвестно';
        }

        const specialtyCode = this._recordBookNumber[2];
        const specialties = {
            '1': 'ПОИТ',
            '2': 'ИСИТ',
            '3': 'ДЭВИ',
            '4': 'ПОИБМС'
        };
        return specialties[specialtyCode] || 'Неизвестно';
    }

    getFacultyCode() {
        return this._recordBookNumber ? this._recordBookNumber[0] : null;
    }

    getAdmissionYear() {
        if (!this._recordBookNumber || this._recordBookNumber.length < 8) return null;
        const yearCode = this._recordBookNumber.substring(3, 5);
        return parseInt('20' + yearCode);
    }

    getStudyType() {
        if (!this._recordBookNumber || this._recordBookNumber.length < 8) return null;
        const typeCode = this._recordBookNumber[5];
        return typeCode === '1' ? 'бюджет' : typeCode === '2' ? 'платники' : 'неизвестно';
    }

    get faculty() {
        return this._faculty;
    }

    get course() {
        return this._course;
    }

    get group() {
        return this._group;
    }

    get recordBookNumber() {
        return this._recordBookNumber;
    }
}

class Faculty {
    constructor(name) {
        this._name = name;
        this._students = [];
    }

    get name() {
        return this._name;
    }

    get groupCount() {
        const uniqueGroups = new Set();
        this._students.forEach(student => {
            uniqueGroups.add(student.group);
        });
        return uniqueGroups.size;
    }

    set groupCount(count) {
        console.log('Количество групп вычисляется автоматически на основе данных студентов');
    }

    get studentCount() {
        return this._students.length;
    }

    set studentCount(count) {
        console.log('Используйте методы addStudent или removeStudent для изменения количества студентов');
    }

    addStudent(student) {
        this._students.push(student);
    }

    removeStudent(recordBookNumber) {
        const index = this._students.findIndex(student =>
            student.recordBookNumber === recordBookNumber
        );
        if (index !== -1) {
            this._students.splice(index, 1);
            return true;
        }
        return false;
    }

    getDev() {
        return this._students.filter(student =>
            student.getSpecialty() === 'ДЭВИ'
        ).length;
    }

    getGroup(groupName) {
        return this._students
            .filter(student => student.group === groupName)
            .map(student => student.getFullName());
    }

    getAllStudents() {
        return this._students.map(student => ({
            fullName: student.getFullName(),
            age: student.age,
            course: student.course,
            group: student.group,
            specialty: student.getSpecialty(),
            recordBookNumber: student.recordBookNumber
        }));
    }
}

const fit = new Faculty("Факультет информационных технологий");

const student1 = new Student(
    "Ульяна",
    "Волчек",
    2006,
    "ул. A, д.1",
    fit.name,
    3,
    "Группа 2",
    "71301300"
);

const student2 = new Student(
    "Каролина",
    "Граховская",
    2001,
    "ул. A, д.1",
    fit.name,
    2,
    "Группа 1",
    "73201301"
);

const student3 = new Student(
    "Галилео",
    "Галилей",
    2000,
    "ул. C, д.2",
    fit.name,
    1,
    "Группа 2",
    "73201302"
);

const student4 = new Student(
    "Зина",
    "Ивановна",
    2002,
    "ул. D, д.3",
    fit.name,
    2,
    "Группа 2",
    "74201303"
);

fit.addStudent(student1);
fit.addStudent(student2);
fit.addStudent(student3);
fit.addStudent(student4);

console.log("=== Информация о студентах ===");
fit.getAllStudents().forEach((student, index) => {
    console.log(`${index + 1}. ${student.fullName}, Возраст: ${student.age}, ` +
        `Курс: ${student.course}, Группа: ${student.group}, ` +
        `Специальность: ${student.specialty}, ` +
        `Номер зачётки: ${student.recordBookNumber}`);
});

console.log("\n=== Статистика факультета ===");
console.log(`Название факультета: ${fit.name}`);
console.log(`Количество студентов: ${fit.studentCount}`);
console.log(`Количество групп: ${fit.groupCount}`);
console.log(`Количество студентов специальности ДЭВИ: ${fit.getDev()}`);

console.log("\n=== Студенты по группам ===");
console.log("Студенты группы 'Группа 1':", fit.getGroup("Группа 1"));
console.log("Студенты группы 'Группа 2':", fit.getGroup("Группа 2"));

console.log("\n=== Изменение возраста ===");
console.log(`Возраст студента1 до изменения: ${student1.age} лет`);
student1.age = 25;
console.log(`Возраст студента1 после изменения: ${student1.age} лет`);
console.log(`Год рождения студента1: ${student1.birthYear}`);

console.log("\n=== Изменение адреса ===");
console.log(`Адрес студента2 до изменения: ${student2.getAddress()}`);
student2.changeAddress("ул. Новая, д.10");
console.log(`Адрес студента2 после изменения: ${student2.getAddress()}`);

console.log("\n=== Изменение курса и группы ===");
console.log(`Студент3: ${student3.getFullName()}, Курс: ${student3.course}, Группа: ${student3.group}`);
student3.changeCourse(2);
student3.changeGroup("Группа 3");
console.log(`После изменений: Курс: ${student3.course}, Группа: ${student3.group}`);

console.log("\n=== Удаление студента ===");
console.log(`Количество студентов до удаления: ${fit.studentCount}`);
fit.removeStudent("71201300");
console.log(`Количество студентов после удаления: ${fit.studentCount}`);