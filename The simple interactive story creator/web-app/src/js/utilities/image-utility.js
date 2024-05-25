import { v4 as uuidv4 } from "uuid";

export const objToImage = (obj, success) => {
    const img = new Image();
    img.onload = () => {
        img.id = obj.id;
        img.name = obj.name;
        success(obj, img);
    };
    img.src = obj.img;
};

const loadImageUrl = (file, url, name, success) => {
    const img = new Image();
    img.onload = () => {
        img.id = uuidv4();
        img.name = name;
        success(img, file);
    };
    img.src = url;
};

export const loadImage = (event, success, reject) => {
    const files = event.target.files;

    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const reader = new FileReader();

        reader.onload = () => loadImageUrl(file, reader.result, file.name, success);
        reader.onerror = reader.onabort = reject;

        reader.readAsDataURL(file);
    }
};

export const imageToSrc = (image) => {
    if (!image) return "";
    if (image instanceof Image) return image.src;
    if (typeof image === "string") return image;
    if (typeof image === "object" && "img" in image)
        return imageToSrc(image.img);

    return "";
};
