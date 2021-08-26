namespace SnakeWebApplication.GameManager
{
    public class GameBoardSize
    {
        private int _width;
        private int _height;

        public GameBoardSize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value > 5)
                {
                    _width = value;
                }
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value > 5)
                {
                    _height = value;
                }
            }
        }
    }
}
