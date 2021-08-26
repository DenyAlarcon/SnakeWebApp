using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SnakeWebApplication.Controllers;
using SnakeWebApplication.GameManager;
using System.Collections.Generic;
using System.Linq;

namespace SnakeWebApplicationNUnitTests.SnakeControllerTests
{
    public class FoodTests
    {
        private SnakeController _snakeController;
        private OkObjectResult _okResultFood;
        private StateKeeper _stateKeeper;

        [SetUp]
        public void Setup()
        {
            _snakeController = new SnakeController();
            _stateKeeper = new StateKeeper(200, 20, 20);
            _okResultFood = _snakeController.Food(_stateKeeper) as OkObjectResult;
        }

        [Test]
        public void Food_ByDefault_ReturnOkResult()
        {
            Assert.NotNull(_okResultFood);
            Assert.AreEqual(200, _okResultFood.StatusCode);
        }

        [Test]
        public void Food_ByDefault_ReturnListOfPoints()
        {
            Assert.NotNull(_okResultFood);
            Assert.AreEqual(typeof(List<Point>), _okResultFood.Value.GetType());
        }

        [Test]
        public void Food_ByDefault_ReturnListOfMinOnePoint()
        {
            Assert.NotNull(_okResultFood);
            var okResultFoodListOfPoints = _okResultFood.Value as List<Point>;
            Assert.NotNull(okResultFoodListOfPoints);
            Assert.IsTrue(okResultFoodListOfPoints.Any());
        }
    }
}
