using System;
using System.Drawing;
using YonatanMankovich.SnakeGameCore;
using YonatanMankovich.SimpleConsoleMenus;

namespace YonatanMankovich.CommandLineSnake
{
    class Program
    {
        static SnakeGameController gameController;
        static AutoSnakePlayer autoSnakePlayer;
        static bool isAuto = false;

        static void Main(string[] args)
        {
            Console.Title = "Yonatan's Command Line Snake Game";
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
            gameController = new SnakeGameController(new Size(10, 10));
            autoSnakePlayer = new AutoSnakePlayer(gameController);
            gameController.OnStepMade += GameController_AfterStepMade;
            gameController.BeforeStepMadeDelegate += GameController_BeforeStepMade;
            gameController.StartGame();
            //ConsoleDrawer.DrawBoard(gameController, new Point(0, 0), autoSnakePlayer);
            while (gameController.IsGameGoing() && !isAuto)
            {
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.Spacebar:
                        if (!gameController.IsGamePaused)
                            gameController.PauseGame();
                        else
                            gameController.StartGame();
                        break;
                    case ConsoleKey.LeftArrow: gameController.SetNextSnakeDirection(Directions.Left); break;
                    case ConsoleKey.UpArrow: gameController.SetNextSnakeDirection(Directions.Up); break;
                    case ConsoleKey.RightArrow: gameController.SetNextSnakeDirection(Directions.Right); break;
                    case ConsoleKey.DownArrow: gameController.SetNextSnakeDirection(Directions.Down); break;
                }
            }
        }

        private static void GameController_BeforeStepMade()
        {
            if (isAuto)
                gameController.SetNextSnakeDirection(autoSnakePlayer.GetNextDirection());
        }

        private static void GameController_AfterStepMade(StepMadeKinds stepMadeKind)
        {
            if (stepMadeKind != StepMadeKinds.Normal)
                autoSnakePlayer.TryRecalculatingPath();
            if (stepMadeKind == StepMadeKinds.HitWall || stepMadeKind == StepMadeKinds.HitSnake)
            {
                gameController.InitializeGame();
                gameController.StartGame();
                Console.Clear();
            }
            else
            {
                ConsoleDrawer.DrawBoard(gameController, new Point(0, 0), autoSnakePlayer);
                Console.WriteLine("Size: " + gameController.Snake.History.Count);
            }
        }
    }
}