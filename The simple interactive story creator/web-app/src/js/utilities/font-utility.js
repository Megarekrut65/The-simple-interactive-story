export const loadFont = async (name, url) => {
    const myFont = new FontFace(name, `url(${url})`);
    return myFont.load().then(() => {
        document.fonts.add(myFont);
    });
}