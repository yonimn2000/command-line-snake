﻿using System;
using System.Drawing;
using System.Timers;

namespace YonatanMankovich.SnakeGameCore
{
    public class SnakeGameController
    {
        public Size BoardSize { get; set; }
        public Snake Snake { get; private set; }
        public Point ApplePoint { get; private set; }
        public SnakeBoardDiff SnakeBoardDiff { get; }
        public AutoSnakePlayer AutoSnakePlayer { get; }


        private readonly Random random = new Random();
        private readonly Timer timer = new Timer(10);
        private Directions nextSnakeDirection;

        public delegate void StepMadeHandler(object sender, StepMadeEventArgs e);
        public event StepMadeHandler OnStepMade;

        public SnakeGameController(Size boardSize)
        {
            BoardSize = boardSize;
            Snake = new Snake(new Point(random.Next(BoardSize.Width), random.Next(BoardSize.Height)),
                            (Directions)random.Next(Enum.GetNames(typeof(Directions)).Length));
            nextSnakeDirection = Snake.Direction;
            SnakeBoardDiff = new SnakeBoardDiff(this);
            AutoSnakePlayer = new AutoSnakePlayer(this);
            CreateAppleOnBoard();
            
            timer.Elapsed += TimerTick;
        }

        public void InitializeGame()
        {
            Snake = new Snake(new Point(random.Next(BoardSize.Width), random.Next(BoardSize.Height)),
                            (Directions)random.Next(Enum.GetNames(typeof(Directions)).Length));
            nextSnakeDirection = Snake.Direction;
            CreateAppleOnBoard();
            //SnakeBoardDiff.ReadCurrentGameState();
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

        public Directions GetNextCalculatedDirection()
        {
            return AutoSnakePlayer.GetNextDirection();
        }

        private void CreateAppleOnBoard()
        {
            Point newApplePoint;
            do newApplePoint = new Point(random.Next(BoardSize.Width-2)+1, random.Next(BoardSize.Height-2)+1);//TODO Remove +-12
            while (Snake.IsPointOnSnake(newApplePoint));
            ApplePoint = newApplePoint;
            AutoSnakePlayer.ReCalculatePath();
        }

        public void SetNextSnakeDirection(Directions direction)
        {
            if (direction != nextSnakeDirection && ((int)direction - (int)nextSnakeDirection) % 2 != 0) // New direction is not same and not opposite of current.
                nextSnakeDirection = direction;
        }

        private void MakeStep()
        {
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
            OnStepMade?.Invoke(this, new StepMadeEventArgs(stepMadeKind)); // FIXME
        }

        public void EndGame()
        {
            timer.Stop();
            SnakeBoardDiff.Reset();
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