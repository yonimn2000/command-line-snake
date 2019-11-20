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
            for (int y = 0; y < gameController.BoardSize.Height; y++)
            {
                for (int x = 0; x < gameController.BoardSize.Width; x++)
                {
                    Console.Write(' ');
                    if (gameController.Snake.IsPointOnSnake(new Point(x,y)))
                        Console.Write('X');
                    else if (new Point(x, y) == gameController.ApplePoint)
                        Console.Write('O');
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
        }
    }
}