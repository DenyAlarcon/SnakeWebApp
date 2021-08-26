using SnakeWebApplication.GameManager;

namespace SnakeWebApplication.Models
{
    public class GameBoardResponseModel
    {
        public int TurnNumber { get; set; }
        public int TimeUntilNextTurnMilliseconds { get; set; }
        public GameBoardSize GameBoardSize { get; set; }
    }
}
