import { FileInput } from "./file-input.js";
import { getUrlValue } from "../utilities.js";
import { currentUser } from "../auth/auth.js";
import { FONTS } from "../resources-loader/fonts.js";
import {constructGeneral} from "../data-classes/story.js";
import { addGeneralConfigs } from "../database/story.js";
import { textToId } from "../text-utility.js";

const generalForm = document.getElementById("general-form");

const identifierInput = generalForm.querySelector("#identifier");
const titleInput = generalForm.querySelector("#title");

const previewFileInput = new FileInput(generalForm, "#preview");

const fontInput = generalForm.querySelector("#font");
const fontExample = generalForm.querySelector("#font-example");
const authorInput = generalForm.querySelector("#author");
const privateCheckbox = generalForm.querySelector("#private");

const framesRow = document.getElementById("frames-row");
const loader = document.getElementById("loader");

const loadSelect = (select, list)=>{
    select.innerHTML = "";

    for(let i in list){
        const option = document.createElement("option");
        option.textContent = list[i].name;
        option.style.fontFamily = list[i].name;

        select.appendChild(option);

        if(list[i].path === undefined) continue;

        const fontFile = new FontFace(
            list[i].name,
            `url(${list[i].path})`,
        );
        document.fonts.add(fontFile);
    }
};

const onFontChange = ()=>{
    const selected = fontInput.selectedOptions[0];
    if(selected){
        fontExample.style.fontFamily = selected.textContent;
    } 
};

const checkUser = async(user)=>{
    if(user == null){
        window.location.href = "login-register.html";
        return;
    }
    console.log(user);

    const storyId = getUrlValue("story");
    if(storyId){
        console.log(storyId);
    }

    authorInput.value = user.displayName;
    authorInput.setAttribute("meta-data", user.uid);

    loadSelect(fontInput, FONTS);
};

const onGeneralSubmit = ()=>{
    loader.style.display = "flex";

    const generalStory = constructGeneral(titleInput.value, null, fontInput.value, authorInput.value, 
        authorInput.getAttribute("meta-data"), privateCheckbox.checked);

    addGeneralConfigs(generalStory).then(_=>{
        loader.style.display = "none";
        framesRow.style.display = "flex";
    }).catch(err=>{
        loader.style.display = "none";
    });

    return false;
};

const onTitleChange = ()=>{
    identifierInput.value = textToId(titleInput.value);
};

export const loadGeneral = ()=>{

    currentUser(checkUser);

    fontInput.onchange = onFontChange;

    generalForm.addEventListener("submit", onGeneralSubmit);

    titleInput.addEventListener("change", onTitleChange);
};