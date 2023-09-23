import {ref, getDownloadURL, uploadBytes, deleteObject} from "https://www.gstatic.com/firebasejs/9.15.0/firebase-storage.js";
import { storage } from "./storage.js";


const directory = "materials/";

export const getImageURL= async (imageId)=>{
    const imgRef = ref(storage, directory + imageId);
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
export const addImage = (file)=>{
    const storageRef = ref(storage, directory + file.name);
    return uploadBytes(storageRef, file);
}
export const removeImage = (imageId)=>{
    const storageRef = ref(storage, directory + imageId);

    return deleteObject(storageRef);
};