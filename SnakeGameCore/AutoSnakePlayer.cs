namespace YonatanMankovich.SnakeGameCore
{
    public static class AutoSnakePlayer
    {
        public static Directions GetNextDirection(SnakeGameController gameController)
        {
            if (gameController.Snake.GetHead().X < gameController.ApplePoint.X)
                return Directions.Right;
            else if (gameController.Snake.GetHead().X > gameController.ApplePoint.X)
                return Directions.Left;
            else if (gameController.Snake.GetHead().Y < gameController.ApplePoint.Y)
                return Directions.Down;
            else //if (gameController.Snake.GetHead().Y > gameController.ApplePoint.Y)
                return Directions.Up;
        }
    }
}