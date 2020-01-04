using System;
using YonatanMankovich.SimpleConsoleMenus;

namespace YonatanMankovich.CommandLineSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            SnakePlayer.UpdateConsoleTitle();
            StartAgain();
        }

        private static void StartAgain()
        {
            SimpleActionConsoleMenu simpleActionConsoleMenu = new SimpleActionConsoleMenu("Welcome to Yonatan's Snake game!");
            simpleActionConsoleMenu.AddOption("Play", SnakePlayer.PlayGame);
            simpleActionConsoleMenu.AddOption("Auto Play", () => { SnakePlayer.IsAutoSnakePlayer = true; SnakePlayer.PlayGame(); });
            simpleActionConsoleMenu.AddOption("Exit", () => Environment.Exit(0));
            simpleActionConsoleMenu.ShowAndDoAction();
            Console.ReadLine();
        }
    }
}