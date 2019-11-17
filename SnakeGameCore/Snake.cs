using System;
using System.Collections.Generic;
using System.Drawing;

namespace YonatanMankovich.SnakeGameCore
{
    public class Snake
    {
        public Point Head { get; set; }
        public Direction Direction { get; set; }
        public Queue<Point> History { get; set; } = new Queue<Point>();

        public Snake(Point head, Direction direction)
        {
            Head = head;
            Direction = direction;
        }

        public bool IsPointOnSnake(Point point)
        {

        }
    }
}