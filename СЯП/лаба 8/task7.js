let user5 = {
    name: 'Masha',
    age: 22,
    studies: {
        university: 'BSTU',
        speciality: 'deisgner',
        year: 2020,
        department: {
            faculty: 'FIT',
            group: 10,
        },
        exams: [
            { maths: true, mark: 8 }, 
            { programming: true, mark: 4 },
        ]
    }
};

let user5Copy = {
    ...user5,
    studies: {
        ...user5.studies,
        department: {
            ...user5.studies.department,
        },
        exams: user5.studies.exams.map(el => ({...el})),
    }
}
console.log(user5);

user5Copy.studies.department.group = 12;
user5Copy.studies.exams[1].mark = 10;

console.log(user5Copy);