using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NUnit.Framework;
using SnakeWebApplication.Controllers;
using SnakeWebApplication.GameManager;
using SnakeWebApplication.Models;

namespace SnakeWebApplicationNUnitTests.SnakeControllerTests
{
    public class GameBoardTests
    {
        private SnakeController _snakeController;
        private OkObjectResult _okResultGameBoard;
        private StateKeeper _stateKeeper;

        [SetUp]
        public void Setup()
        {
            _snakeController = new SnakeController();
            _stateKeeper = new StateKeeper(200, 20, 20);
            _okResultGameBoard = _snakeController.GameBoard(_stateKeeper) as OkObjectResult;
        }

        [Test]
        public void GameBoard_ByDefault_ReturnOkResult()
        {
            Assert.NotNull(_okResultGameBoard);
            Assert.AreEqual(200, _okResultGameBoard.StatusCode);
        }

        [Test]
        public void GameBoard_ByDefault_ReturnGameBoardResponseModel()
        {
            Assert.NotNull(_okResultGameBoard);
            Assert.AreEqual(typeof(GameBoardResponseModel), _okResultGameBoard.Value.GetType());
        }
    }
}
