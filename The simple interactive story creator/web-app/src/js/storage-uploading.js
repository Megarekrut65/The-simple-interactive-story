import { getUser } from "./firebase/auth";
import { uploadFileAndGetUrl } from "./firebase/storage";
import { setStorageList } from "./firebase/story";

const uploadFile = (items, newItem, type, key) => {
    const user = getUser();
    if (!user || !newItem) return Promise.resolve(null);

    if (!newItem.file) return Promise.resolve(newItem);

    for (let i in items) {
        if (items[i].name === newItem.name) {
            return Promise.resolve(items[i]);
        }
    }

    return uploadFileAndGetUrl(user.uid, type, newItem.file).then((res) => {
        const item = { id: newItem.id, name: newItem.name, [key]: res };
        items.push(item);

        return setStorageList(user.uid, items, type).then(() => item);
    });
};

export const uploadImage = (images, image) => {
    return uploadFile(images, image, "images", "img");
};

export const uploadSound = (sounds, sound) => {
    return uploadFile(sounds, sound, "sounds", "sound");
};
