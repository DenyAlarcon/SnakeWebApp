using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SnakeWebApplication.Controllers;

namespace SnakeWebApplicationNUnitTests.SnakeControllerTests
{
    public class IndexTests
    {
        private SnakeController _snakeController;

        [SetUp]
        public void Setup()
        {
            _snakeController = new SnakeController();
        }

        [Test]
        public void Index_ByDefault_ReturnSnakeIndexView()
        {
            var resultView = _snakeController.Index() as ViewResult;
            Assert.NotNull(resultView);
            Assert.AreEqual("Index", resultView.ViewName);
        }
    }
}
