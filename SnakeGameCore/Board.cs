using System;
using System.Drawing;

namespace YonatanMankovich.SnakeGameCore
{
    public class Board
    {
        public Size Size { get; set; }
        public Snake Snake { get; set; }
        public Point ApplePoint { get; set; }

        private readonly Random random = new Random();

        public Board(Size size)
        {
            Size = size;
            Snake = new Snake(new Point(random.Next(Size.Width), random.Next(Size.Height)), (Direction)random.Next(Enum.GetNames(typeof(Direction)).Length));
            CreateAnApple();
        }

        private void CreateAnApple()
        {
            Point newApplePoint;
            do newApplePoint = new Point(random.Next(Size.Width), random.Next(Size.Height));
            while (Snake.IsPointOnSnake(newApplePoint));
            ApplePoint = newApplePoint;
        }
    }
}