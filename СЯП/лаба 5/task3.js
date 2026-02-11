function* moveObject() {
    let x = 0;
    let y = 0;
    console.log(`Начальные координаты объекта: x:${x}, y:${y}`);

    while(true) {
        let direction = yield {x, y};
        switch (direction) {
            case "left":
                x = x - 10;
                break;
                case "right":
                    x = x + 10;
                    break;
                    case "up":
                y = y + 10;
                break;
                case "down":
                y = y - 10;
                break;
                default:
                    console.log("Неизвестная команда: " + direction);
        }
        console.log(`Новые координаты объекта: x:${x}, y:${y}`);
    }
}
const objectMover = moveObject();
objectMover.next();
for(let i = 0; i < 10; i++) {
    let answer = prompt("Введите команду (left/right/up/down): ");
    objectMover.next(answer);
}