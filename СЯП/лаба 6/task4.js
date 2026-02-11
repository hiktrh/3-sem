let tasks = [
    {id: 1, title: "HTML&CSS", isDone: true},
    {id: 2, title: "JS", isDone: true},
    {id: 3, title: "ReactJS", isDone: false},
    {id: 4, title: "Rest API", isDone: false},
    {id: 5, title: "GraphQL", isDone: false},
]

let newTask = {id: 6, title: "JS2.0", isDone: false};

tasks = [...tasks, newTask];
console.log(tasks);