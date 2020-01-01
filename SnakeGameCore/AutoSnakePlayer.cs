using System;
using System.Collections.Generic;
using System.Drawing;
using YonatanMankovich.PathStar;

namespace YonatanMankovich.SnakeGameCore
{
    public class AutoSnakePlayer
    {
        public SnakeGameController SnakeGameController { get; }
        public List<Point> Path { get; private set; } = new List<Point>();

        public AutoSnakePlayer(SnakeGameController snakeGameController)
        {
            SnakeGameController = snakeGameController;
            TryRecalculatingPath();
        }

        public void SetNextSnakeDirection()
        {
            SnakeGameController.SetNextSnakeDirection(GetNextDirection());
        }

        public void TryRecalculatingPath()
        {
            Path.Clear();
            try
            {
                IGridAstar PathFinder = new GridAstar(SnakeGameController.BoardSize, SnakeGameController.Snake.GetHead(),
                SnakeGameController.ApplePoint, SnakeGameController.Snake.History);
                PathFinder.FindPath();
                Path = PathFinder.Path;
            }
            catch (Exception e) when (e is PathNotFoundException || e is PointOutsideOfGridException) { } // Ignore exceptions
        }

        public Directions GetNextDirection()
        {
            TryRecalculatingPath();
            if (Path.Count > 0)
            {
                Point nextPoint = Path[1];
                if (SnakeGameController.Snake.GetHead().X < nextPoint.X)
                    return Directions.Right;
                else if (SnakeGameController.Snake.GetHead().X > nextPoint.X)
                    return Directions.Left;
                else if (SnakeGameController.Snake.GetHead().Y < nextPoint.Y)
                    return Directions.Down;
                else if (SnakeGameController.Snake.GetHead().Y > nextPoint.Y)
                    return Directions.Up;
            }
            return GetNextRandomPossibleDirection(); // If could not calculate path...
        }

        public Directions GetNextRandomPossibleDirection()
        {
            List<Directions> nextPossibleDirections = new List<Directions>(4);
            Point snakeHead = SnakeGameController.Snake.GetHead();

            if (IsPossibleNextPoint(new Point(snakeHead.X, snakeHead.Y - 1)))
                nextPossibleDirections.Add(Directions.Up);
            if (IsPossibleNextPoint(new Point(snakeHead.X, snakeHead.Y + 1)))
                nextPossibleDirections.Add(Directions.Down);
            if (IsPossibleNextPoint(new Point(snakeHead.X + 1, snakeHead.Y)))
                nextPossibleDirections.Add(Directions.Right);
            if (IsPossibleNextPoint(new Point(snakeHead.X - 1, snakeHead.Y)))
                nextPossibleDirections.Add(Directions.Left);

            if (nextPossibleDirections.Count > 0)
                return nextPossibleDirections[new Random().Next(nextPossibleDirections.Count)];
            return SnakeGameController.Snake.Direction; // Continue in the same direction (give up).
        }

        private bool IsPossibleNextPoint(Point point)
        {
            return !(SnakeGameController.IsPointOutOfBounds(point) || SnakeGameController.Snake.IsPointOnSnake(point));
        }
    }
}