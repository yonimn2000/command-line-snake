using System;
using System.Drawing;
using YonatanMankovich.SnakeGameCore;

namespace YonatanMankovich.CommandLineSnake
{
    public class ConsoleDrawer
    {
        public static void DrawBoard(SnakeGameController gameController, Point point)
        {
            Console.CursorVisible = false;
            DrawBorder(gameController.BoardSize, point);
            foreach (SnakeBoardChange snakeBoardChange in gameController.SnakeBoardDiff.GetSnakeBoardChanges())
            {
                Console.SetCursorPosition(point.X * 2 + snakeBoardChange.Point.X * 2 + 2, point.Y + snakeBoardChange.Point.Y + 1);
                switch (snakeBoardChange.SnakeBoardDiff)
                {
                    case SnakeBoardDiffs.AppleRemoved:
                    case SnakeBoardDiffs.SnakeRemoved: Draw(Console.BackgroundColor); break;
                    case SnakeBoardDiffs.SnakeAdded: Draw(ConsoleColor.Yellow); break;
                    case SnakeBoardDiffs.AppleAdded:
                        //DrawPathToApple();
                        Draw(ConsoleColor.Red);
                        break;
                }
            }
            Console.SetCursorPosition(point.X * 2, point.Y + gameController.BoardSize.Height +2);

            void DrawPathToApple() //TODO: Remove
            {
                foreach (Point pathPoint in gameController.AutoSnakePlayer.Path)
                {
                    Console.SetCursorPosition(point.X * 2 + pathPoint.X * 2 + 2, point.Y + pathPoint.Y + 1);
                    Draw(ConsoleColor.Cyan);
                }
                Console.CursorLeft -= 2;
            }
        }

        public static void DrawBorder(Size boardSize, Point point)
        {
            Point cursorStart = new Point(Console.CursorLeft, Console.CursorTop);
            Console.SetCursorPosition(point.X * 2, point.Y);
            ConsoleColor prevColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Gray;

            for (int x = point.X; x < point.X + boardSize.Width + 2; x++)
                Console.Write("  ");

            Console.SetCursorPosition(point.X * 2, point.Y + boardSize.Height + 1);
            for (int x = point.X; x < point.X + boardSize.Width + 2; x++)
                Console.Write("  ");

            for (int y = point.Y + 1; y < point.Y + boardSize.Height + 1; y++)
            {
                Console.SetCursorPosition(point.X * 2, y);
                Console.Write("  ");
                Console.SetCursorPosition(point.X * 2 + boardSize.Width * 2 + 2, y);
                Console.Write("  ");
            }

            Console.BackgroundColor = prevColor;
            Console.SetCursorPosition(cursorStart.X, cursorStart.Y);
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