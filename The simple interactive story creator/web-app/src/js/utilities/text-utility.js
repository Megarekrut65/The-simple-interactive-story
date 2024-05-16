import { slugify } from 'https://cdn.jsdelivr.net/npm/transliteration@2.1.8/dist/browser/bundle.esm.min.js';

export const textToId = (text) => {
    const latin = slugify(text);

    return latin;
};

export const getUniqueName = (container, name) => {
    if (!container.includes(name)) return name;

    const last = name.slice(-1);
    let number = parseInt(last);
    if (!isNaN(last) && !isNaN(number)) {
        name = name.slice(0, -1);
    }
    else {
        number = 1;
    }

    while (container.includes(`${name}${number}`)) {
        number += 1;
    }

    return `${name}${number}`;
};