using System;
using System.Drawing;
using YonatanMankovich.SnakeGameCore;
using System.Threading;

namespace YonatanMankovich.CommandLineSnake
{
    public static class SnakePlayer
    {
        static SnakeGameController gameController;
        static SnakeBoardDiff snakeBoardDiff;
        static AutoSnakePlayer autoSnakePlayer;
        static int maxSnakeSize = 0;
        static int prevAutoSpeed = -1;
        static readonly object drawingLock = new object();
        public static bool IsAutoSnakePlayer { get; set; } = false;

        public static void UpdateConsoleTitle()
        {
            string text = "Yonatan's Command Line Snake Game";
            if (gameController != null)
                text += " | Speed: " + gameController.Speed;
            if (IsAutoSnakePlayer)
                text += " | Auto";
            if (gameController != null && gameController.IsGamePaused)
                text += " | Paused";
            Console.Title = text;
        }

        public static void PlayGame()
        {
            Console.Clear();
            gameController = new SnakeGameController(new Size(Console.BufferWidth / 2 - 2, 20));
            //gameController = new SnakeGameController(new Size(10, 10));
            autoSnakePlayer = new AutoSnakePlayer(gameController);
            snakeBoardDiff = new SnakeBoardDiff(gameController);
            gameController.AfterStepMadeDelegate += GameController_AfterStepMade;
            gameController.BeforeStepMadeDelegate += GameController_BeforeStepMade;
            if (IsAutoSnakePlayer && prevAutoSpeed > 0)
                gameController.Speed = prevAutoSpeed;
            else if (IsAutoSnakePlayer)
                gameController.Speed = 100;
            SnakeConsoleDrawer.DrawBorder(gameController.BoardSize);
            gameController.StartGame();
            UpdateConsoleTitle();
            while (gameController.IsGameGoing())
            {
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.Spacebar)
                {
                    if (!gameController.IsGamePaused)
                        gameController.PauseGame();
                    else
                        gameController.StartGame();
                }
                else if (key == ConsoleKey.A)
                {
                    IsAutoSnakePlayer = !IsAutoSnakePlayer;
                    if (IsAutoSnakePlayer)
                        gameController.Speed = 10;
                }
                else if (IsAutoSnakePlayer)
                {
                    switch (key)
                    {
                        case ConsoleKey.Add: gameController.Speed++; break;
                        case ConsoleKey.Subtract: gameController.Speed--; break;
                    }
                    prevAutoSpeed = gameController.Speed;
                    UpdateConsoleTitle();
                }
                else if (!IsAutoSnakePlayer)
                {
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow: gameController.SetNextSnakeDirection(Directions.Left); break;
                        case ConsoleKey.UpArrow: gameController.SetNextSnakeDirection(Directions.Up); break;
                        case ConsoleKey.RightArrow: gameController.SetNextSnakeDirection(Directions.Right); break;
                        case ConsoleKey.DownArrow: gameController.SetNextSnakeDirection(Directions.Down); break;
                    }
                }
                else
                {
                    Thread.Sleep(10);
                    continue;
                }
                UpdateConsoleTitle();
            }
        }

        private static void GameController_BeforeStepMade()
        {
            if (IsAutoSnakePlayer)
                autoSnakePlayer.SetNextSnakeDirection();
        }

        private static void GameController_AfterStepMade(StepMadeKinds stepMadeKind)
        {
            if (stepMadeKind != StepMadeKinds.Normal && IsAutoSnakePlayer)
                autoSnakePlayer.TryRecalculatingPath();
            if (stepMadeKind == StepMadeKinds.HitWall || stepMadeKind == StepMadeKinds.HitSnake)
                PlayGame();
            else
            {
                lock (drawingLock)
                {
                    maxSnakeSize = Math.Max(maxSnakeSize, gameController.Snake.History.Count);
                    SnakeConsoleDrawer.DrawBoard(gameController, snakeBoardDiff);
                    Console.WriteLine($"Size: {gameController.Snake.History.Count} (Max: {maxSnakeSize} | {Math.Round(100 * (double)maxSnakeSize / (gameController.BoardSize.Width * gameController.BoardSize.Height), 2)}%)");
                }
            }
        }
    }
}