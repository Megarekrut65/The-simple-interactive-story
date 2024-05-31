import {
    collection, deleteDoc, deleteField, doc, getDoc, getDocs,
    limit, orderBy, query, setDoc, startAfter, where
} from "firebase/firestore";
import { fs as db } from "./firestore";

const mainCollection = "userStories", storyCollection = "stories", scenesCollection = "scenes";
const publishCollection = "publishStories";

const getStoryCollection = (userId) => {
    return `${mainCollection}/${userId}/${storyCollection}`;
};

const getSceneCollection = (userId, storyId) => {
    return `${getStoryCollection(userId)}/${storyId}/${scenesCollection}`;
};
const dataOrNull = (res) => {
    if (res.exists()) return res.data();

    return null;
};
const dataDocs = (res) => {
    return res.docs.map(item => item.data());
};
export const setScene = (userId, storyId, scene) => {
    const coll = getSceneCollection(userId, storyId);
    return setDoc(doc(db, coll, scene.id), scene);
};

export const publishStory = (story) => {
    return setDoc(doc(db, publishCollection, story.id), story).then(() => {
        const coll = getStoryCollection(story.authorId);

        return setDoc(doc(db, coll, story.storyId), { publish: story.id }, { merge: true });
    });
};
export const unpublishStory = (authorId, storyId, publishId) => {
    return deleteDoc(doc(db, publishCollection, publishId)).then(() => {
        const coll = getStoryCollection(authorId);

        return setDoc(doc(db, coll, storyId), { publish: deleteField() }, { merge: true });
    });
};

export const getPublish = (publishId) => {
    return getDoc(doc(db, publishCollection, publishId)).then(dataOrNull);
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

export const getPublishStories = (perPage, after = null) => {
    const q = query(collection(db, publishCollection),
        where("privateStory", "==", false),
        orderBy("id"),
        orderBy("publishDate", "desc"),
        startAfter(after),
        limit(perPage));

    return getDocs(q).then(dataDocs);
};

export const searchPublishStories = (title, author, authorId, genre, perPage, after = null) => {
    const conditionals = [];
    if (authorId) {
        conditionals.push(where("authorId", "==", authorId));
    }

    const q = query(collection(db, publishCollection), ...conditionals, where("privateStory", "==", false),
        orderBy("id"),
        orderBy("publishDate", "desc"),
        startAfter(after),
        limit(perPage));

    return getDocs(q).then(dataDocs).then(lst => {
        const promises = lst.map(publish => getStory(publish.authorId, publish.storyId));
        return Promise.all(promises.map(res => res.catch(() => null))).then(stories => {
            return stories.filter(story => story
                && ((!title || title.length < 2) || story.title.toLowerCase().includes(title.toLowerCase()))
                && ((!author || author.length < 2) || story.author.toLowerCase().includes(author.toLowerCase()))
                && ((!genre || genre.length < 2) || story.genre.toLowerCase().includes(genre.toLowerCase())));
        });
    });
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

export const cascadeRemoveStory = (userId, storyId, publishId) => {
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
            if (publishId) return deleteDoc(doc(db, publishCollection, publishId));

            return Promise.resolve();
        }).then(() => {
            return deleteDoc(doc(db, storyColl, storyId));
        });
    });
};