import { doc, getDoc } from "firebase/firestore";
import { fs } from "./firestore";

const collectionName = "system";

export const getSystemData = (key) => {
    return getDoc(doc(fs, collectionName, key))
        .then(res => res.exists() ? res.data() : null);
};
