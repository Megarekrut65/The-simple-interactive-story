import {
    doc, 
    setDoc,
    updateDoc,
    getDoc} from 'https://www.gstatic.com/firebasejs/10.4.0/firebase-firestore.js';
import { db } from './db.js';

const mainCollection = "users";

/**
 * 
 * @param {String} uid id of user
 * @returns user data or null
 */
export const getUserData = async(uid)=>{
    const res = await getDoc(doc(db, mainCollection, uid));

    return res.exists()?res:null;
}
/**
 * 
 * @param {String} uid id of created user
 * @param {Map} userData addition data of user
 * @returns 
 */
export const addUserData = async(uid, userData)=>{
    const docRef = await setDoc(doc(db, mainCollection, uid), userData);

    return docRef;
};
/**
 * 
 * @param {String} uid id of user
 * @param {Map} newData only fields to update. Other fields will not be updated
 */
export const updateUserData = async(uid, newData)=>{
    const ref = await updateDoc(doc(db, mainCollection, uid), newData);

    return ref;
};