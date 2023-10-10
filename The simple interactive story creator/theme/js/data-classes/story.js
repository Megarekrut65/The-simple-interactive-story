import { textToId } from "../text-utility.js";

export const constructGeneral = (title, preview, font, author, authorId, keepPrivate)=>{
    return {
        id: textToId(title),
        title: title,
        preview: preview,
        font: font,
        author: author,
        authorId: authorId,
        keepPrivate: keepPrivate
    };
};