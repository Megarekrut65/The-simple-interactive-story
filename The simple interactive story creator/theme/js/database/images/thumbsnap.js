const apiKey = "000033fefc8dac03fe770d01073f1c68";


export const uploadImage = async(imageFile, success, failure)=>{
    const xhr = new XMLHttpRequest(), formData = new FormData();

    xhr.withCredentials = false;
    xhr.open('POST', 'https://thumbsnap.com/api/upload');

    xhr.onload = () =>{
        let json;

        if (xhr.status < 200 || xhr.status >= 300) {
            failure('HTTP Error: ' + xhr.status);
            return;
        }

        json = JSON.parse(xhr.responseText);

        if (!json || !json.data ||typeof json.data.media != 'string') {
            failure('Invalid JSON: ' + xhr.responseText);
            return;
        }

        success(json.data.media);
    };

    xhr.onerror = ()=> {
        failure('Image upload failed due to a XHR Transport error. Code: ' + xhr.status);
    };

    formData.append('key', apiKey);
    formData.append('media', imageFile, imageFile.name);

    xhr.send(formData);
};