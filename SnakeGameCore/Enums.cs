namespace YonatanMankovich.SnakeGameCore
{
    public enum Directions { Up, Right, Down, Left}
    public enum StepMadeKinds { Normal, HitSnake, HitWall, AteApple }
    public enum SnakeBoardDiffs { NoChange, SnakeRemoved, SnakeAdded, AppleRemoved, AppleAdded }
}