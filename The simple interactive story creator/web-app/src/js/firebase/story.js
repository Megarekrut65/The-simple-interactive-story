import { collection, deleteDoc, doc, getDoc, getDocs, limit, orderBy, query, setDoc, startAfter } from "firebase/firestore";
import { fs as db } from "./firestore";

const mainCollection = "userStories", storyCollection = "stories", scenesCollection = "scenes";


const getStoryCollection = (userId) => {
    return `${mainCollection}/${userId}/${storyCollection}`;
};

const getSceneCollection = (userId, storyId) => {
    return `${getStoryCollection(userId)}/${storyId}/${scenesCollection}`;
};

export const setScene = (userId, storyId, scene) => {
    const coll = getSceneCollection(userId, storyId);
    return setDoc(doc(db, coll, scene.id), scene);
};

export const storyExists = (userId, storyId) => {
    const coll = getStoryCollection(userId);

    return getDoc(doc(db, coll, storyId)).then(res => {
        return res.exists();
    });
};

export const setStory = (userId, story) => {
    const coll = getStoryCollection(userId);

    return setDoc(doc(db, coll, story.id), story);
};

const dataOrNull = (res) => {
    if (res.exists()) return res.data();

    return null;
};
const dataDocs = (res) => {
    return res.docs.map(item => item.data());
};

export const getStory = (userId, storyId) => {
    const coll = getStoryCollection(userId);

    return getDoc(doc(db, coll, storyId)).then(dataOrNull);
};

export const getUserStories = (userId, perPage, after = null) => {
    const q = query(collection(db, getStoryCollection(userId)),
        orderBy("title"),
        orderBy("creatingDate", "desc"),
        startAfter(after),
        limit(perPage));

    return getDocs(q).then(dataDocs);
};

export const getStoryScenes = (userId, storyId) => {
    return getDocs(collection(db, getSceneCollection(userId, storyId))).then(dataDocs);
};

export const createUserStory = (userId) => {
    return setDoc(doc(db, mainCollection, userId), { images: [], sounds: [] });
};

export const getUserStorage = (userId) => {
    return getDoc(doc(db, mainCollection, userId)).then(dataOrNull);
};

export const setStorageImages = (userId, images) => {
    return setDoc(doc(db, mainCollection, userId), { images: images }, { merge: true });
};
export const setStorageSounds = (userId, sounds) => {
    return setDoc(doc(db, mainCollection, userId), { sounds: sounds }, { merge: true });
};

export const removeStory = (userId, storyId) => {
    const coll = getStoryCollection(userId);
    return deleteDoc(doc(db, coll, storyId));
};