let user = {
    name: "Ulyana",
    age: 18,
}

let admin = {
    admin: true,
    ...user
}

console.log(admin);