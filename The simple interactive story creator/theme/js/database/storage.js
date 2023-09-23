import { app } from "../init.js";
import { getStorage } from "https://www.gstatic.com/firebasejs/10.4.0/firebase-storage.js";

export const storage = getStorage(app);