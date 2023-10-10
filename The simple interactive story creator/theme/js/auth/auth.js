import { getAuth, 
    createUserWithEmailAndPassword, 
    onAuthStateChanged,
    signOut, 
    updateProfile,
    sendPasswordResetEmail,
    signInWithEmailAndPassword } from "https://www.gstatic.com/firebasejs/10.4.0/firebase-auth.js";

import { app } from "../init.js";

const auth = getAuth(app);

/**
 * 
 * @param {String} email 
 * @param {String} password 
 * @returns promise with answer
 */
export const registerUserEmail = (email, password)=>{
    return createUserWithEmailAndPassword(auth, email, password);
};

export const addUserName = (user, name)=>{
    return updateProfile(user, {displayName: name});
};

/**
 * 
 * @param {String} email 
 * @param {String} password 
 * @returns promise with answer
 */
export const singInUserEmail = (email, password)=>{
    return signInWithEmailAndPassword(auth, email, password);
}


export const signOutCurrentUser = ()=>{
    return signOut(auth);
};
/**
 * 
 * @param {(user)=>{}} action - function to do something with user
 */
export const currentUser = (action)=>{
    onAuthStateChanged(auth, action);
};

export const resetPasswordEmail = (email)=>{
    return sendPasswordResetEmail(auth, email);
};