import { collection, deleteDoc, doc, getDoc, getDocs, limit, orderBy, query, setDoc, startAfter } from "firebase/firestore";
import { fs as db } from "./firestore";

const mainCollection = "userStories", storyCollection = "stories", scenesCollection = "scenes";
const publishCollection = "publishStories";

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

export const publishStory = (story) => {
    return setDoc(doc(db, publishCollection, story.id), story).then(() => {
        const coll = getStoryCollection(story.authorId);

        return setDoc(doc(db, coll, story.storyId), { publish: story }, { merge: true });
    });
};

export const storyExists = (storyId) => {
    return getDoc(doc(db, publishCollection, storyId)).then(res => {
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

export const setStorageList = (userId, list, listKey) => {
    return setDoc(doc(db, mainCollection, userId), { [listKey]: list }, { merge: true });
};

export const cascadeRemoveStory = (userId, storyId) => {
    const storyColl = getStoryCollection(userId);
    const sceneColl = getSceneCollection(userId, storyId);
    const docs = getDocs(collection(db, sceneColl));

    return docs.then(res => {
        const promises = [];
        for (let i in res.docs) {
            const id = res.docs[i].id;
            promises.push(deleteDoc(doc(db, sceneColl, id)));
        }

        return Promise.all(promises).then(() => {
            return deleteDoc(doc(db, storyColl, storyId));
        });
    });
};