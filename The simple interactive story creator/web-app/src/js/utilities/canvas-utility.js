export const drawCircle = (ctx, x, y, radius) => {
    ctx.beginPath();
    ctx.arc(x, y, radius, 0, Math.PI * 2);
    ctx.stroke();
}

export const drawResizingCircles = (ctx, selectedImageIndex, index, image) => {
    if (selectedImageIndex != index) return;

    const circleRadius = 5;
    ctx.fillStyle = "blue";

    drawCircle(ctx, image.x, image.y, circleRadius);
    drawCircle(ctx, image.x + image.width, image.y, circleRadius);
    drawCircle(ctx, image.x + image.width, image.y + image.height, circleRadius);
    drawCircle(ctx, image.x, image.y + image.height, circleRadius);
    ctx.strokeRect(image.x, image.y, image.width, image.height);
};

export const collidePoint = (event, image) => {
    const mouseX = event.offsetX;
    const mouseY = event.offsetY;

    return mouseX >= image.x && mouseX <= image.x + image.width &&
        mouseY >= image.y && mouseY <= image.y + image.height;
};

export const getCornerUnderMouse = (image, mouseX, mouseY) => {
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
};


