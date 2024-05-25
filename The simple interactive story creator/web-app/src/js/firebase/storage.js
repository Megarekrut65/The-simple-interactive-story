import { getDownloadURL, getStorage, ref, uploadBytes } from "firebase/storage";
import { firebaseApp } from "./firebase";

const storage = getStorage(firebaseApp);
const defaultFolder = "user-data";

const getFileRef = (userId, subfolder, file) => {
    return ref(storage, `${defaultFolder}/${userId}/${subfolder}/${file.name}`);
};

export const uploadFile = (userId, subfolder, file) => {
    const fileRef = getFileRef(userId, subfolder, file);
    return uploadBytes(fileRef, file);
};


export const uploadFileAndGetUrl = (userId, subfolder, file) => {
    return uploadFile(userId, subfolder, file).then(res => {
        return getDownloadURL(res.ref);
    })
};

export const getFileUrl = (userId, subfolder, file) => {
    const fileRef = getFileRef(userId, subfolder, file);

    return getDownloadURL(fileRef);
};
