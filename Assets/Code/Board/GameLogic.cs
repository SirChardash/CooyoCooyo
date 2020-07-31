using UnityEngine;

namespace Code.Board
{
  public class GameLogic
  {
    public readonly BoardState BoardState;
    private readonly FallingBlockGenerator _fallingBlockGenerator;
    public FallingBlock FallingBlock;
    private readonly BoardCleaner _cleaner;

    public GameLogic()
    {
      BoardState = new BoardState(10, 8);
      _fallingBlockGenerator = new FallingBlockGenerator(4);
      FallingBlock = _fallingBlockGenerator.Next();
      _cleaner = new BoardCleaner(10, 8);
    }

    public void Update(float timeIncrement)
    {
      if (!BoardState.IsEmpty(3, 0)) return; // game over

      if (!BoardState.IsEmpty(FallingBlock.StaticBlock.x, FallingBlock.StaticBlock.y + 1)
          || !BoardState.IsEmpty(FallingBlock.RotatingBlock.x, FallingBlock.RotatingBlock.y + 1))
      {
        var staticBlockY = FallingBlock.StaticBlock.y;
        var rotatingBlockY = FallingBlock.RotatingBlock.y;
        while (BoardState.IsEmpty(FallingBlock.StaticBlock.x, staticBlockY + 1)) staticBlockY++;
        while (BoardState.IsEmpty(FallingBlock.RotatingBlock.x, rotatingBlockY + 1)) rotatingBlockY++;

        if (FallingBlock.StaticBlock.y < FallingBlock.RotatingBlock.y) staticBlockY--;
        else if (FallingBlock.StaticBlock.y > FallingBlock.RotatingBlock.y) rotatingBlockY--;

        BoardState.Set(FallingBlock.StaticBlock.x, staticBlockY, FallingBlock.StaticCode);
        BoardState.Set(FallingBlock.RotatingBlock.x, rotatingBlockY, FallingBlock.RotatingCode);
        var cleaningResult = _cleaner.TryClean(BoardState);
        if (cleaningResult.AnythingHappened())
        {
          BoardState.Set(cleaningResult.BoardStates[cleaningResult.BoardStates.Count - 1]);
        }

        FallingBlock = _fallingBlockGenerator.Next();
      }

      FallingBlock.Update(timeIncrement, BoardState);
    }
  }
}