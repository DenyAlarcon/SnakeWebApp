// canvas
const canvas = document.getElementById("gameBoard");

// canvas context
const context = canvas.getContext("2d");

// cell size
const cellSize = 7;

// token
const token = "APsZtyZK2KbPxYN8pC8j";

// is game started
let isStarted = false;

// is game paused
let isPaused = false;

// time turn milliseconds
let turnTimeMilliseconds;

// game board size
let gameBoardSize;

//other players
let players;

// snake position
let snake;

// food position
let food;

// walls
let walls;

// snake direction
let jsonDirection = {
    "direction": "Up",
    "token": token
};

GetInitialGameState();

$(document).keydown(function (e) {
    if (e.which === 37) {
        jsonDirection.direction = "Left";
    }
    else if (e.which === 38) {
        jsonDirection.direction = "Up";
    }
    else if (e.which === 39) {
        jsonDirection.direction = "Right";
    }
    else if (e.which === 40) {
        jsonDirection.direction = "Down";
    } else {
        return;
    }

    fetch("http://167.172.186.24/api/Player/direction", {
        method: "POST",
        body: JSON.stringify(jsonDirection),
        headers: {
            'Content-Type': "application/json; charset=utf-8"
        }
    });
});

async function GetInitialGameState() {
    // send request to get initial game state
    await fetch(`http://167.172.186.24/api/Player/gameboard?token=${token}`)
        .then(response => response.json())
        .then(function (responseJson) {

            isStarted = responseJson["isStarted"];
            isPaused = responseJson["isPaused"];
            turnTimeMilliseconds = responseJson["turnTimeMilliseconds"];
            gameBoardSize = responseJson["gameBoardSize"];

            canvas.width = gameBoardSize["width"] * cellSize;
            canvas.height = gameBoardSize["height"] * cellSize;

            if (isStarted && !isPaused) {
                setInterval(GetCurrentGameState, turnTimeMilliseconds);
                setInterval(Draw, 30);
            }
        }); 
}

async function GetCurrentGameState() {
    await fetch(`http://167.172.186.24/api/Player/gameboard?token=${token}`)
        .then(response => response.json())
        .then(function (responseJson) {

            players = responseJson["players"];
            snake = responseJson["snake"];
            food = responseJson["food"];
            walls = responseJson["walls"];

            $("#turn-number").text(`${responseJson["turnNumber"]}`);
            $("#round-number").text(`${responseJson["roundNumber"]}`);
            $("#max-food").text(`${responseJson["maxFood"]}`);

            $("#players-table-body").html("");
            for (let i = 0; i < players.length; i++) {
                if (players[i].snake != null) {
                    $("#players-table-body").append(`<tr>
                    <td>${players[i].name}</td>
                    <td class="px-3">${players[i].snake.length}</td>
                    </tr>`);
                }
            }
        });
}

// draw everything to the canvas
function Draw() {
    // clear canvas
    context.clearRect(0, 0, canvas.width, canvas.height);

    // draw grid
    for (let x = cellSize; x < canvas.width; x += cellSize) {
        context.moveTo(x, 0);
        context.lineTo(x, canvas.height);
    }
    for (let y = cellSize; y < canvas.height; y += cellSize) {
        context.moveTo(0, y);
        context.lineTo(canvas.width, y);
    }
    context.strokeStyle = "#EAEAEA";
    context.stroke();

    //draw players snakes
    for (let i = 0; i < players.length; i++) {
        if (players[i].snake != null) {
            for (let j = 0; j < players[i].snake.length; j++) {
                if (players[i].isSpawnProtected) {
                    context.fillStyle = (j === 0) ? "#EEE555" : "yellow";
                }
                else if (players[i].snake.length >= snake.length) {
                    context.fillStyle = (j === 0) ? "#9000FF" : "#CE00FF";
                }
                else {
                    context.fillStyle = (j === 0) ? "blue" : "#00BCFF";
                }
                context.fillRect(players[i].snake[j].x * cellSize,
                    players[i].snake[j].y * cellSize,
                    cellSize,
                    cellSize);
            }
        }
    }

    // draw snake
    if (snake != null) {
        for (let i = 0; i < snake.length; i++) {
            context.fillStyle = (i === 0) ? "green" : "#C6FF00";
            context.fillRect(snake[i].x * cellSize, snake[i].y * cellSize, cellSize, cellSize);
        }
    }

    //draw food
    if (food != null) {
        context.fillStyle = "red";
        for (let i = 0; i < food.length; i++) {
            context.fillRect(food[i].x * cellSize, food[i].y * cellSize, cellSize, cellSize);
        }
    }

    //draw walls
    if (walls != null) {
        context.fillStyle = "black";
        for (let i = 0; i < walls.length; i++) {
            context.fillRect(
                walls[i].x * cellSize, walls[i].y * cellSize,
                walls[i].width * cellSize, walls[i].height * cellSize
            );
        }
    }
}
