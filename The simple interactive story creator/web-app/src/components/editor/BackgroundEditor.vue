<script setup>
import { onMounted, watch } from "vue";
import { collidePoint, drawResizingCircles, getCornerUnderMouse } from "@/js/utilities/canvas-utility";
import { makeAspectRatio } from "@/js/utilities/size-utility";

const props = defineProps({
    images: {
        type: Array,
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


const redrawCanvas = () => {
    // eslint-disable-next-line no-self-assign
    canvas.width = canvas.width; // clear canvas


    imagesContainer.forEach((image, index) => {
        ctx.drawImage(image.img, image.x, image.y, image.width, image.height);
        drawResizingCircles(ctx, selectedImageIndex, index, image);
    });

};

const handleMouseUp = () => {
    isDragging = false;
    isResizing = false;
};

const handleMouseDown = (event) => {
    const mouseX = event.offsetX;
    const mouseY = event.offsetY;

    for (let i = imagesContainer.length - 1; i >= 0; i--) {
        const image = imagesContainer[i];
        const corner = getCornerUnderMouse(image, mouseX, mouseY);
        if (corner !== -1) {
            selectedImageIndex = i;
            selectedCornerIndex = corner;
            isResizing = true;
            prevX = mouseX;
            prevY = mouseY;
            console.log("resize")

            return;
        }

        if (collidePoint(event, image)) {
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
    const mouseX = event.offsetX;
    const mouseY = event.offsetY;
    const dx = mouseX - prevX;
    const dy = mouseY - prevY;

    const selectedImage = imagesContainer[selectedImageIndex];
    selectedImage.x += dx;
    selectedImage.y += dy;
};

const reiszeImage = (event) => {
    const mouseX = event.offsetX;
    const mouseY = event.offsetY;
    const dx = mouseX - prevX;
    const dy = mouseY - prevY;

    const selectedImage = imagesContainer[selectedImageIndex];
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
};

const handleMouseMove = (event) => {
    if (!isDragging && !isResizing) return;


    if (isDragging && selectedImageIndex !== -1) dragImage(event);
    else if (isResizing && selectedImageIndex !== -1 && selectedCornerIndex !== -1) reiszeImage(event);

    redrawCanvas();

    prevX = event.offsetX;
    prevY = event.offsetY;
};



const setUpCanvas = (canvasId) => {
    canvas = document.getElementById(canvasId);
    if (canvas) {
        ctx = canvas.getContext("2d");
        canvas.addEventListener("mousedown", handleMouseDown);
        canvas.addEventListener("mousemove", handleMouseMove);
        canvas.addEventListener("mouseup", handleMouseUp);
        canvas.addEventListener("resize", () => {
            ctx = canvas.getContext("2d");
        });

        makeAspectRatio(canvasId, (width) => width / 1.7, () => {
            ctx = canvas.getContext("2d");
            redrawCanvas();
        });
    }
};


onMounted(() => {
    setUpCanvas("canvas");

    watch(props.images, (_, newOne) => {
        let added = false;
        for (let i in newOne) {
            if (!imagesContainer.includes(newOne[i])) {
                const newImage = {
                    img: newOne[i],
                    x: 5,
                    y: 5,
                    width: newOne[i].width,
                    height: newOne[i].height
                };
                imagesContainer.push(newImage);
                added = true;
            }
        }

        if (added) {
            redrawCanvas();
        }

    });
});


</script>

<template>
    <canvas class="my-canvas" id="canvas" width="1920" height="1080"></canvas>
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
