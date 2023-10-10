import {answerForm, closeForm, getAnswerFromForm } from "./answer-form.js";
import { AnswerList } from "./answer-list.js";
import { FileInput } from "./file-input.js";

const framesForm = document.getElementById("frame-form");


const viewFrameButton = framesForm.querySelector("#view-frame");
const makeMainButton = framesForm.querySelector("#make-main");

const frameSelect = framesForm.querySelector("#frame");

const titleInput = framesForm.querySelector("#title");
const backgroundFileInput = new FileInput(framesForm, "#background");
const musicInput = new FileInput(framesForm, "#background-music");
const textArea = framesForm.querySelector("#text");


const answerList = new AnswerList(framesForm, "#answer");

const leftImageInput = new FileInput(framesForm, "#left-image");
const underImageInput = new FileInput(framesForm, "#under-image");
const overImageInput = new FileInput(framesForm, "#over-image");
const rightImageInput = new FileInput(framesForm, "#right-image");

const autoSaveCheckbox = framesForm.querySelector("#auto-save");

const submitAnswer = ()=>{
    const answer = getAnswerFromForm();
    answerList.editAnswer(answer);
    closeForm();
};

export const loadFrames = ()=>{
    answerForm.addEventListener("submit", submitAnswer);
};