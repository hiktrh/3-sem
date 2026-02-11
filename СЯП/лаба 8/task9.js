let user7 = {
    name: 'Masha',
    age: 20,
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
                    degree: 'PhD',
                    articles: [
                        { title: "About HTML", pageNumber: 3 },
                        { title: "About CSS", pageNumber: 5 },
                        { title: "About JavaScript", pageNumber: 1 },
                    ]
                }
            },
            {
                programming: true,
                mark: 10,
                professor: {
                    name: 'Petr Petrov',
                    degree: 'PhD',
                    articles: [
                        { title: "About HTML", pageNumber: 3 },
                        { title: "About CSS", pageNumber: 5 },
                        { title: "About JavaScript", pageNumber: 1 },
                    ]
                }
            },
        ]
    }
};

let user7Copy = {
    ...user7,
    studies: {
        ...user7.studies,
        department: {
            ...user7.studies.department,
        },
        exams: user7.studies.exams.map(el => (
            {
                ...el,
                professor: {
                    ...el.professor,
                    articles: el.professor.articles.map(obj  => (
                        {
                            ...obj,
                        }
                    ))
                }
            }
        ))
    }
}

user7Copy.studies.exams[1].professor.articles[1].pageNumber = 3;
console.log(user7);
console.log(user7Copy);