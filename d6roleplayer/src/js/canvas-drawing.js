"use strict";

var canvas,
    context,
    colorBlack = "#000000",
    colorGrey = "#4b4b4b",
    colorBlue = "#3366ff",
    colorGreen = "#30ff59",
    colorYellow = "#fffb00",
    colorRed = "#ff0000",
    colorOrange = "#ff7d40",
    colorPink = "#ff40df",
    colorWhite = "#ffffff",
    paint = false,
    curColor = colorBlack,
    clickX = [],
    clickY = [],
    clickColor = [],
    clickDrag = [],
    prevClickX = [],
    prevClickY = [],
    prevClickColor = [],
    prevClickDrag = [],
    canvasWidth = 600,
    canvasHeight = 600,
    mediumStartX = 18,
    mediumStartY = 19,
    mediumImageWidth = 93,
    mediumImageHeight = 24,
    drawingAreaX = 111,
    drawingAreaY = 11,
    drawingAreaWidth = 450,
    drawingAreaHeight = 550;

document.addEventListener("DOMContentLoaded", function () {
    init();
});

connection.on("UpdateDrawing", function (x, y, color, drag) {
    clickX = clickX.concat(x);
    clickY = clickY.concat(y);
    clickColor = clickColor.concat(color);
    clickDrag = clickDrag.concat(drag);
    redraw();
});

connection.on("SyncDrawing", function (x, y, color, drag) {
    clickX = x;
    clickY = y;
    clickColor = color;
    clickDrag = drag;
    redraw();
});

connection.on("ResetDrawing", function () {
    clickX = [];
    clickY = [];
    clickColor = [];
    clickDrag = [];
    prevClickX = [];
    prevClickY = [];
    prevClickColor = [];
    prevClickDrag = [];
    redraw();
});

document.getElementById("syncDrawing").addEventListener("click", function (event) {
    connection.invoke("RequestSyncDrawing", clickX, clickY, clickColor, clickDrag)
        .catch(function (err) {
            window.alert("Connection to the session has been lost. Please reload the page." + err.toString());
            return console.error(err.toString());
        });
    event.preventDefault();
});

document.getElementById("resetDrawing").addEventListener("click", function (event) {
    connection.invoke("RequestResetDrawing")
        .catch(function (err) {
            window.alert("Connection to the session has been lost. Please reload the page." + err.toString());
            return console.error(err.toString());
        });
    event.preventDefault();
});

// Clears the canvas.
var clearCanvas = function () {

    context.clearRect(0, 0, canvasWidth, canvasHeight);
};

// Redraws the canvas.
var redraw = function () {

    var locX,
        locY,
        radius,
        i,
        selected;

    var drawMarker = function (x, y, color) {

        context.beginPath();
        context.arc(x + 10, y + 24, 10, 0, 2 * Math.PI);
        context.closePath();
        context.fillStyle = color;
        context.fill();
    };

    var drawEraser = function (x, y) {

        context.beginPath();
        context.arc(x + 10, y + 24, 10, 0, 2 * Math.PI);
        context.stroke();
    };

    clearCanvas();

    selected = (curColor === colorBlack);
    locX = selected ? 18 : 52;
    locY = 10;
    drawMarker(locX, locY, colorBlack);

    selected = (curColor === colorGrey);
    locX = selected ? 18 : 52;
    locY += mediumImageHeight;
    drawMarker(locX, locY, colorGrey);

    selected = (curColor === colorBlue);
    locX = selected ? 18 : 52;
    locY += mediumImageHeight;
    drawMarker(locX, locY, colorBlue);

    selected = (curColor === colorGreen);
    locX = selected ? 18 : 52;
    locY += mediumImageHeight;
    drawMarker(locX, locY, colorGreen);

    selected = (curColor === colorYellow);
    locX = selected ? 18 : 52;
    locY += mediumImageHeight;
    drawMarker(locX, locY, colorYellow);

    selected = (curColor === colorRed);
    locX = selected ? 18 : 52;
    locY += mediumImageHeight;
    drawMarker(locX, locY, colorRed);

    selected = (curColor === colorOrange);
    locX = selected ? 18 : 52;
    locY += mediumImageHeight;
    drawMarker(locX, locY, colorOrange);

    selected = (curColor === colorPink);
    locX = selected ? 18 : 52;
    locY += mediumImageHeight;
    drawMarker(locX, locY, colorPink);

    selected = (curColor === colorWhite);
    locX = selected ? 18 : 52;
    locY += mediumImageHeight;
    drawEraser(locX, locY);

    // Draw the canvas.
    context.beginPath();
    context.rect(drawingAreaX, drawingAreaY, drawingAreaWidth, drawingAreaHeight);
    context.strokeStyle = "#708090";
    context.stroke();

    // Keep the drawing in the drawing area.
    context.save();
    context.beginPath();
    context.rect(drawingAreaX, drawingAreaY, drawingAreaWidth, drawingAreaHeight);
    context.clip();

    // For each point drawn.
    for (i = 0; i < clickX.length; i += 1) {

        // Set the drawing path.
        context.beginPath();

        // If dragging then draw a line between the two points.
        if (clickDrag[i] && i) {
            context.moveTo(clickX[i - 1], clickY[i - 1]);
        } else {
            // The x position is moved over one pixel so a circle even if not dragging.
            context.moveTo(clickX[i] - 1, clickY[i]);
        }

        context.lineTo(clickX[i], clickY[i]);
        context.strokeStyle = clickColor[i];

        if (clickColor[i] == colorWhite) {
            radius = 35;
        } else {
            radius = 5;
        }

        context.lineCap = "round";
        context.lineJoin = "round";
        context.lineWidth = radius;
        context.stroke();
    }
    context.closePath();
    context.restore();

    context.globalAlpha = 1;
};

// Adds a point to the drawing array.
var addClick = function (x, y, dragging) {
    clickX.push(x);
    clickY.push(y);
    clickColor.push(curColor);
    clickDrag.push(dragging);

    prevClickX.push(x);
    prevClickY.push(y);
    prevClickColor.push(curColor);
    prevClickDrag.push(dragging);
};

// Add mouse and touch event listeners to the canvas.
var createUserEvents = function () {

    var press = function (e) {
        // Mouse down location.
        var mouseX = (e.changedTouches ? e.changedTouches[0].pageX : e.pageX) - this.offsetLeft;
        var mouseY = (e.changedTouches ? e.changedTouches[0].pageY : e.pageY) - this.offsetTop;

        if (mouseX < drawingAreaX) { // Left of the drawing area.
            if (mouseX > mediumStartX) {
                if (mouseY > mediumStartY && mouseY < mediumStartY + mediumImageHeight) {
                    curColor = colorBlack;
                } else if (mouseY > mediumStartY + mediumImageHeight && mouseY < mediumStartY + mediumImageHeight * 2) {
                    curColor = colorGrey;
                } else if (mouseY > mediumStartY + mediumImageHeight * 2 && mouseY < mediumStartY + mediumImageHeight * 3) {
                    curColor = colorBlue;
                } else if (mouseY > mediumStartY + mediumImageHeight * 3 && mouseY < mediumStartY + mediumImageHeight * 4) {
                    curColor = colorGreen;
                } else if (mouseY > mediumStartY + mediumImageHeight * 4 && mouseY < mediumStartY + mediumImageHeight * 5) {
                    curColor = colorYellow;
                } else if (mouseY > mediumStartY + mediumImageHeight * 5 && mouseY < mediumStartY + mediumImageHeight * 6) {
                    curColor = colorRed;
                } else if (mouseY > mediumStartY + mediumImageHeight * 6 && mouseY < mediumStartY + mediumImageHeight * 7) {
                    curColor = colorOrange;
                } else if (mouseY > mediumStartY + mediumImageHeight * 7 && mouseY < mediumStartY + mediumImageHeight * 8) {
                    curColor = colorPink;
                } else if (mouseY > mediumStartY + mediumImageHeight * 8 && mouseY < mediumStartY + mediumImageHeight * 9) {
                    curColor = colorWhite;
                }
            }
        }

        paint = true;
        addClick(mouseX, mouseY, false);
        redraw();
    };

    var drag = function (e) {

        var mouseX = (e.changedTouches ? e.changedTouches[0].pageX : e.pageX) - this.offsetLeft,
            mouseY = (e.changedTouches ? e.changedTouches[0].pageY : e.pageY) - this.offsetTop;

        if (paint) {
            addClick(mouseX, mouseY, true);
            redraw();
        }
        // Prevent the whole page from dragging if on mobile.
        e.preventDefault();
    };

    var release = function () {
        paint = false;
        redraw();
        connection.invoke("RequestDrawingClick", prevClickX, prevClickY, prevClickColor, prevClickDrag)
            .catch(function (err) {
                window.alert("Connection to the session has been lost. Please reload the page." + err.toString());
                return console.error(err.toString());
            });

        prevClickX = [];
        prevClickY = [];
        prevClickColor = [];
        prevClickDrag = [];
    };

    var cancel = function () {
        paint = false;
    };

    // Add mouse event listeners to canvas element.
    canvas.addEventListener("mousedown", press, false);
    canvas.addEventListener("mousemove", drag, false);
    canvas.addEventListener("mouseup", release);
    canvas.addEventListener("mouseout", cancel, false);

    // Add touch event listeners to canvas element.
    canvas.addEventListener("touchstart", press, false);
    canvas.addEventListener("touchmove", drag, false);
    canvas.addEventListener("touchend", release, false);
    canvas.addEventListener("touchcancel", cancel, false);
};

// Creates a canvas element, loads images, adds events, and draws the canvas for the first time.
var init = function () {

    canvas = document.createElement('canvas');
    canvas.setAttribute('width', canvasWidth);
    canvas.setAttribute('height', canvasHeight);
    canvas.setAttribute('id', 'canvas');
    document.getElementById('canvasDiv').appendChild(canvas);

    if (typeof G_vmlCanvasManager !== "undefined") {
        canvas = G_vmlCanvasManager.initElement(canvas);
    }

    context = canvas.getContext("2d");

    redraw();
    createUserEvents();
};