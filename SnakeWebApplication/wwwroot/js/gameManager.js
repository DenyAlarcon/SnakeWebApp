﻿// canvas
const canvas = document.getElementById("gameBoard");

// canvas context
const context = canvas.getContext("2d");

// cell size
const cellSize = 20;

// time until next draw
let timeUntilNextDraw;

// time until next turn
let timeUntilNextTurnMilliseconds;

// game board size
let gameBoardSize;

// snake part position
let snakePart;

// food part position
let foodPart;

// snake position
let snake = [];

// food position
let food = [];

// snake direction
let jsonDirection = { "direction": "Top" };

GetInitialGameState();

$(document).keydown(function (e) {
    if (e.which === 37 && jsonDirection.direction !== "Right") {
        jsonDirection.direction = "Left";
    }
    else if (e.which === 38 && jsonDirection.direction !== "Bottom") {
        jsonDirection.direction = "Top";
    }
    else if (e.which === 39 && jsonDirection.direction !== "Left") {
        jsonDirection.direction = "Right";
    }
    else if (e.which === 40 && jsonDirection.direction !== "Top") {
        jsonDirection.direction = "Bottom";
    }

    fetch("Direction", {
        method: "POST",
        body: JSON.stringify(jsonDirection),
        headers: {
            'Content-Type': "application/json; charset=utf-8"
        }
    });
});

async function GetInitialGameState() {
    // send request to get initial game state
    await fetch("GameBoard")
        .then(response => response.json())
        .then(function (responseJson) {
            timeUntilNextTurnMilliseconds = responseJson["timeUntilNextTurnMilliseconds"];
            gameBoardSize = responseJson["gameBoardSize"];
            $("h6").text(`Turn number: ${responseJson["turnNumber"]}`);
            canvas.width = gameBoardSize["width"] * cellSize;
            canvas.height = gameBoardSize["height"] * cellSize;
            timeUntilNextDraw = timeUntilNextTurnMilliseconds / 4;
            setInterval(GetCurrentGameState, timeUntilNextTurnMilliseconds / 2);
            setInterval(Draw, timeUntilNextDraw);
        });
}

async function GetCurrentGameState()
{
    await fetch("Food")
        .then(response => response.json())
        .then(function (responseJson) {
            food = [];
            for (let i = 0; i < responseJson.length; i++) {
                foodPart = {
                    x: responseJson[i]["x"] * cellSize,
                    y: responseJson[i]["y"] * cellSize
                };
                food.push(foodPart);
            }
        });

    await fetch("Snake")
        .then(response => response.json())
        .then(function (responseJson) {
            snake = [];
            for (let i = 0; i < responseJson.length; i++) {
                snakePart = {
                    x: responseJson[i]["x"] * cellSize,
                    y: responseJson[i]["y"] * cellSize
                };
                snake.push(snakePart);
            }
        });

    await fetch("GameBoard")
        .then(response => response.json())
        .then(function (responseJson) {
            $("h6").text(`Turn number: ${responseJson["turnNumber"]}`);
            // reset direction
            if (responseJson["turnNumber"] === 0) jsonDirection.direction = "Top";
        });
}

// draw everything to the canvas
function Draw() {
    // clear canvas
    context.clearRect(0, 0, canvas.width, canvas.height);

    // draw grid
    for (let x = cellSize; x < canvas.width; x += cellSize) {
        context.moveTo(x, 0);
        context.lineTo(x, canvas.width);
    }
    for (let y = cellSize; y < canvas.height; y += cellSize) {
        context.moveTo(0, y);
        context.lineTo(canvas.height, y);
    }
    context.strokeStyle = "#EAEAEA";
    context.stroke();

    // draw snake
    for (let i = 0; i < snake.length; i++) {
        context.fillStyle = (i === 0) ? "green" : "#C6FF00";
        context.fillRect(snake[i].x, snake[i].y, cellSize, cellSize);
    }

    //draw food
    context.fillStyle = "red";
    for (let i = 0; i < food.length; i++) {
        context.fillRect(food[i].x, food[i].y, cellSize, cellSize);
    }
}
