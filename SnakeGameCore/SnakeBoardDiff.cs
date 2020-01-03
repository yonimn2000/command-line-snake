using System.Collections.Generic;
using System.Drawing;

namespace YonatanMankovich.SnakeGameCore
{
    public class SnakeBoardDiff
    {
        private SnakeGameController SnakeGameController { get; }
        private Point LastApplePoint { get; set; }
        private List<Point> LastSnakePoints { get; set; }

        public SnakeBoardDiff(SnakeGameController snakeGameController)
        {
            SnakeGameController = snakeGameController;
            Reset();
        }

        public void Reset()
        {
            LastApplePoint = new Point(-1, -1);
            LastSnakePoints = new List<Point>();
        }

        public void ReadCurrentGameState()
        {
            LastApplePoint = SnakeGameController.ApplePoint;
            LastSnakePoints = new List<Point>(SnakeGameController.Snake.History);
        }

        public List<SnakeBoardChange> GetSnakeBoardChanges()
        {
            List<SnakeBoardChange> snakeBoardChanges = new List<SnakeBoardChange>();

            if (LastSnakePoints.Count > 0) // If not the first frame.
            {
                if (SnakeGameController.ApplePoint != LastApplePoint)
                {
                    snakeBoardChanges.Add(new SnakeBoardChange(LastApplePoint, SnakeBoardDiffs.AppleRemoved));
                    snakeBoardChanges.Add(new SnakeBoardChange(SnakeGameController.ApplePoint, SnakeBoardDiffs.AppleAdded));
                }
                for (int i = 0; i < LastSnakePoints.Count; i++)
                {
                    Point snakePoint = LastSnakePoints[i];
                    if (!SnakeGameController.Snake.History.Contains(snakePoint))
                        snakeBoardChanges.Add(new SnakeBoardChange(snakePoint, SnakeBoardDiffs.SnakeRemoved));
                }

                for (int i = 0; i < SnakeGameController.Snake.History.Count; i++)
                {
                    Point snakePoint = SnakeGameController.Snake.History[i];
                    if (!LastSnakePoints.Contains(snakePoint))
                        snakeBoardChanges.Add(new SnakeBoardChange(snakePoint, SnakeBoardDiffs.SnakeAdded));
                }
            }
            else
            {
                snakeBoardChanges.Add(new SnakeBoardChange(SnakeGameController.ApplePoint, SnakeBoardDiffs.AppleAdded));
                snakeBoardChanges.Add(new SnakeBoardChange(SnakeGameController.Snake.GetHead(), SnakeBoardDiffs.SnakeAdded));
            }
            ReadCurrentGameState();
            return snakeBoardChanges;
        }
    }
}