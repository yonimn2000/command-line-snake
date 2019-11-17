using System;
using System.Drawing;
using YonatanMankovich.SnakeGameCore;

namespace YonatanMankovich.CommandLineSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            GameController gameController = new GameController(new Size(10, 10));
            gameController.StartGame();
            ConsoleDrawer.DrawBoard(gameController);
            Console.ReadLine();
        }
    }
}