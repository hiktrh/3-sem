class Task {
    constructor(id, name, state) {
        this.id = id;
        this.name = name;
        this.state = state;
    }

    changeName(newName) { 
        this.name = newName;
    }

    changeState(newState) {
        this.state = newState;
    }
}

class Todolist {
    constructor(id, name) {
        this.id = id;
        this.name = name;
        this.tasks = [];
        this.taskId = 1;
    }

    changeName(newName) {
        this.name = newName;
    }

    addTask(taskName, state = 'не выполнено') {
        let newTask = new Task(this.taskId++, taskName, state);
        this.tasks.push(newTask);
    }   

    filterTasks(state = 'all') {
        if (state === 'completed') {
            return this.tasks.filter(task => task.state === 'выполнено');
        } else if (state === 'not_completed') {
            return this.tasks.filter(task => task.state === 'не выполнено');
        }
        return this.tasks;
    }
}

let routineList = new Todolist(1, "Список дел aesthetic cleangirl");
routineList.addTask("Встать в 6 утра", "выполнено");
routineList.addTask("Обмазаться льдом", "выполнено");
routineList.addTask("выпить 5л бутылку с лимоном", "не выполнено");
routineList.addTask("утреняя пробежка 15 км минимум", "не выполнено");
routineList.addTask("15-ступенчатыый уход за кожей лица", "выполнено");
routineList.addTask("приготовить pinterest-завтрак", "выполнено");
routineList.addTask("grwm-макияж cleangirl", "выполнено");

let tasks = routineList.tasks;

function renderTasks(filter = 'all') {
    let taskList = document.getElementById('taskList');
    taskList.innerHTML = '';
    let filteredTasks = routineList.filterTasks(
        filter === 'completed' ? 'completed' :
        filter === 'not_completed' ? 'not_completed' : 'all'
    );

    filteredTasks.forEach(task => {
        let li = document.createElement('li');
        li.className = 'taskItem';
        li.innerHTML = `
            <input type="checkbox" onchange="toggleTaskState(${task.id}, this.checked)" ${task.state === 'выполнено' ? 'checked' : ''}>
            <span><b>${task.id}.</b>${task.name}</span>
            <button class="editButton" onclick="editTask(${task.id})">Редактировать</button>
            <button class="deleteButton" onclick="deleteTask(${task.id})">Удалить</button>
        `;
        taskList.appendChild(li);
    });
}
function addTask() {
    let taskNameInput = document.getElementById('taskName');
    let taskName = taskNameInput.value;
    if (taskName) {
        routineList.addTask(taskName, 'не выполнено'); 
        tasks = routineList.tasks; 
        taskNameInput.value = '';
        renderTasks();
    }
}

function toggleTaskState(taskId, checked) {
    let task = tasks.find(t => t.id === taskId);
    if (task) {
        task.changeState(checked ? 'выполнено' : 'не выполнено');
        renderTasks();
    }
}

function deleteTask(taskId) {
    routineList.tasks = routineList.tasks.filter(task => task.id !== taskId);
    tasks = routineList.tasks;
    renderTasks();
}

function editTask(taskId) {
    let task = tasks.find(t => t.id === taskId);
    let newName = prompt("Измените название задачи:", task.name);
    if (newName) {
        task.changeName(newName);
        renderTasks();
    }
}

function filterTasks(state) {
    renderTasks(state);
}

renderTasks();