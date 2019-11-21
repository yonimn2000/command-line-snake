using System;
using System.Drawing;
using YonatanMankovich.SnakeGameCore;

namespace YonatanMankovich.CommandLineSnake
{
    class Program
    {
        static SnakeGameController gameController;

        static void Main(string[] args)
        {
            gameController = new SnakeGameController(new Size(50, 50));
            gameController.OnStepMade += GameController_OnStepMade;
            gameController.StartGame();
            while (gameController.IsGameGoing())
            {
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.Spacebar: gameController.PauseGame(); break; // TODO
                    case ConsoleKey.LeftArrow: gameController.SetNextSnakeDirection(Directions.Left); break;
                    case ConsoleKey.UpArrow: gameController.SetNextSnakeDirection(Directions.Up); break;
                    case ConsoleKey.RightArrow: gameController.SetNextSnakeDirection(Directions.Right); break;
                    case ConsoleKey.DownArrow: gameController.SetNextSnakeDirection(Directions.Down); break;
                }
            }
            Console.ReadLine();
        }

        private static void GameController_OnStepMade(object sender, StepMadeEventArgs e)
        {
            if (e.StepMadeKind == StepMadeKinds.HitWall || e.StepMadeKind == StepMadeKinds.HitSnake)
            {
                gameController.InitializeGame();
                gameController.StartGame();
                Console.Clear();
            }
            else
            {
                ConsoleDrawer.DrawBoard(gameController);
                //Console.WriteLine(e.StepMadeKind);
                gameController.SetNextSnakeDirection(AutoSnakePlayer.GetNextDirection(gameController));
            }
        }
    }
}