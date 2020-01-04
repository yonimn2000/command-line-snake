using System.Collections.Generic;
using System.Drawing;

namespace YonatanMankovich.SnakeGameCore
{
    public class Snake
    {
        public Directions Direction { get; internal set; }

        public List<Point> History { get; } = new List<Point>();

        public Snake(Point head, Directions direction)
        {
            History.Add(head);
            Direction = direction;
        }

        public Point GetNextPoint()
        {
            Point nextPoint;
            Point head = GetHead();
            switch (Direction)
            {
                case Directions.Up: nextPoint = new Point(head.X, head.Y - 1); break;
                case Directions.Down: nextPoint = new Point(head.X, head.Y + 1); break;
                case Directions.Left: nextPoint = new Point(head.X - 1, head.Y); break;
                case Directions.Right: nextPoint = new Point(head.X + 1, head.Y); break;
            }
            return nextPoint;
        }

        public Point GetHead()
        {
            return new Point(History[History.Count - 1].X, History[History.Count - 1].Y);
        }

        public bool IsPointOnSnake(Point point)
        {
            foreach (Point snakePoint in History.ToArray())
                if (snakePoint == point)
                    return true;
            return false;
        }
    }
}