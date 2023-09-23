
import {resetPasswordEmail, singInUserEmail} from "../auth/auth.js";

const loginForm = document.getElementById("login-form");

const emailInput = loginForm.querySelector("#email");
const passwordInput = loginForm.querySelector("#password");

const logError = loginForm.querySelector("#error-message");

const resetPassword = loginForm.querySelector("#forgot-password");

const resetAction = ()=>{
    logError.textContent = "";
    const email = emailInput.value;
    if(email.length == 0){
        logError.textContent = "Enter the email address used to create the account in the field above!";//STRING
        return;
    }
    resetPasswordEmail(email).then(()=>{
        logError.textContent = `A password reset letter has been sent to the email address '${email}'`;//STRING
    }).catch((err) => {
        console.log(err);
        console.log(err.message);
        logError.textContent = err.message;
    });
};

window.addEventListener("load", ()=>{
    loginForm.addEventListener("submit", ()=>{
        logError.textContent = "";
        const email = emailInput.value, password = passwordInput.value;

        singInUserEmail(email, password).then((result) => {
            window.location.href = "account.html";
        }).catch((err) => {
                console.log(err);
                console.log(err.message);
                logError.textContent = "Invalid email address or password!";//STRING
        });
        
        return false;
    });  

    resetPassword.onclick = resetAction;
});