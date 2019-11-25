using System.Collections.Generic;
using System.Drawing;

namespace YonatanMankovich.SnakeGameCore
{
    public class SnakeBoardDiff
    {
        private Point lastApplePoint;
        private List<Point> lastSnakePoints;
        private SnakeGameController snakeGameController;

        public SnakeBoardDiff(SnakeGameController snakeGameController)
        {
            this.snakeGameController = snakeGameController;
            Reset();
        }

        public void Reset()
        {
            lastSnakePoints = new List<Point>();
        }

        public void ReadCurrentGameState()
        {
            lastApplePoint = snakeGameController.ApplePoint;
            lastSnakePoints = new List<Point>(snakeGameController.Snake.History);
        }

        public List<SnakeBoardChange> GetSnakeBoardChanges()
        {
            List<SnakeBoardChange> snakeBoardChanges = new List<SnakeBoardChange>();
            
            if (lastSnakePoints.Count > 0) // If not the first frame.
            {
                if (snakeGameController.ApplePoint != lastApplePoint)
                {
                    snakeBoardChanges.Add(new SnakeBoardChange(lastApplePoint, SnakeBoardDiffs.AppleRemoved));
                    snakeBoardChanges.Add(new SnakeBoardChange(snakeGameController.ApplePoint, SnakeBoardDiffs.AppleAdded));
                }
                if (lastSnakePoints[lastSnakePoints.Count - 1] != snakeGameController.Snake.GetHead()) // If head changed.
                    snakeBoardChanges.Add(new SnakeBoardChange(snakeGameController.Snake.GetHead(), SnakeBoardDiffs.SnakeAdded));
                if (lastSnakePoints[0] != snakeGameController.Snake.History[0]) // If tail changed.
                    snakeBoardChanges.Add(new SnakeBoardChange(lastSnakePoints[0], SnakeBoardDiffs.SnakeRemoved)); 
            }
            else
            {
                snakeBoardChanges.Add(new SnakeBoardChange(snakeGameController.ApplePoint, SnakeBoardDiffs.AppleAdded));
                snakeBoardChanges.Add(new SnakeBoardChange(snakeGameController.Snake.GetHead(), SnakeBoardDiffs.SnakeAdded));
            }
            ReadCurrentGameState();
            return snakeBoardChanges;
        }
    }

    public struct SnakeBoardChange
    {
        public Point Point { get; }
        public SnakeBoardDiffs SnakeBoardDiff { get; }

        public SnakeBoardChange(Point point, SnakeBoardDiffs snakeBoardDiff)
        {
            Point = point;
            SnakeBoardDiff = snakeBoardDiff;
        }
    }
}