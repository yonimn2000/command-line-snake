using System.Drawing;

namespace YonatanMankovich.SnakeGameCore
{
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