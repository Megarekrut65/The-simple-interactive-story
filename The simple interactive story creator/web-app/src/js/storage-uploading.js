import { getUser } from "./firebase/auth";
import { uploadFileAndGetUrl } from "./firebase/storage";
import { setStorageImages } from "./firebase/story";
import { imageToSrc } from "./utilities/image-utility";
import { soundToSrc } from "./utilities/sound-utility";


const uploadFile = (items, newItem, toSrcFunction, type, key) => {
    const user = getUser();
    if (!user || !newItem) return Promise.resolve(null);

    if (!newItem.file) return Promise.resolve(toSrcFunction(newItem));

    return uploadFileAndGetUrl(user.uid, type, newItem.file).then((res) => {
        const item = { id: newItem.id, name: newItem.name, [key]: res };
        items.push(item);

        return setStorageImages(user.uid, items).then(() => res);
    });
}

export const uploadImage = (images, image) => {
    return uploadFile(images, image, imageToSrc, "images", "img");
};

export const uploadSound = (sounds, sound) => {
    return uploadFile(sounds, sound, soundToSrc, "sounds", "sound");
};
