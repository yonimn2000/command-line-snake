using System.Collections.Generic;
using System.Drawing;
using YonatanMankovich.PathStar;

namespace YonatanMankovich.SnakeGameCore
{
    public class AutoSnakePlayer
    {
        public SnakeGameController SnakeGameController { get; private set; }
        public Queue<Point> Path { get; private set; }
        private GridAstar PathFinder { get; set; }

        public AutoSnakePlayer(SnakeGameController snakeGameController)
        {
            SnakeGameController = snakeGameController;
        }

        public void ReCalculatePath()
        {
            try
            {
                PathFinder = new GridAstar(SnakeGameController.BoardSize, SnakeGameController.Snake.GetNextPoint(),
                SnakeGameController.ApplePoint, SnakeGameController.Snake.History);
                PathFinder.FindPath();
                PathFinder.Path.Reverse();
                Path = new Queue<Point>(PathFinder.Path);
            }
            catch (System.Exception)
            {
                //No solution or other
            }
        }

        public Directions GetNextDirection()
        {
            if (Path.Count > 0)
            {
                Point nextPoint = Path.Dequeue();
                if (SnakeGameController.Snake.GetHead().X < nextPoint.X)
                    return Directions.Right;
                else if (SnakeGameController.Snake.GetHead().X > nextPoint.X)
                    return Directions.Left;
                else if (SnakeGameController.Snake.GetHead().Y < nextPoint.Y)
                    return Directions.Down;
                else if (SnakeGameController.Snake.GetHead().Y > nextPoint.Y)
                    return Directions.Up;
            }
            return SnakeGameController.Snake.Direction;
        }
    }
}