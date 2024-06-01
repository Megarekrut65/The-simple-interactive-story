export const drawCircle = (ctx, x, y, radius) => {
    ctx.beginPath();
    ctx.arc(x, y, radius, 0, Math.PI * 2);
    ctx.stroke();
}
export const drawRotatedImage = (ctx, image, rect) => {
    ctx.save();

    ctx.translate(rect.x + rect.width / 2, rect.y + rect.height / 2);
    ctx.rotate(rect.rotation * Math.PI / 180);
    ctx.drawImage(image, -rect.width / 2, -rect.height / 2, rect.width, rect.height);

    ctx.restore();
};
export const drawResizingCircles = (ctx, rect) => {
    const circleRadius = 5;
    ctx.strokeStyle = "#07085d";

    drawCircle(ctx, rect.x - 3 * circleRadius, rect.y - 3 * circleRadius, circleRadius);
    drawCircle(ctx, rect.x, rect.y, circleRadius);
    drawCircle(ctx, rect.x + rect.width, rect.y, circleRadius);
    drawCircle(ctx, rect.x + rect.width, rect.y + rect.height, circleRadius);
    drawCircle(ctx, rect.x, rect.y + rect.height, circleRadius);
    ctx.strokeRect(rect.x, rect.y, rect.width, rect.height);
};

export const drawRotatedResizingCircles = (ctx, rect) => {
    ctx.save();

    ctx.translate(rect.x + rect.width / 2, rect.y + rect.height / 2);
    ctx.rotate(rect.rotation * Math.PI / 180);
    const rotatedRect = {
        x: -rect.width / 2,
        y: -rect.height / 2,
        width: rect.width,
        height: rect.height
    };

    drawResizingCircles(ctx, rotatedRect);
    ctx.restore();
};

export const collidePoint = (mousePos, image) => {
    const { mouseX, mouseY } = mousePos;

    return mouseX >= image.x && mouseX <= image.x + image.width &&
        mouseY >= image.y && mouseY <= image.y + image.height;
};

const rotatePoint = (point, center, angle) => {
    const radians = angle * Math.PI / 180;
    const cos = Math.cos(radians);
    const sin = Math.sin(radians);
    const dx = point.x - center.x;
    const dy = point.y - center.y;
    return {
        x: center.x + (dx * cos - dy * sin),
        y: center.y + (dx * sin + dy * cos)
    };
};

export const getCornerUnderMouse = (image, mouseX, mouseY) => {
    const tolerance = 5;
    const circleRadius = 8;
    const initCorners = [
        { x: image.x, y: image.y },
        { x: image.x + image.width, y: image.y },
        { x: image.x + image.width, y: image.y + image.height },
        { x: image.x, y: image.y + image.height }
    ];
    const centerX = image.x + image.width / 2;
    const centerY = image.y + image.height / 2;
    const corners = initCorners.map(corner => rotatePoint(corner, { x: centerX, y: centerY }, image.rotation));

    for (let i = 0; i < corners.length; i++) {
        const corner = corners[i];
        const distance = Math.sqrt(Math.pow(mouseX - corner.x, 2) + Math.pow(mouseY - corner.y, 2));
        if (distance <= circleRadius + tolerance) {
            return i;
        }
    }

    return -1;
};
export const getRotatorUnderMouse = (image, mouseX, mouseY) => {
    const tolerance = 8;
    const circleRadius = 5;

    const centerX = image.x + image.width / 2;
    const centerY = image.y + image.height / 2;
    const cornerInit = { x: image.x - 3 * circleRadius, y: image.y - 3 * circleRadius }
    const corner = rotatePoint(cornerInit, { x: centerX, y: centerY }, image.rotation);

    const distance = Math.sqrt(Math.pow(mouseX - corner.x, 2) + Math.pow(mouseY - corner.y, 2));
    if (distance <= circleRadius + tolerance) {
        return true
    }

    return false;
};

export const calculateNewDimensions = (event, rect, cornerIndex, dx, dy) => {
    const aspect = rect.width / rect.height;

    let newX = rect.x;
    let newY = rect.y;
    let newWidth = rect.width;
    let newHeight = rect.height;

    switch (cornerIndex) {
        case 0: // Top-left corner
            newX += dx;
            newY += dy;
            newWidth -= dx;
            newHeight -= dy;
            break;
        case 1: // Top-right corner
            newY += dy;
            newWidth += dx;
            newHeight -= dy;
            break;
        case 2: // Bottom-right corner
            newWidth += dx;
            newHeight += dy;
            break;
        case 3: // Bottom-left corner
            newX += dx;
            newWidth -= dx;
            newHeight += dy;
            break;
    }

    if (!event.shiftKey) {
        if (cornerIndex === 0 || cornerIndex === 1 || cornerIndex === 3) {
            newWidth = newHeight * aspect;
        } else {
            newHeight = newWidth / aspect;
        }
    }

    return { newX, newY, newWidth, newHeight };
};

export const rotateDelta = (dx, dy, angle) => {
    const radians = angle * Math.PI / 180;
    const cos = Math.cos(radians);
    const sin = Math.sin(radians);
    return {
        dx: dx * cos + dy * sin,
        dy: dy * cos - dx * sin
    };
};