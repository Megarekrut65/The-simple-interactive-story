export const makeAspectRatio = (itemId, calculateHeight, afterResize) => {
    const item = document.getElementById(itemId);
    if (item) {
        window.addEventListener("resize", () => {
            item.height = calculateHeight(item.width);

            afterResize();
        });
    }
};