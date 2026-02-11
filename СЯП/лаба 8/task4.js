let user2 = {
    name: 'Masha',
    age: 28,
    skills: ["HTML", "CSS", "JavaScript", "React"]
};

let user2Copy = {
    ...user2,
    skills: [...user2.skills],
};

console.log(user2);
console.log(user2Copy);