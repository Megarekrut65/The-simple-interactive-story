export const makeAspectRatio = (itemId, calculateHeight, afterResize) => {
    const item = document.getElementById(itemId);
    if (item) {
        window.addEventListener("resize", (event) => {
            event.target.height = calculateHeight(event.target.width);

            afterResize();
        });
    }
};

export const getNormSize = (size, initCanvasSize, canvas) => {
    const bWidth = canvas.getBoundingClientRect().width;
    const bHeight = canvas.getBoundingClientRect().height;
    const coeffH = initCanvasSize.height / bHeight;
    const coeffW = initCanvasSize.width / bWidth;

    const imageSize = {
        height: coeffH * size.height,
        width: coeffW * size.width,
    };

    return imageSize;
};
export const getNormImage = (image, initCanvasSize, canvas) => {
    const bWidth = canvas.getBoundingClientRect().width;
    const bHeight = canvas.getBoundingClientRect().height;
    const coeffH = initCanvasSize.height / bHeight;
    const coeffW = initCanvasSize.width / bWidth;

    const imageSize = {
        height: coeffH * image.height,
        width: coeffW * image.width,
        x: coeffH * image.x,
        y: coeffW * image.y,
    };

    return imageSize;
};