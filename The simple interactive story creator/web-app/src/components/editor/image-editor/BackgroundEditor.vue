<script setup>
import { computed, onMounted } from "vue";
import { calculateNewDimensions, collidePoint, drawRotatedImage, drawRotatedResizingCircles, getCornerUnderMouse, getRotatorUnderMouse, rotateDelta } from "@/js/utilities/canvas-utility";
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
    },
    onPress: {
        type: Function,
        required: true
    }
});

const imagesContainer = computed(() => props.images);

let canvas;
let ctx;
let isDragging = false;
let isResizing = false;
let isRotating = false;
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
    props.onPress(selectedImageIndex);
    // eslint-disable-next-line no-self-assign
    canvas.width = canvas.width; // clear canvas

    if (background.value) ctx.drawImage(background.value, 0, 0, size.width, size.height);

    imagesContainer.value.forEach((image, index) => {
        if (!image) return;
        const draw = props.draws[image.img.name];
        if (!draw) return;

        drawRotatedImage(ctx, draw, image.rect);
        if (selectedImageIndex === index) drawRotatedResizingCircles(ctx, image.rect);
    });
};

const handleMouseUp = () => {
    isDragging = false;
    isResizing = false;
    isRotating = false;
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


        if (!image.rect.rotation) image.rect.rotation = 0;




        const corner = getCornerUnderMouse(image.rect, mouseX, mouseY);
        if (corner !== -1) {
            selectedImageIndex = i;
            selectedCornerIndex = corner;
            isResizing = true;
            prevX = mouseX;
            prevY = mouseY;

            redrawCanvas();

            return;
        }
        const rotator = getRotatorUnderMouse(image.rect, mouseX, mouseY);
        if (rotator) {
            selectedImageIndex = i;
            isRotating = true;
            prevX = mouseX;
            prevY = mouseY;
            redrawCanvas();
            return;
        }

        if (collidePoint(mousePos, image.rect)) {
            selectedImageIndex = i;
            isDragging = true;
            prevX = mouseX;
            prevY = mouseY;

            redrawCanvas();

            return;
        }
    }

    selectedImageIndex = -1;
    selectedCornerIndex = -1;

    redrawCanvas();
};
const rotateImage = (event) => {
    if (!isRotating) return;

    const { mouseX, mouseY } = getMousePos(event);

    const selectedImage = imagesContainer.value[selectedImageIndex].rect;
    if (!selectedImage.rotation) selectedImage.rotation = 0;

    const centerX = selectedImage.x + selectedImage.width / 2;
    const centerY = selectedImage.y + selectedImage.height / 2;

    const angle1 = Math.atan2(prevY - centerY, prevX - centerX);
    const angle2 = Math.atan2(mouseY - centerY, mouseX - centerX);
    const angleDelta = angle2 - angle1;

    selectedImage.rotation += angleDelta * (180 / Math.PI);

    prevX = mouseX;
    prevY = mouseY;
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

    const selectedImage = imagesContainer.value[selectedImageIndex].rect;
    const dx = mouseX - prevX;
    const dy = mouseY - prevY;

    const { dx: rotatedDx, dy: rotatedDy } = rotateDelta(dx, dy, -((selectedImage.rotation + 360) % 360));

    const { newX, newY, newWidth, newHeight } = calculateNewDimensions(event, selectedImage, selectedCornerIndex, rotatedDx, rotatedDy);

    selectedImage.x = newX;
    selectedImage.y = newY;
    selectedImage.width = newWidth;
    selectedImage.height = newHeight;

    prevX = mouseX;
    prevY = mouseY;

};

const handleMouseMove = (event) => {
    if (!isDragging && !isResizing && !isRotating) return;

    if (isDragging && selectedImageIndex !== -1) dragImage(event);
    else if (isResizing && selectedImageIndex !== -1 && selectedCornerIndex !== -1) resizeImage(event);
    else if (isRotating && selectedImageIndex !== -1) rotateImage(event);

    redrawCanvas();
    if (selectedImageIndex !== -1) {
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

onMounted(() => {
    setUpCanvas("canvas");

    redrawCanvas();
});

const redrawAll = (selectedImage = -1) => {
    selectedImageIndex = selectedImage;
    redrawCanvas();
};

defineExpose({
    redrawAll,
    selectedImageIndex
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
