import { initializeApp } from "firebase/app";

const firebaseConfig = {
    apiKey: "AIzaSyCCZrF3kfm7mUDRnsYaVYs2E3lrxVbF-s0",
    authDomain: "the-simple-interactive-story.firebaseapp.com",
    projectId: "the-simple-interactive-story",
    storageBucket: "the-simple-interactive-story.appspot.com",
    messagingSenderId: "921279479317",
    appId: "1:921279479317:web:8bed5c7fa3c9b604a8981c"
};


export const firebaseApp = initializeApp(firebaseConfig);