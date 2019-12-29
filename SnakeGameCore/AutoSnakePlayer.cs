using System;
using System.Collections.Generic;
using System.Drawing;
using YonatanMankovich.PathStar;

namespace YonatanMankovich.SnakeGameCore
{
    public class AutoSnakePlayer
    {
        public SnakeGameController SnakeGameController { get; private set; }
        public Queue<Point> Path { get; private set; } = new Queue<Point>();
        private IGridAstar PathFinder { get; set; }

        public AutoSnakePlayer(SnakeGameController snakeGameController)
        {
            SnakeGameController = snakeGameController;
        }

        public void TryRecalculatingPath()
        {
            Point nextSnakePoint = SnakeGameController.Snake.GetNextPoint();
            if (IsPossibleNextPoint(nextSnakePoint))
            {
                PathFinder = new GridAstar(SnakeGameController.BoardSize, nextSnakePoint,
                SnakeGameController.ApplePoint, SnakeGameController.Snake.History);
                PathFinder.FindPath();
                Path = new Queue<Point>(PathFinder.Path);
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
                return SnakeGameController.Snake.Direction; // Continue in the same direction.
            }
            TryRecalculatingPath();
            return GetNextRandomPossibleDirection(); // If could not calculate path...
        }

        private Directions GetNextRandomPossibleDirection()
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