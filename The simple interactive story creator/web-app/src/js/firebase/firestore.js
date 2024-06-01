import { firebaseApp } from "./firebase";
import { getFirestore } from "firebase/firestore";

export const fs = getFirestore(firebaseApp);