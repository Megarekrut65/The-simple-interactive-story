export const getUrlValue = (key, href=undefined)=>{
    if(href === undefined) href = window.location;

    const url = new URL(href);
    return url.searchParams.get(key);
};