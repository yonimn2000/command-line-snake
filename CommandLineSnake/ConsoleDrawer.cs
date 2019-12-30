using System;
using System.Drawing;
using YonatanMankovich.SnakeGameCore;

namespace YonatanMankovich.CommandLineSnake
{
    public class ConsoleDrawer
    {
        public static void DrawBoard(SnakeGameController gameController, Point point, AutoSnakePlayer autoSnakePlayer)
        {
            Console.CursorVisible = false;
            DrawBorder(gameController.BoardSize, point);

            /*foreach (SnakeBoardChange snakeBoardChange in gameController.SnakeBoardDiff.GetSnakeBoardChanges())
            {
                Console.SetCursorPosition(point.X * 2 + snakeBoardChange.Point.X * 2 + 2, point.Y + snakeBoardChange.Point.Y + 1);
                switch (snakeBoardChange.SnakeBoardDiff)
                {
                    case SnakeBoardDiffs.AppleRemoved:
                    case SnakeBoardDiffs.SnakeRemoved: Draw(Console.BackgroundColor); break;
                    case SnakeBoardDiffs.SnakeAdded: Draw(ConsoleColor.Yellow); break;
                    case SnakeBoardDiffs.AppleAdded:
                        Draw(ConsoleColor.Red);
                        break;
                }
            }*/

            //------------Temp
            Console.SetCursorPosition(point.X * 2 + 2, point.Y + 1);
            for (int y = 0; y <= gameController.BoardSize.Height; y++)
            {
                for (int x = 0; x < gameController.BoardSize.Width; x++)
                    Console.Write("  ");
                Console.SetCursorPosition(point.X * 2 + 2, point.Y + 1 + y);
            }
            DrawPathToApple();
            Console.SetCursorPosition(gameController.ApplePoint.X * 2 +2, gameController.ApplePoint.Y+1);
            Draw(ConsoleColor.Red);
            foreach (Point snakePoint in gameController.Snake.History)
            {
                Console.SetCursorPosition(snakePoint.X * 2+2, snakePoint.Y+1);
                Draw(ConsoleColor.Yellow);
            }
            //-------------------

            Console.SetCursorPosition(point.X * 2, point.Y + gameController.BoardSize.Height + 2);

            void DrawPathToApple() //TODO: Remove
            {
                foreach (Point pathPoint in autoSnakePlayer.Path)
                {
                    Console.SetCursorPosition(point.X * 2 + pathPoint.X * 2 + 2, point.Y + pathPoint.Y + 1);
                    Draw(ConsoleColor.Cyan);
                }
                //Console.CursorLeft -= 2;
            }
        }

        public static void DrawBorder(Size boardSize, Point point)
        {
            Point cursorStart = new Point(Console.CursorLeft, Console.CursorTop);
            Console.SetCursorPosition(point.X * 2, point.Y);
            ConsoleColor prevColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.DarkGray;

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