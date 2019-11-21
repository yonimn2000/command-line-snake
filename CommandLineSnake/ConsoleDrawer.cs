using System;
using System.Drawing;
using YonatanMankovich.SnakeGameCore;

namespace YonatanMankovich.CommandLineSnake
{
    public class ConsoleDrawer
    {
        public static void DrawBoard(SnakeGameController gameController)
        {
            Console.CursorVisible = false;
            foreach (SnakeBoardChange snakeBoardChange in gameController.SnakeBoardDiff.GetSnakeBoardChanges())
            {
                Console.SetCursorPosition(snakeBoardChange.Point.X * 2, snakeBoardChange.Point.Y);
                switch (snakeBoardChange.SnakeBoardDiff)
                {
                    case SnakeBoardDiffs.AppleRemoved:
                    case SnakeBoardDiffs.SnakeRemoved:
                        Console.Write(' ');
                        break;
                    case SnakeBoardDiffs.SnakeAdded:
                        Console.Write('X');
                        break;
                    case SnakeBoardDiffs.AppleAdded:
                        Console.Write('O');
                        break;
                }
                Console.Write(' ');
            }
        }
    }
}