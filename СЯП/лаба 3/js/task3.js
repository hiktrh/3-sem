function filterStudentsGroup(students) {
    let resultObject = {};
  
    students.forEach((student) => {
      let { name, age, groupId } = student; 
      if (age > 17) {
        if (resultObject[groupId]) {
          resultObject[groupId].push(student);
        } else {
          resultObject[groupId] = [student];
        }
      }
    });
    return resultObject;
}

let students = [
    { name: 'Ulyana', age: 19, groupId: 1 },
    { name: 'Alina', age: 17, groupId: 1 },
    { name: 'Pavel', age: 16, groupId: 1 },
    { name: 'Timoha', age: 15, groupId: 2 },
    { name: 'Danik', age: 21, groupId: 1 },
];
console.log(student[4].name);

let filteredStudents = filterStudentsGroup(students);
console.log(filteredStudents);