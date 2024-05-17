import { v4 as uuidv4 } from "uuid";



export const loadImage = (event, success, reject) => {
    const files = event.target.files;

    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const reader = new FileReader();

        reader.onload = function () {
            const img = new Image();
            img.onload = function () {
                img.id = uuidv4();
                success(img);
            }
            img.src = reader.result;
        };
        reader.onerror = reader.onabort = reject;

        reader.readAsDataURL(file);
    }
};

export const imageToSrc = (image) => {
    if (image === undefined) return "";
    if (image instanceof Image) return image.src;
    if (typeof image === "string") return image;
    if (typeof image === "object" && "img" in image)
        return imageToSrc(image.img);

    return "";
};
