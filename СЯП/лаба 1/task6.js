let russian = prompt("Введите вашу отметку по русскому языку:");
let mathematics = prompt("Введите вашу отметку по математике:");
let english = prompt("Введите вашу отметку по английскому языку:");

if (russian > 3 && mathematics > 3 && english > 3)
    alert("Поздравляем! Вас перевели на следующий курс!");
else if (russian < 4 && mathematics < 4 && english < 4)
    alert("Поздравляю! Вы отчислены! Свободен!");
else if (russian < 4 || mathematics < 4 || english < 4)
    alert("Вам нужно пересдать экзамен");
let a = 0 || "hello"