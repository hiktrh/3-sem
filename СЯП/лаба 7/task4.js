let team = {
    players: [
        {name: "tN1R", position: "rifler", number: 2},
        {name: "donk", position: "entry fragger", number: 1},
        {name: "magixx", position: "anchor", number: 3},
    ],
    showPlayers(){
        this.players.forEach(player => {
            console.log(`Игрок: ${player.name}, Позиция: ${player.position}, Номер: ${player.number}`);
        });
        console.log(`Всего игроков: ${this.players.length}`);
    }
}

team.showPlayers();