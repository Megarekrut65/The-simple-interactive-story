import { slugify } from 'https://cdn.jsdelivr.net/npm/transliteration@2.1.8/dist/browser/bundle.esm.min.js';

export const textToId = (text)=>{
    const latin = slugify(text);

    return latin;
};