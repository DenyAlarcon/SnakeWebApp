namespace SnakeWebApplication.GameManager
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void ToLeft()
        {
            X -= 1;
        }

        public void ToRight()
        {
            X += 1;
        }

        public void ToTop()
        {
            Y -= 1;
        }

        public void ToBottom()
        {
            Y += 1;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator <(Point p1, int comparableValue)
        {
            return p1.X < comparableValue || p1.Y < comparableValue;
        }

        public static bool operator >(Point p1, int comparableValue)
        {
            return p1.X > comparableValue || p1.Y > comparableValue;
        }
    }
}