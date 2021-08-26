using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SnakeWebApplication.GameManager;
using SnakeWebApplication.Models;

namespace SnakeWebApplication.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class SnakeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet] public IActionResult OrganizersSnake()
        {
            return View("OrganizersSnake");
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GameBoard([FromServices] StateKeeper engine)
        {
            return Ok(new GameBoardResponseModel
                {
                    TurnNumber = engine.TurnNumber,
                    TimeUntilNextTurnMilliseconds = engine.TimeUntilNextTurnMilliseconds,
                    GameBoardSize = new GameBoardSize(
                        engine.GameBoardSize.Width,
                        engine.GameBoardSize.Height)
                }
            );
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Point>), 200)]
        public IActionResult Snake([FromServices] StateKeeper engine)
        {
            return Ok(engine.Snake);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Point>), 200)]
        public IActionResult Food([FromServices] StateKeeper engine)
        {
            return Ok(engine.Food);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Direction([FromBody] DirectionRequestModel request, [FromServices] StateKeeper engine)
        {
            try
            {
                engine.SetDirection(request.Direction);
                return Ok();
            }
            catch (Exception exception)
            {
                if (exception is ArgumentNullException) return BadRequest();
                throw;
            }
        }
    }
}
