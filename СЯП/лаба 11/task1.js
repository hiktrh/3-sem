class Task {
    constructor (id, name, state){
        this.id = id;
        this.name = name;
        this.state = state;
    }

    changeName (newName) {
        this.name = newName;
    }

    changeState (newState) {
        this.state = newState;
    }
}

class Todolist {
    constructor (id, name, todolist) {
        this.id = id;
        this.name = name;
        this.todolist = todolist;
    }

    changeName(newName) {
        this.name = newName;
    }

    addTask(newTask) {
        this.todolist.push(newTask);
    }

    filterTasks (state) {
        let filterTasks = this.todolist.filter((task) => task.state === state);
        return filterTasks;
    }
}

let task1 = new Task(1, "купить продукты", "не выполнено");
let task2 = new Task(2, "сделать сяп", "выполнено");
let task3 = new Task(3, "потренироваться", "не выполнено");
let task4 = new Task(4, "полить цветы", "выполнено");
let task5 = new Task(5, "помыть зеркало", "не выполнено");
let task6 = new Task(6, "послушать музыку", "выполнено");
let task7 = new Task(7, "выпить витамины", "выполнено");

let list1 = new Todolist(1, "list1", [task1, task2]);
let list2 = new Todolist(2, "list2", [task4, task5, task6]);

list2.changeName("list3");
task4.changeName("task8");
task1.changeState("выполнено")

list1.addTask(task3);
list1.addTask(task7);

console.log(list1);

filteredTasksByDone  = list1.filterTasks("не выполнено");
console.log(filteredTasksByDone);