using System;
using System.Drawing;
using YonatanMankovich.SnakeGameCore;
using YonatanMankovich.SimpleConsoleMenus;

namespace YonatanMankovich.CommandLineSnake
{
    class Program
    {
        static SnakeGameController gameController;
        static bool isAuto = false;

        static void Main(string[] args)
        {
            StartAgain();
        }

        private static void StartAgain()
        {
            SimpleActionConsoleMenu simpleActionConsoleMenu = new SimpleActionConsoleMenu("Welcome to Yonatan's Snake game!");
            simpleActionConsoleMenu.AddOption("Play", PlayGame);
            simpleActionConsoleMenu.AddOption("Auto Play", () => { isAuto = true; PlayGame(); });
            simpleActionConsoleMenu.AddOption("Exit", () => Environment.Exit(0));
            simpleActionConsoleMenu.ShowAndDoAction();
            Console.ReadLine();
        }

        private static void PlayGame()
        {
            Console.Clear();
            gameController = new SnakeGameController(new Size(30, 20));
            gameController.OnStepMade += GameController_OnStepMade;
            gameController.StartGame();
            while (gameController.IsGameGoing() && !isAuto)
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
                if(isAuto)
                gameController.SetNextSnakeDirection(gameController.GetNextCalculatedDirection());
            }
        }
    }
}