import { loadAnswer, showForm  } from "./answer-form.js";
import { constructAnswer} from "../data-classes/answer.js";


export class AnswerList{
    answers = []

    listContainer
    addButton

    currentIndex = -1

    constructor(form, id){
        this.listContainer = form.querySelector(id + "-list");
        this.addButton = form.querySelector(id + "-add");

        this.addButton.addEventListener("click", this.addClicked);
    }

    addClicked = ()=>{
        console.log("click");
        this.createAnswer();
    }

    createAnswer = ()=>{
        const html = `
        <div class="close-button" title="Remove answer"><i class="gg-close"></i></div>
        <div class="edit-button" title="Edit answer"><i class="gg-pen"></i></div>
        
        <input id="answer-text" type="text" value="Answer text..." readonly>
        <input id="answer-next" type="text" value="Next frame..." readonly>
        `;

        const obj = document.createElement("div");
        obj.className = "part-container answer-container";
        obj.innerHTML = html;

        const index = this.answers.length;
        obj.setAttribute("meta-data", index);

        const closeButton = obj.querySelector(".close-button");
        closeButton.onclick = ()=>{
            this.answers.splice(obj.getAttribute("meta-data"), 1); 
            obj.remove();
        };

        const editButton = obj.querySelector(".edit-button");
        editButton.onclick = ()=>{
            showForm();
            loadAnswer(this.answers[index].data);
            this.currentIndex = index;
        };

        this.answers.push({data:constructAnswer("Answer text...", "Next frame..."), obj:obj});
        this.listContainer.appendChild(obj);
    }

    editAnswer = (answer)=>{
        this.answers[this.currentIndex].data = answer;

        const obj = this.answers[this.currentIndex].obj;

        const text = obj.querySelector("#answer-text");
        text.value = answer.text;

        const next = obj.querySelector("#answer-next");
        next.value = answer.next;
    }
};