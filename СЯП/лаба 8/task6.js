let user4 = {
    name: 'Masha',
    age: 19,
    studies: {
        university: 'BSTU',
        speciality: 'deisgner',
        year: 2020,
        exams: {
            maths: true,
            programming: false
        }
    }
};

let user4Copy = {...user4,
    studies: {
        ...user4.studies,
        exams: {
            ...user4.studies.exams,
        }
    }
};

console.log(user4);
console.log(user4Copy);