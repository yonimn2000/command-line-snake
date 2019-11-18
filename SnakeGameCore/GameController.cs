using System;
using System.Drawing;
using System.Timers;

namespace YonatanMankovich.SnakeGameCore
{
    public class GameController
    {
        public Size BoardSize { get; set; }
        public Snake Snake { get; private set; }
        public Point ApplePoint { get; private set; }

        private readonly Random random = new Random();
        private readonly Timer timer = new Timer(500);

        public delegate void StepMadeHandler(object sender, StepMadeEventArgs e);
        public event StepMadeHandler OnStepMade;

        public GameController(Size boardSize)
        {
            BoardSize = boardSize;
            InitializeGame();
            timer.Elapsed += TimerTick;
        }

        public void InitializeGame()
        {
            Snake = new Snake(new Point(random.Next(BoardSize.Width), random.Next(BoardSize.Height)),
                            (Directions)random.Next(Enum.GetNames(typeof(Directions)).Length));
            CreateAppleOnBoard();
        }

        public void StartGame()
        {
            timer.Start();
        }

        public void PauseGame()
        {
            timer.Stop();
        }

        private void TimerTick(object source, ElapsedEventArgs e)
        {
            MakeStep();
        }

        public bool IsGameGoing()
        {
            return timer.Enabled;
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
            StepMadeKinds stepMadeKind = StepMadeKinds.Normal;
            if (IsPointOutOfBounds(nextSnakePoint))
            {
                stepMadeKind = StepMadeKinds.HitWall;
                timer.Stop();
            }
            else if (Snake.IsPointOnSnake(nextSnakePoint))
            {
                stepMadeKind = StepMadeKinds.HitSnake;
                timer.Stop();
            }
            Snake.History.Add(nextSnakePoint);
            if (nextSnakePoint == ApplePoint)
            {
                CreateAppleOnBoard();
                stepMadeKind = StepMadeKinds.AteApple;
            }
            else
                Snake.History.RemoveAt(0);
            OnStepMade?.Invoke(this, new StepMadeEventArgs(stepMadeKind)); // FIXME
        }

        private bool IsPointOutOfBounds(Point point)
        {
            return point.X < 0 || point.Y < 0 || point.X >= BoardSize.Width || point.Y >= BoardSize.Height;
        }
    }

    public class StepMadeEventArgs : EventArgs
    {
        public StepMadeKinds StepMadeKind { get; }
        public StepMadeEventArgs(StepMadeKinds stepMadeKind)
        {
            StepMadeKind = stepMadeKind;
        }
    }
}