using System;
using System.Drawing;
using YonatanMankovich.SnakeGameCore;

namespace YonatanMankovich.CommandLineSnake
{
    public static class SnakeConsoleDrawer
    {
        public static void DrawBoard(SnakeGameController gameController, SnakeBoardDiff snakeBoardDiff)
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;

            foreach (SnakeBoardChange snakeBoardChange in snakeBoardDiff.GetSnakeBoardChanges())
            {
                Console.SetCursorPosition(snakeBoardChange.Point.X * 2 + 2, snakeBoardChange.Point.Y + 1);
                switch (snakeBoardChange.SnakeBoardDiff)
                {
                    case SnakeBoardDiffs.AppleRemoved:
                    case SnakeBoardDiffs.SnakeRemoved: Draw(Console.BackgroundColor); break;
                    case SnakeBoardDiffs.SnakeAdded: Draw(ConsoleColor.Yellow); break;
                    case SnakeBoardDiffs.AppleAdded: Draw(ConsoleColor.Red); break;
                }
            }
            Console.SetCursorPosition(0, gameController.BoardSize.Height + 2); // For writing text to the console.
        }

        public static void DrawBorder(Size boardSize)
        {
            Console.SetCursorPosition(0, 0);
            ConsoleColor prevColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.DarkGray;

            for (int x = 0; x < boardSize.Width + 2; x++)
                Console.Write("  ");

            Console.SetCursorPosition(0, boardSize.Height + 1);
            for (int x = 0; x < boardSize.Width + 2; x++)
                Console.Write("  ");

            for (int y = 1; y < boardSize.Height + 1; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("  ");
                Console.SetCursorPosition(boardSize.Width * 2 + 2, y);
                Console.Write("  ");
            }

            Console.BackgroundColor = prevColor;
        }

        private static void Draw(ConsoleColor consoleColor)
        {
            ConsoleColor prevColor = Console.BackgroundColor;
            Console.BackgroundColor = consoleColor;
            Console.Write("  ");
            Console.BackgroundColor = prevColor;
        }
    }
}