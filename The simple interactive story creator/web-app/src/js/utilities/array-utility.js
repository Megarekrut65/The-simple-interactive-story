export const removeAt = (array, index) => {
    if (index >= 0 && index < array.length) {
        array.splice(index, 1);
    }
};