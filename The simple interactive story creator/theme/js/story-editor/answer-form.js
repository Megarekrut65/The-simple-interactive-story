import { FileInput } from "./file-input.js";
import { constructAnswer } from "../data-classes/answer.js";

const formContainer = document.getElementById("answer-form-container");
export const answerForm = document.getElementById("answer-form");

const answerTextInput = answerForm.querySelector("#answer-text");
const frameIdInput = answerForm.querySelector("#frame-id");
const clickMusicFileInput = new FileInput(answerForm, "#click-music");
const clickActionInput = answerForm.querySelector("#click-action");

const cancelButton = answerForm.querySelector("#cancel");

export const showForm = ()=>{
    formContainer.style.display = "flex";
};

export const loadAnswer = (answer)=>{

    answerTextInput.value = answer.text;
    
    frameIdInput.value = answer.next;
    clickMusicFileInput.value = answer.sound;
    clickActionInput.value = answer.action;
};

export const getAnswerFromForm = ()=>{
    return constructAnswer(answerTextInput.value, frameIdInput.value,  null, clickMusicFileInput.value,clickActionInput.value);
};

export const closeForm = ()=>{
    formContainer.style.display = "none";
};

export const loadAnswerForm = ()=>{
    cancelButton.onclick = closeForm;
};