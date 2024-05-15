<script setup>
import { onMounted, watch } from "vue";
import { collidePoint, drawResizingCircles, getCornerUnderMouse } from "@/js/utilities/canvas-utility";
import { getNormSize } from "@/js/utilities/size-utility";

const props = defineProps({
    images: {
        type: Array,
        required: true
    },
    currentScene: {
        type: Object,
        required: true
    }
});

const imagesContainer = [];

let canvas;
let ctx;
let isDragging = false;
let isResizing = false;
let prevX, prevY;
let selectedImageIndex = -1;
let selectedCornerIndex = -1;

const size = { width: 1920, height: 1080 };


const redrawCanvas = () => {
    // eslint-disable-next-line no-self-assign
    canvas.width = canvas.width; // clear canvas

    ctx.drawImage(props.currentScene.background, 0, 0, size.width, size.height);

    imagesContainer.forEach((image, index) => {

        ctx.drawImage(image.img, image.x, image.y, image.width, image.height);
        drawResizingCircles(ctx, selectedImageIndex, index, image);
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

    for (let i = imagesContainer.length - 1; i >= 0; i--) {
        const image = imagesContainer[i];
        const corner = getCornerUnderMouse(image, mouseX, mouseY);
        if (corner !== -1) {
            selectedImageIndex = i;
            selectedCornerIndex = corner;
            isResizing = true;
            prevX = mouseX;
            prevY = mouseY;

            return;
        }

        if (collidePoint(mousePos, image)) {
            selectedImageIndex = i;
            isDragging = true;
            prevX = mouseX;
            prevY = mouseY;

            return;
        }
    }

    selectedImageIndex = -1;
    selectedCornerIndex = -1;
};
const dragImage = (event) => {
    const { mouseX, mouseY } = getMousePos(event);
    const dx = mouseX - prevX;
    const dy = mouseY - prevY;

    const selectedImage = imagesContainer[selectedImageIndex];
    selectedImage.x += dx;
    selectedImage.y += dy;
};

const resizeImage = (event) => {
    const { mouseX, mouseY } = getMousePos(event);
    const dx = mouseX - prevX;
    const dy = mouseY - prevY;

    const selectedImage = imagesContainer[selectedImageIndex];

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
    redrawCanvas();

    if (!isDragging && !isResizing) return;


    if (isDragging && selectedImageIndex !== -1) dragImage(event);
    else if (isResizing && selectedImageIndex !== -1 && selectedCornerIndex !== -1) resizeImage(event);

    redrawCanvas();

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

    watch(props.images, (_, newOne) => {
        let changed = false;

        newOne.forEach(image => {
            if (imagesContainer.some(item => item.id === image.id)) return;

            const newImage = {
                id: image.id,
                img: image,
                x: 5,
                y: 5,
                width: image.width,
                height: image.height
            };
            imagesContainer.push(newImage);
            changed = true;
        });

        imagesContainer.forEach((image, index) => {
            if (newOne.some(item => item.id === image.id)) return;

            imagesContainer.splice(index, 1);
            changed = true;
        });

        if (changed) {
            selectedImageIndex = -1;
            selectedCornerIndex = -1;
            redrawCanvas();
        }
    });

    watch(props.currentScene, () => {
        redrawCanvas();
    });
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
