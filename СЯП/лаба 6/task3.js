let store = {
  state: { // 1 уровень
    profilePage: { // 2 уровень
      posts: [ // 3 уровень
        {id: 1, message: 'Hi', likesCount: 12}, 
        {id: 2, message: 'By', likesCount: 1},
      ],
      newPostText: 'About me',
    },

    dialogsPage: {
      dialogs: [
        {id: 1, name: 'Valera'},
        {id: 2, name: 'Andrey'},
        {id: 3, name: 'Sasha'},
        {id: 4, name: 'Viktor'},
      ],
      messages: [
        {id: 1, message: 'hi'},
        {id: 2, message: 'hi hi'},
        {id: 3, message: 'hi hi hi'},
      ],
    },
    sidebar: [],
  },
};

let {
  state: {
    profilePage: {
      posts 
    },
    dialogsPage: {
      dialogs, 
      messages 
    },
    sidebar 
  }
} = store;


console.log("LikesCount из массива posts:");
for (let post of posts) { 
  console.log(post.likesCount); 
}


let filteredDialogs = dialogs.filter(dialog => dialog.id % 2 === 0);
console.log("\nПользователи с четными id:");
console.log(filteredDialogs);


let newMessages = messages.map(message => ({ 
  ...message, 
  message: "Hello user" 
}));

console.log("\nИзмененные сообщения:");
console.log(newMessages);


let simpleMessages = messages.map(() => "Hello user");
console.log("\nТолько текст сообщений:");
console.log(simpleMessages);