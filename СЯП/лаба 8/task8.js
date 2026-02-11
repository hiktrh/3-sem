let user6 = {
    name: 'Masha',
    age: 21,
    studies: {
        university: 'BSTU',
        speciality: 'deisgner',
        year: 2020,
        department: {
            faculty: 'FIT',
            group: 10,
        },
        exams: [
            {
                maths: true,
                mark: 8,
                professor: {
                    name: 'Ivan Ivanov',
                    degree: 'PhD'
                }
            },
            {
                programming: true,
                mark: 10,
                professor: {
                    name: 'Petr Petrov',
                    degree: 'PhD'
                }
            },
        ]
    }
};

let user6Copy = {
    ...user6,
    studies: {
        ...user6.studies,
        department: {
            ...user6.studies.department,
        },
        exams: user6.studies.exams.map(el => (
            {
                ...el,
                professor: {
                    ...el.professor,
                }
            }
        ))
    }
}

user6Copy.studies.exams[0].professor.name = 'Марина Фёдоровна';
user6Copy.studies.exams[1].professor.name = 'Аделина Сергеевна';
console.log(user6);
console.log(user6Copy);