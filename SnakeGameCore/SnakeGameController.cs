using System;
using System.Drawing;
using System.Timers;

namespace YonatanMankovich.SnakeGameCore
{
    public class SnakeGameController
    {
        public Size BoardSize { get; set; }
        public Snake Snake { get; private set; }
        public Point ApplePoint { get; private set; }
        public bool IsGamePaused { get; private set; } = false;
        public int Speed
        {
            get => (int)Math.Round(1000 / timer.Interval);
            set
            {
                double newInterval = 1000.0 / value;
                if (newInterval < 10)
                    newInterval = 10;
                if (newInterval > 1000)
                    newInterval = 1000;
                timer.Interval = newInterval;
            }
        }

        private readonly Random random = new Random();
        private readonly Timer timer = new Timer(100);
        private Directions nextSnakeDirection;

        public BeforeStepMade BeforeStepMadeDelegate;
        public AfterStepMade OnStepMade;

        public delegate void BeforeStepMade();
        public delegate void AfterStepMade(StepMadeKinds stepMadeKind);

        public SnakeGameController(Size boardSize)
        {
            BoardSize = boardSize;
            Snake = new Snake(new Point(random.Next(BoardSize.Width), random.Next(BoardSize.Height)),
                            (Directions)random.Next(Enum.GetNames(typeof(Directions)).Length));
            nextSnakeDirection = Snake.Direction;
            CreateAppleOnBoard();
            timer.Elapsed += TimerTick;
        }

        public void InitializeGame()
        {
            Snake = new Snake(new Point(random.Next(BoardSize.Width), random.Next(BoardSize.Height)),
                            (Directions)random.Next(Enum.GetNames(typeof(Directions)).Length));
            nextSnakeDirection = Snake.Direction;
            CreateAppleOnBoard();
        }

        public void StartGame()
        {
            timer.Start();
            IsGamePaused = false;
        }

        public void PauseGame()
        {
            timer.Stop();
            IsGamePaused = true;
        }

        private void TimerTick(object source, ElapsedEventArgs e)
        {
            MakeStep();
        }

        public bool IsGameGoing()
        {
            return timer.Enabled || IsGamePaused;
        }

        private void CreateAppleOnBoard()
        {
            Point newApplePoint;
            do newApplePoint = new Point(random.Next(BoardSize.Width), random.Next(BoardSize.Height));
            while (Snake.IsPointOnSnake(newApplePoint));
            ApplePoint = newApplePoint;
        }

        public void SetNextSnakeDirection(Directions direction)
        {
            // New direction is not same and not opposite of current unless snake is of size one.
            if (direction != nextSnakeDirection && (((int)direction - (int)nextSnakeDirection) % 2 != 0 || Snake.History.Count == 1))
                nextSnakeDirection = direction;
        }

        private void MakeStep()
        {
            BeforeStepMadeDelegate?.Invoke();
            Snake.Direction = nextSnakeDirection;
            Point nextSnakePoint = Snake.GetNextPoint();
            StepMadeKinds stepMadeKind = StepMadeKinds.Normal;
            if (IsPointOutOfBounds(nextSnakePoint))
            {
                stepMadeKind = StepMadeKinds.HitWall;
                EndGame();
            }
            else if (Snake.IsPointOnSnake(nextSnakePoint))
            {
                stepMadeKind = StepMadeKinds.HitSnake;
                EndGame();
            }
            Snake.History.Add(nextSnakePoint);
            if (nextSnakePoint == ApplePoint)
            {
                CreateAppleOnBoard();
                stepMadeKind = StepMadeKinds.AteApple;
            }
            else
                Snake.History.RemoveAt(0);
            OnStepMade?.Invoke(stepMadeKind);
        }

        public void EndGame()
        {
            timer.Stop();
        }

        public bool IsPointOutOfBounds(Point point)
        {
            return point.X < 0 || point.Y < 0 || point.X >= BoardSize.Width || point.Y >= BoardSize.Height;
        }
    }
}