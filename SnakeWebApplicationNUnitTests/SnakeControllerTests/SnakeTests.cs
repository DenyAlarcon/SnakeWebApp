using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SnakeWebApplication.Controllers;
using SnakeWebApplication.GameManager;
using System.Collections.Generic;
using System.Linq;
using SnakeWebApplication.Models;

namespace SnakeWebApplicationNUnitTests.SnakeControllerTests
{
    public class SnakeTests
    {
        private SnakeController _snakeController;
        private OkObjectResult _okResultSnake;
        private StateKeeper _stateKeeper;

        [SetUp]
        public void Setup()
        {
            _snakeController = new SnakeController();
            _stateKeeper = new StateKeeper(-1, 20, 20);
            _okResultSnake = _snakeController.Snake(_stateKeeper) as OkObjectResult;
        }

        [Test]
        public void Snake_ByDefault_ReturnOkResult()
        {
            Assert.NotNull(_okResultSnake);
            Assert.AreEqual(200, _okResultSnake.StatusCode);
        }

        [Test]
        public void Snake_ByDefault_ReturnListOfPoints()
        {
            Assert.NotNull(_okResultSnake);
            Assert.AreEqual(typeof(List<Point>), _okResultSnake.Value.GetType());
        }

        [Test]
        public void Snake_ByDefault_ReturnListOfMinTwoPoints()
        {
            var okResultSnakeListOfPoints = _okResultSnake.Value as List<Point>;
            Assert.NotNull(okResultSnakeListOfPoints);
            Assert.IsTrue(okResultSnakeListOfPoints.Count() > 1);
        }

        [Test]
        [TestCase(DirectionEnum.Left, -1 , 0)]
        [TestCase(DirectionEnum.Right, 1, 0)]
        [TestCase(DirectionEnum.Top, 0, -1)]
        [TestCase(DirectionEnum.Bottom, 0, 1)]
        public void Snake_AfterMove_ReturnSnakeWithChangedCoordinates(
            DirectionEnum direction, int xCoordinateChange, int yCoordinateChange)
        {
            Assert.NotNull(_okResultSnake);
            Assert.AreEqual(200, _okResultSnake.StatusCode);

            var oldCoordinates = _okResultSnake.Value as List<Point>;
            Assert.NotNull(oldCoordinates);

            var okResultDirection = _snakeController.Direction(new DirectionRequestModel
            {
                Direction = direction
            }, _stateKeeper) as OkResult;
            Assert.NotNull(okResultDirection);
            Assert.AreEqual(200, okResultDirection.StatusCode);

            _stateKeeper.MoveSnake(null);

            var newOkResultSnake = _snakeController.Snake(_stateKeeper) as OkObjectResult;
            Assert.NotNull(newOkResultSnake);
            Assert.AreEqual(200, newOkResultSnake.StatusCode);

            var newCoordinatesEnumerable = newOkResultSnake.Value as IEnumerable<Point>;
            Assert.NotNull(newCoordinatesEnumerable);

            var newCoordinates = newCoordinatesEnumerable.ToList();
            Assert.NotNull(newCoordinates);

            Assert.AreEqual(oldCoordinates[0].X + xCoordinateChange, newCoordinates[0].X);
            Assert.AreEqual(oldCoordinates[0].Y + yCoordinateChange, newCoordinates[0].Y);
        }
    }
}
