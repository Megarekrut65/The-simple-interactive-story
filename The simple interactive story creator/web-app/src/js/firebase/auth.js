import {
    getAuth,
    createUserWithEmailAndPassword,
    onAuthStateChanged,
    updateProfile,
    signOut,
    signInWithEmailAndPassword,
    GoogleAuthProvider,
    signInWithPopup
} from "firebase/auth";
import { firebaseApp } from "./firebase";
import i18n from "@/i18n";
import { createUserStory } from "./story";

const auth = getAuth(firebaseApp);
const googleProvider = new GoogleAuthProvider();

const authChangeEvents = new Set();

/**
 * Subscribe event to user change state events
 * 
 * @param {function} event - function to call it when user change state
 */
export const subscribeAuthChange = (event) => {
    authChangeEvents.add(event);
    event(auth.currentUser);
};

/**
 * Unsubscribe event to user change state events
 * 
 * @param {function} event - function to call it when user change state
 */
export const unsubscribeAuthChange = (event) => {
    authChangeEvents.delete(event);
    event(auth.currentUser);
};

/**
 * Call all function that subscribed to user auth change evens
 * @param {*} user 
 */
const callEvents = (user) => {
    authChangeEvents.forEach(event => {
        event(user);
    });
};

/**
 * Creates new user in database
 * 
 * @param {String} name 
 * @param {String} email 
 * @param {String} password 
 * @returns promise to answer
 */
export const createNewUser = (name, email, password) => {
    return createUserWithEmailAndPassword(auth, email, password)
        .then((userCredential) => {
            return updateProfile(userCredential.user, {
                displayName: name
            }).then(() => {
                localStorage.setItem("userData", JSON.stringify(userCredential.user));
                console.log(userCredential.user.uid)
                return createUserStory(userCredential.user.uid).then(() => {
                    callEvents(userCredential.user);
                });
            });
        });
};

/**
 * Login user
 * 
 * @param {String} email 
 * @param {String} password 
 */
export const loginUserEmail = (email, password) => {
    return signInWithEmailAndPassword(auth, email, password).then(res => {
        localStorage.setItem("userData", JSON.stringify(res.user));
        callEvents(res.user);
        return res;
    });
};

export const loginGoogle = () => {
    return signInWithPopup(auth, googleProvider);
};

/**
 * Logout user
 * 
 * @returns promise to answer
 */
export const logout = () => {
    const cookies = document.cookie.split(";");

    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i];
        const eqPos = cookie.indexOf("=");
        const name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
    }
    localStorage.clear();
    callEvents(undefined);

    return signOut(auth);
};

export const getUser = () => {
    const userData = localStorage.getItem("userData");
    if (userData && auth.currentUser) return auth.currentUser;
    if (userData) return JSON.parse(userData);

    return undefined;
};

const onAuthChange = (action) => {
    return onAuthStateChanged(auth, (user) => {
        const userData = localStorage.getItem("userData");
        if (userData && user) {
            localStorage.setItem("userData", JSON.stringify(user));
            action(user);

        } else {
            action(undefined);
        }
    });
};
onAuthChange((user) => {
    if (user) {
        callEvents(user);

    } else {
        callEvents(undefined);
    }
});

export const ifAuthenticated = (to, from, next) => {
    const unsubscribe = onAuthChange((user) => {
        if (user) {
            next();
            unsubscribe();
            return;
        }
        const query = to.params;
        query.next = to.name;
        delete query.locale;

        next({ name: "auth", query: query, params: { locale: i18n.getLocale() } });
        unsubscribe();
    });
};

export const loadWithUser = (load) => {
    const unsubscribe = onAuthChange((user) => {
        load(user);
        unsubscribe();
    });
}