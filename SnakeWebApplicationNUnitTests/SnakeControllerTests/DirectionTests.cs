using System;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SnakeWebApplication.Controllers;
using SnakeWebApplication.GameManager;
using SnakeWebApplication.Models;

namespace SnakeWebApplicationNUnitTests.SnakeControllerTests
{
    public class DirectionTests
    {
        private SnakeController _snakeController;
        private StateKeeper _stateKeeper;

        [SetUp]
        public void Setup()
        {
            _snakeController = new SnakeController();
            _stateKeeper = new StateKeeper(200,20,20);
        }

        [Test]
        [TestCase("Left")]
        [TestCase("Right")]
        [TestCase("Top")]
        [TestCase("Bottom")]
        public void Direction_JsonValidData_ReturnOkResult(string validData)
        {
            var okResultDirection = _snakeController.Direction(new DirectionRequestModel
            {
                Direction = (DirectionEnum)Enum.Parse(typeof(DirectionEnum), validData)
            }, _stateKeeper) as OkResult;
            Assert.NotNull(okResultDirection);
            Assert.AreEqual(200, okResultDirection.StatusCode);
        }

        [Test]
        [TestCase("None")]
        public void Direction_JsonInvalidData_ReturnBadRequestResult(string invalidData)
        {
            var badRequestResultDirection = _snakeController.Direction(new DirectionRequestModel
            {
                Direction = (DirectionEnum)Enum.Parse(typeof(DirectionEnum), invalidData)
            }, _stateKeeper) as BadRequestResult;
            Assert.NotNull(badRequestResultDirection);
            Assert.AreEqual(400, badRequestResultDirection.StatusCode);
        }
    }
}
