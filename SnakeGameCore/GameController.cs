using System;
using System.Drawing;
using System.Timers;

namespace YonatanMankovich.SnakeGameCore
{
    public class GameController
    {
        public Size BoardSize { get; set; }
        public Snake Snake { get; }
        public Point ApplePoint { get; private set; }

        private readonly Random random = new Random();
        private readonly Timer timer = new Timer(500);

        public GameController(Size boardSize)
        {
            BoardSize = boardSize;
            Snake = new Snake(new Point(random.Next(BoardSize.Width), random.Next(BoardSize.Height)),
                (Direction)random.Next(Enum.GetNames(typeof(Direction)).Length));
            CreateAppleOnBoard();
            timer.Elapsed += TimerTick;
        }

        public void StartGame()
        {
            timer.Start();
        }

        private void TimerTick(object source, ElapsedEventArgs e)
        {
            MakeStep();
        }

        private void CreateAppleOnBoard()
        {
            Point newApplePoint;
            do newApplePoint = new Point(random.Next(BoardSize.Width), random.Next(BoardSize.Height));
            while (Snake.IsPointOnSnake(newApplePoint));
            ApplePoint = newApplePoint;
        }

        private void MakeStep()
        {
            Point nextSnakePoint = Snake.GetNextPoint();
            Snake.History.Add(nextSnakePoint);
            if (nextSnakePoint == ApplePoint)
                CreateAppleOnBoard();
            else
                Snake.History.RemoveAt(0);
        }
    }
}