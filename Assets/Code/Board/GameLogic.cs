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
      BoardState = new BoardState();
      _fallingBlockGenerator = new FallingBlockGenerator();
      FallingBlock = _fallingBlockGenerator.Next();
      _cleaner = new BoardCleaner();
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
        BoardState.Set(_cleaner.TryClean(BoardState, FallingBlock.StaticBlock.x, staticBlockY).BoardResult);
        BoardState.Set(_cleaner.TryClean(BoardState, FallingBlock.RotatingBlock.x, rotatingBlockY).BoardResult);
        FallingBlock = _fallingBlockGenerator.Next();
      }

      FallingBlock.Update(timeIncrement, BoardState);
    }
  }
}