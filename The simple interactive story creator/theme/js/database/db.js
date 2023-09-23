import {getFirestore} from 'https://www.gstatic.com/firebasejs/10.4.0/firebase-firestore.js';
import { app } from '../init.js';

export const db = getFirestore(app);