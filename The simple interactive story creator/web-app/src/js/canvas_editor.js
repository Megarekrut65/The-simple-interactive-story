const canvas = document.getElementById('canvas');
const ctx = canvas.getContext('2d');
let images = [];
let isDragging = false;
let isResizing = false;
let prevX, prevY;
let selectedImageIndex = -1;
let selectedCornerIndex = -1;

// Function to handle image loading
function loadImage(event) {
    const files = event.target.files;

    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const reader = new FileReader();

        reader.onload = function () {
            const img = new Image();
            img.onload = function () {
                const newImage = {
                    img: img,
                    x: 0,
                    y: 0,
                    width: img.width,
                    height: img.height
                };
                images.push(newImage);
                redrawCanvas();
            }
            img.src = reader.result;
        }

        reader.readAsDataURL(file);
    }
}

// Function to redraw the canvas with all images
// Function to redraw the canvas with all images
function redrawCanvas() {
    // eslint-disable-next-line no-self-assign
    canvas.width = canvas.width; // clear canvas

    images.forEach((image, index) => {
        ctx.drawImage(image.img, image.x, image.y, image.width, image.height);
        drawResizingCircles(index, image.x, image.y, image.width, image.height);
    });
}


// Function to draw resizing circles at the corners of an image
// Function to draw resizing circles at the corners of an image
function drawResizingCircles(index, x, y, width, height) {
    if (selectedImageIndex != index) return;
    const circleRadius = 5;
    ctx.fillStyle = 'blue';
    ctx.beginPath();
    ctx.arc(x, y, circleRadius, 0, Math.PI * 2);
    ctx.stroke();
    ctx.beginPath();
    ctx.arc(x + width, y, circleRadius, 0, Math.PI * 2);
    ctx.stroke();
    ctx.beginPath();
    ctx.arc(x + width, y + height, circleRadius, 0, Math.PI * 2);
    ctx.stroke();
    ctx.beginPath();
    ctx.arc(x, y + height, circleRadius, 0, Math.PI * 2);
    //ctx.closePath(); // Close the path to ensure only the outlines are drawn
    ctx.stroke(); // Use stroke() instead of fill() to draw outlines
    ctx.strokeRect(x, y, width, height);
}


// Function to handle mouse down event
function handleMouseDown(event) {
    const mouseX = event.offsetX;
    const mouseY = event.offsetY;

    // Check if any image is clicked
    for (let i = images.length - 1; i >= 0; i--) {
        const image = images[i];
        const corner = getCornerUnderMouse(image, mouseX, mouseY);
        if (corner !== -1) {
            selectedImageIndex = i;
            selectedCornerIndex = corner;
            isResizing = true;
            prevX = mouseX;
            prevY = mouseY;
            return;
        } else if (mouseX >= image.x && mouseX <= image.x + image.width &&
            mouseY >= image.y && mouseY <= image.y + image.height) {
            selectedImageIndex = i;
            isDragging = true;
            prevX = mouseX;
            prevY = mouseY;
            return;
        }
    }

    // If no image is clicked, reset selectedImageIndex
    selectedImageIndex = -1;
    selectedCornerIndex = -1;
}

// Function to handle mouse move event
function handleMouseMove(event) {
    if (isDragging && selectedImageIndex !== -1) {
        const mouseX = event.offsetX;
        const mouseY = event.offsetY;
        const dx = mouseX - prevX;
        const dy = mouseY - prevY;

        const selectedImage = images[selectedImageIndex];
        selectedImage.x += dx;
        selectedImage.y += dy;

        redrawCanvas();

        prevX = mouseX;
        prevY = mouseY;
    } else if (isResizing && selectedImageIndex !== -1 && selectedCornerIndex !== -1) {
        const mouseX = event.offsetX;
        const mouseY = event.offsetY;
        const dx = mouseX - prevX;
        const dy = mouseY - prevY;

        const selectedImage = images[selectedImageIndex];
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

        redrawCanvas();

        prevX = mouseX;
        prevY = mouseY;
    }
}

// Function to handle mouse up event
function handleMouseUp() {
    isDragging = false;
    isResizing = false;
}

// Function to determine if the mouse is over one of the resizing circles
function getCornerUnderMouse(image, mouseX, mouseY) {
    const tolerance = 5;
    const circleRadius = 5;
    const corners = [
        { x: image.x, y: image.y },
        { x: image.x + image.width, y: image.y },
        { x: image.x + image.width, y: image.y + image.height },
        { x: image.x, y: image.y + image.height }
    ];

    for (let i = 0; i < corners.length; i++) {
        const corner = corners[i];
        const distance = Math.sqrt(Math.pow(mouseX - corner.x, 2) + Math.pow(mouseY - corner.y, 2));
        if (distance <= circleRadius + tolerance) {
            return i;
        }
    }

    return -1;
}

// Function to handle image change event
function handleImageChange(event) {
    loadImage(event);
}

// Event listeners
document.getElementById('fileInput').addEventListener('change', handleImageChange);
canvas.addEventListener('mousedown', handleMouseDown);
canvas.addEventListener('mousemove', handleMouseMove);
canvas.addEventListener('mouseup', handleMouseUp);

/*
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Image Editor</title>
  <link rel="stylesheet" href="styles.css">
</head>
<body>
  <input type="file" id="fileInput">
  <canvas id="canvas" width="400" height="250"></canvas>
  
  <script src="script.js"></script>
</body>
</html>
canvas {
  border: 1px solid black;
  margin-top: 20px;
  cursor: move;
}

*/