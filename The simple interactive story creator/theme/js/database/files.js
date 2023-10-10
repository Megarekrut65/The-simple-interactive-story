import {ref, getDownloadURL, uploadBytes, deleteObject} from "https://www.gstatic.com/firebasejs/10.4.0/firebase-storage.js";
import { storage } from "./storage.js";


export const getFileURL= async (folder, imageId)=>{
    const imgRef = ref(storage, folder + imageId);
    try{
        const img = await getDownloadURL(imgRef);
        return img;
    }catch(err){
        console.log(err);
    }
    return "";
};
/**
 * 
 * @param {File} file - file api
 */
export const addFile = (folder, file)=>{
    const storageRef = ref(storage, folder + file.name);
    return uploadBytes(storageRef, file);
}
export const removeFile = (folder, imageId)=>{
    const storageRef = ref(storage, folder + imageId);

    return deleteObject(storageRef);
};