import { loadGeneral } from "./general.js";
import { loadFrames } from "./frames.js";
import { loadAnswerForm } from "./answer-form.js";

window.addEventListener("load", ()=>{
    loadGeneral();
    loadFrames();
    loadAnswerForm();
})