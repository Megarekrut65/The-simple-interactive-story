import {constructUser} from "../data-classes/user.js";
import {registerUserEmail, addUserName} from "../auth/auth.js";
import { addUserData } from "../database/users.js";

const registerForm = document.getElementById("register-form");

const nameInput = registerForm.querySelector("#name");
const emailInput = registerForm.querySelector("#email");
const passwordInput = registerForm.querySelector("#password");
const passwordRepeatInput = registerForm.querySelector("#password-repeat");

const regError = registerForm.querySelector("#error-message");

const errorFunction = (err) => {
    console.log(err);
    console.log(err.message);
    regError.textContent = "The user with the entered email address already exists!";//STRING
};

window.addEventListener("load", ()=>{
    registerForm.addEventListener("submit", ()=>{
        const password = passwordInput.value, repeat = passwordRepeatInput.value;

        if(password !== repeat){
            regError.textContent = "Passwords don't match!";//STRING
            return false;
        }

        const name = nameInput.value, email = emailInput.value;
        
        const userData = constructUser(name, email);
        

        registerUserEmail(email, password).then((result) => {
            addUserName(result.user, name).then(()=>{
                addUserData(result.user.uid, userData).then(()=>{
                    window.location.href = "account.html";
                }).catch(errorFunction);

            }).catch(errorFunction);

        }).catch(errorFunction);    
        
        return false;
    });
});