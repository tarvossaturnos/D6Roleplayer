"use strict";

var canvas,
    context,
    paint = false,
    curColor = '#000000',
    clickX = [],
    clickY = [],
    clickColor = [],
    clickDrag = [],
    prevClickX = [],
    prevClickY = [],
    prevClickColor = [],
    prevClickDrag = [];

window.addEventListener("load", function () {
    initCanvas();
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
    context.clearRect(0, 0, canvas.width, canvas.height);
};

var redraw = function () {
    var radius;
    var i;

    clearCanvas();

    // For each point stored in the array.
    for (i = 0; i < clickX.length; i += 1) {
        context.beginPath();

        // Dragging.
        if (clickDrag[i] && i) {
            context.moveTo(clickX[i - 1], clickY[i - 1]);
        } else {
            // Single click.
            context.moveTo(clickX[i] - 1, clickY[i]);
        }

        context.lineTo(clickX[i], clickY[i]);
        context.strokeStyle = clickColor[i];

        if (clickColor[i] == "#ffffff") {
            radius = 5;
        } else {
            radius = 1;
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

var createUserEvents = function () {
    var press = function (e) {
        var mousePos = getMousePos(e);
        paint = true;
        addClick(mousePos.x, mousePos.y, false);
        redraw();
    };

    var drag = function (e) {
        if (paint) {
            var mousePos = getMousePos(e);
            addClick(mousePos.x, mousePos.y, true);
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

    canvas.addEventListener("mousedown", press, false);
    canvas.addEventListener("mousemove", drag, false);
    canvas.addEventListener("mouseup", release);
    canvas.addEventListener("mouseout", cancel, false);

    canvas.addEventListener("touchstart", press, false);
    canvas.addEventListener("touchmove", drag, false);
    canvas.addEventListener("touchend", release, false);
    canvas.addEventListener("touchcancel", cancel, false);
};

var initCanvas = function () {
    canvas = document.createElement('canvas');
    canvas.setAttribute('id', 'canvas');
    document.getElementById('canvasDiv').appendChild(canvas);

    canvas.width = 600;
    canvas.height = 600;

    if (typeof G_vmlCanvasManager !== "undefined") {
        canvas = G_vmlCanvasManager.initElement(canvas);
    }

    context = canvas.getContext("2d");

    redraw();
    createUserEvents(); 
};

function getMousePos(e) {

    var rect = canvas.getBoundingClientRect();
    return {
        x: Math.round((e.clientX - rect.left) / (rect.right - rect.left) * canvas.width),
        y: Math.round((e.clientY - rect.top) / (rect.bottom - rect.top) * canvas.height)
    };
}

function selectColor(color) {
    curColor = color;
}