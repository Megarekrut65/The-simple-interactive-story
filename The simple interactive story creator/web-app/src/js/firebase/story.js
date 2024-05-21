import { collection, doc, getDoc, setDoc } from "firebase/firestore";
import { fs as db } from "./firestore";

const mainCollection = "userStories", storyCollection = "stories", scenesCollection = "scenes";


const getStoryCollection = (userId) => {
    return collection(db, `${mainCollection}/${userId}/${storyCollection}`);
};

const getSceneCollection = (userId, storyId) => {
    return collection(db, `${getStoryCollection(userId)}/${storyId}/${scenesCollection}`);
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