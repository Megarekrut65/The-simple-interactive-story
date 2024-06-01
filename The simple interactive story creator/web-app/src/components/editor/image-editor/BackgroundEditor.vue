<script setup>
import { computed, onMounted } from "vue";
import { collidePoint, drawResizingCircles, getCornerUnderMouse } from "@/js/utilities/canvas-utility";
import { getNormSize } from "@/js/utilities/size-utility";
import { imageToSrc } from "@/js/utilities/image-utility";

const props = defineProps({
    images: {
        type: Array,
        required: true
    },
    draws: {
        type: Object,
        required: true
    },
    updateImage: {
        type: Function,
        required: true
    },
    currentScene: {
        type: Object,
        required: true
    }
});


const imagesContainer = computed(() => props.images);

let canvas;
let ctx;
let isDragging = false;
let isResizing = false;
let prevX, prevY;
let selectedImageIndex = -1;
let selectedCornerIndex = -1;

const size = { width: 1920, height: 1080 };

const background = computed(() => {
    if (!props.currentScene.background) return null;
    if (props.currentScene.background.img instanceof Image) return props.currentScene.background.img;

    const src = imageToSrc(props.currentScene.background);
    const image = new Image();
    image.src = src;
    return image;
});

const redrawCanvas = () => {
    // eslint-disable-next-line no-self-assign
    canvas.width = canvas.width; // clear canvas

    if (background.value) ctx.drawImage(background.value, 0, 0, size.width, size.height);

    imagesContainer.value.forEach((image, index) => {
        if (!image) return;
        const draw = props.draws[image.img.name];
        if (!draw) return;

        ctx.drawImage(draw, image.rect.x, image.rect.y, image.rect.width, image.rect.height);
        drawResizingCircles(ctx, selectedImageIndex, index, image.rect);
    });
};

const handleMouseUp = () => {
    isDragging = false;
    isResizing = false;
};

const getMousePos = (event) => {
    const normalSize = getNormSize({ height: event.offsetY, width: event.offsetX }, size, canvas)

    return { mouseX: normalSize.width, mouseY: normalSize.height };
}

const handleMouseDown = (event) => {
    const mousePos = getMousePos(event);
    const { mouseX, mouseY } = mousePos;

    for (let i = imagesContainer.value.length - 1; i >= 0; i--) {
        const image = imagesContainer.value[i];
        const corner = getCornerUnderMouse(image.rect, mouseX, mouseY);
        if (corner !== -1) {
            selectedImageIndex = i;
            selectedCornerIndex = corner;
            isResizing = true;
            prevX = mouseX;
            prevY = mouseY;

            return;
        }

        if (collidePoint(mousePos, image.rect)) {
            selectedImageIndex = i;
            isDragging = true;
            prevX = mouseX;
            prevY = mouseY;

            return;
        }
    }

    selectedImageIndex = -1;
    selectedCornerIndex = -1;
    redrawCanvas();
};
const dragImage = (event) => {
    const { mouseX, mouseY } = getMousePos(event);
    const dx = mouseX - prevX;
    const dy = mouseY - prevY;

    const selectedImage = imagesContainer.value[selectedImageIndex].rect;
    selectedImage.x += dx;
    selectedImage.y += dy;
};

const resizeImage = (event) => {
    const { mouseX, mouseY } = getMousePos(event);
    const dx = mouseX - prevX;
    const dy = mouseY - prevY;

    const selectedImage = imagesContainer.value[selectedImageIndex].rect;

    const aspect = selectedImage.width / selectedImage.height;

    switch (selectedCornerIndex) {
        case 0: // Top-left corner
            selectedImage.x += dx;
            selectedImage.y += dy;
            selectedImage.width -= dx;
            selectedImage.height -= dy;
            break;
        case 1: // Top-right corner
            selectedImage.y += dy;
            selectedImage.width += dx;
            selectedImage.height -= dy;
            break;
        case 2: // Bottom-right corner
            selectedImage.width += dx;
            selectedImage.height += dy;
            break;
        case 3: // Bottom-left corner
            selectedImage.x += dx;
            selectedImage.width -= dx;
            selectedImage.height += dy;
            break;
    }

    if (!event.shiftKey) {
        selectedImage.width = selectedImage.height * aspect;
    }
};

const handleMouseMove = (event) => {
    if (!isDragging && !isResizing) return;


    if (isDragging && selectedImageIndex !== -1) dragImage(event);
    else if (isResizing && selectedImageIndex !== -1 && selectedCornerIndex !== -1) resizeImage(event);

    redrawCanvas();
    if (selectedImageIndex != -1) {
        props.updateImage(selectedImageIndex, imagesContainer.value[selectedImageIndex]);
    }

    const { mouseX, mouseY } = getMousePos(event);
    prevX = mouseX;
    prevY = mouseY;
};



const setUpCanvas = (canvasId) => {
    canvas = document.getElementById(canvasId);
    if (canvas) {
        ctx = canvas.getContext("2d");
        canvas.addEventListener("mousedown", handleMouseDown);
        canvas.addEventListener("mousemove", handleMouseMove);

        canvas.addEventListener("mouseup", handleMouseUp);
        window.addEventListener("resize", redrawCanvas);
    }
};

const redrawAll = () => {
    selectedImageIndex = -1;
    selectedCornerIndex = -1;
    redrawCanvas();
};

onMounted(() => {
    setUpCanvas("canvas");

    redrawAll();
});

defineExpose({
    redrawAll
});
</script>

<template>
    <canvas class="my-canvas" id="canvas" :width="size.width" :height="size.height"></canvas>
</template>
<style>
.my-canvas {
    border: 1px solid black;
    margin-top: 20px;
    cursor: move;
    background-color: white;
    max-width: 100%;
}
</style>
