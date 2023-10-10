export class FileInput{
    listInput
    datalist
    fileInput

    constructor(form, inputId){
        this.listInput = form.querySelector(inputId + "-saved");
        this.datalist = form.querySelector(inputId + "-list");
        this.fileInput = form.querySelector(inputId);
    }

    loadDatalist = (items)=>{
        for(let i in items){
            const option = document.createElement("option");
            option.textContent = items[i];
    
            this.datalist.appendChild(option);
        }
    }

    get value(){
        return null;
    }
};