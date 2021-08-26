using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeWebApplication.GameManager
{
    public enum DirectionEnum { None, Left, Right, Top, Bottom }

    public class StateKeeper
    {
        private readonly Timer _timer;
        public int TurnNumber { get; private set; }
        public int TimeUntilNextTurnMilliseconds { get; }
        public GameBoardSize GameBoardSize { get; }
        public IEnumerable<Point> Snake { get; private set; }
        public IEnumerable<Point> Food { get; private set; }
        public string Direction { get; set; }

        public StateKeeper(int timeUntilNextTurnMilliseconds, int gameBoardWidth, int gameBoardHeight)
        {
            TimeUntilNextTurnMilliseconds = timeUntilNextTurnMilliseconds;
            GameBoardSize = new GameBoardSize(gameBoardWidth, gameBoardHeight);
            Snake = new List<Point>
            {
                new Point(gameBoardWidth / 2, gameBoardHeight / 2),
                new Point(gameBoardWidth / 2, gameBoardHeight / 2 + 1)
            };
            Food = new List<Point> { new Point(gameBoardWidth / 2, gameBoardHeight / 4) };
            Direction = nameof(DirectionEnum.Top);

            _timer = new Timer(MoveSnake, null, TimeUntilNextTurnMilliseconds, TimeUntilNextTurnMilliseconds);
        }

        // calculate new snake coordinates
        public void MoveSnake(object state)
        {
            // check null reference
            if (Food == null || Snake == null || GameBoardSize == null) return;

            // get old head position
            var oldHeadPosition = new Point(Snake.First());

            // calculate new head position
            switch (Direction)
            {
                case nameof(DirectionEnum.Left):
                    oldHeadPosition.ToLeft();
                    break;
                case nameof(DirectionEnum.Top):
                    oldHeadPosition.ToTop();
                    break;
                case nameof(DirectionEnum.Right):
                    oldHeadPosition.ToRight();
                    break;
                case nameof(DirectionEnum.Bottom):
                    oldHeadPosition.ToBottom();
                    break;
            }

            var newHeadPosition = new Point(oldHeadPosition);

            // if snake ate food, create new food, else remove tail
            int i;
            for (i = 0; i < Food.Count(); i++)
            {
                // if snake ate food, remove eaten food, create new and break loop
                if (newHeadPosition != Food.ElementAt(i)) continue;
                UpdateFood(i);
                break;
            }
            // if snake didn't eat food, remove tail 
            if (i == Food.Count()) Snake = Snake.Take(Snake.Count() - 1);
          
            // check board and body collision
            if (newHeadPosition < 0 || newHeadPosition > (GameBoardSize.Width - 1) ||
                IsBodyCollision(newHeadPosition, Snake))
            {
                // if collision, reset game state
                ResetGameState();
            }
            else
            {
                // if no collision, add new head
                Snake = Snake.Prepend(newHeadPosition);
                TurnNumber++;
            }
        }

        // remove eaten food and create new
        private void UpdateFood(int indexOfFoodToRemove)
        {
            var random = new Random();

            var firstPart = Food.Take(indexOfFoodToRemove);
            var secondPart = Food.Skip(indexOfFoodToRemove + 1);
            var newFoodCoordinate = new List<Point>
            {
                new Point(random.Next(0, GameBoardSize.Width), random.Next(0, GameBoardSize.Height))
            };
            Food = firstPart.Concat(newFoodCoordinate);
            Food = Food.Concat(secondPart);
        }

        // check body collision
        private bool IsBodyCollision(Point headPosition, IEnumerable<Point> snakeBody)
        {
            return snakeBody.Any(snakePart => snakePart == headPosition);
        }

        // reset game state
        private void ResetGameState()
        {
            TurnNumber = 0;
            Snake = new List<Point>
            {
                new Point(GameBoardSize.Width / 2, GameBoardSize.Height / 2),
                new Point(GameBoardSize.Width / 2, GameBoardSize.Height / 2 + 1)
            };
            Food = new List<Point> { new Point(10, 5) };
            Direction = nameof(DirectionEnum.Top);
        }

        public void SetDirection(DirectionEnum direction)
        {
            if (direction == DirectionEnum.None)
            {
                throw new ArgumentNullException();
            }
            Direction = direction.ToString();
        }
    }
}
