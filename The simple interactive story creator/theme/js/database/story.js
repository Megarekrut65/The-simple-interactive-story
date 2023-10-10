import {
    collection,
    addDoc, 
    setDoc,
    updateDoc,
    deleteDoc,
    orderBy,
    startAfter,
    endBefore,
    limit,
    query,
    doc,
    getDocs,
    getDoc} from 'https://www.gstatic.com/firebasejs/10.4.0/firebase-firestore.js';

import { db } from "./db.js";

const mainCollection = "stories";

/**
 * Gets story by id
 * 
 * @param {String} id id of story
 * @returns story data or null
 */
export const getStory = async(id)=>{
    const res = await getDoc(doc(db, mainCollection, id));

    return res.exists()?res:null;
};
/**
 * Gets all stories
 * 
 * @returns list of document snapshot
 */
export const getAllStories = async()=>{
    const q = query(collection(db, mainCollection), 
        orderBy("publishDate", "desc"));
    const res = await getDocs(q);

    return res;
};
/**
 * Gets stories
 * 
 * @returns list of document snapshot
 */
export const getStories = async(perPage, after = null)=>{
    const q = query(collection(db, mainCollection), 
        after == null?endBefore(null):startAfter(after),
        limit(perPage));

    const res = await getDocs(q);

    return res;
};
/**
 * Adds genereal configs of story to database
 * 
 * @param {Object} story 
 * @returns object from firebase
 */
export const addGeneralConfigs = async(story)=>{
    const docRef = await setDoc(doc(db, mainCollection, story.id), story);

    return docRef;
}
/**
 * Updates story in database
 * 
 * @param {String} id 
 * @param {Object} story 
 * @returns object from firebase
 */
export const updateStory = async(id, story)=>{
    const docRef = await updateDoc(doc(db, mainCollection, id), story);

    return docRef;
}
/**
 * Removes story from database
 * 
 * @param {String} id 
 * @returns object from firebase
 */
export const removeStory = (id)=>{
    return deleteDoc(doc(db, mainCollection, id));
}