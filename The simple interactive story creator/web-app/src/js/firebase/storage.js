import { getDownloadURL, getStorage, ref, uploadBytes } from "firebase/storage";
import { firebaseApp } from "./firebase";

const storage = getStorage(firebaseApp);

export const uploadFile = (userId, subfolder, file) => {
    const fileRef = ref(storage, `${userId}/${subfolder}/${file.name}`);

    return uploadBytes(fileRef, file);
};


export const uploadFileAndGetUrl = (userId, subfolder, file) => {
    return uploadFile(userId, subfolder, file).then(res => {
        return getDownloadURL(res.ref);
    })
};