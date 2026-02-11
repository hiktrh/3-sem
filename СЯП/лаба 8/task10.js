let store = {
    state: { 
        profilePage: { 
            posts: [ 
                { id: 1, message: 'Hi', likesCount: 12 },
                { id: 2, message: 'By', likesCount: 1 },
            ],
            newPostText: 'About me',
        },
        dialogsPage: {
            dialogs: [
                { id: 1, name: 'Valera' },
                { id: 2, name: 'Andrey' },
                { id: 3, name: 'Sasha' },
                { id: 4, name: 'Viktor' }
            ],
            messages: [
                { id: 1, message: 'hi' },
                { id: 2, message: 'hi hi,' },
                { id: 3, message: 'hi hi hi' }
            ],
        },

        sidebar: [],
    },
}

let storeCopy = {
    ...store,
    state: {
        ...store.state,
        profilePage: {
            ...store.state.profilePage,
            posts: store.state.profilePage.posts.map(el => ({ ...el })),
        },

        dialogsPage: {

            ...store.state.dialogsPage,
            dialogs: store.state.dialogsPage.dialogs.map(el => ({ ...el })),

            messages: store.state.dialogsPage.messages.map(el => ({ ...el })),
        },

        sidebar: [...store.state.sidebar],
    }
}

storeCopy.state.profilePage.posts.map(num => num.message = "Hello");
storeCopy.state.dialogsPage.messages.map(num => num.message = "Hello");
console.log(store);
console.log(storeCopy);