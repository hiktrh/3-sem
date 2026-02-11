function Book(title, author){
    this.title = title;
    this.author = author;

    this.getTitle = function(){
        alert("Название книги: " + this.title);
    }

    this.getAuthor = function(){
        alert("Автор: " + this.author);
    }
}

let book1 = new Book("Мастер и Маргарита", "М.Булгаков");

book1.getTitle();
book1.getAuthor();