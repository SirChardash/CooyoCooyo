using Code.Events;
using UnityEngine;

namespace Code.Board
{
  public class GameLogic
  {
    public readonly BoardState BoardState;
    private readonly FallingBlockGenerator _fallingBlockGenerator;
    public FallingBlock FallingBlock;
    private readonly BoardCleaner _cleaner;

    public GameLogic(int boardHeight, int boardWidth, int blockCount)
    {
      BoardState = new BoardState(boardHeight, boardWidth);
      _fallingBlockGenerator = new FallingBlockGenerator(blockCount);
      FallingBlock = _fallingBlockGenerator.Next();
      _cleaner = new BoardCleaner(boardHeight, boardWidth);
    }

    public void Update(float timeIncrement)
    {
      if (!BoardState.IsEmpty(3, 0)) return; // game over

      if ((!BoardState.IsEmpty(FallingBlock.StaticBlock.x, FallingBlock.StaticBlock.y + 1)
           || !BoardState.IsEmpty(FallingBlock.RotatingBlock.x, FallingBlock.RotatingBlock.y + 1))
          && FallingBlock.ShouldDrop())
      {
        var staticBlockY = FallingBlock.StaticBlock.y;
        var rotatingBlockY = FallingBlock.RotatingBlock.y;
        while (BoardState.IsEmpty(FallingBlock.StaticBlock.x, staticBlockY + 1)) staticBlockY++;
        while (BoardState.IsEmpty(FallingBlock.RotatingBlock.x, rotatingBlockY + 1)) rotatingBlockY++;

        if (FallingBlock.StaticBlock.y < FallingBlock.RotatingBlock.y) staticBlockY--;
        else if (FallingBlock.StaticBlock.y > FallingBlock.RotatingBlock.y) rotatingBlockY--;

        BoardState.Set(FallingBlock.StaticBlock.x, staticBlockY, FallingBlock.StaticCode);
        BoardState.Set(FallingBlock.RotatingBlock.x, rotatingBlockY, FallingBlock.RotatingCode);
        FallingBlock = _fallingBlockGenerator.Next();

        var cleaningResult = _cleaner.TryClean(BoardState);
        if (cleaningResult.AnythingHappened()) throw new BoardCleaningEvent {CleaningResult = cleaningResult};
      }
      else
      {
        if (Input.GetKeyDown(KeyCode.DownArrow)
            && BoardState.IsEmpty(FallingBlock.StaticBlock.x, FallingBlock.StaticBlock.y + 1)
            && BoardState.IsEmpty(FallingBlock.RotatingBlock.x, FallingBlock.RotatingBlock.y + 1))
        {
          FallingBlock.FallFast();
        }

        if (FallingBlock.ShouldDrop()) FallingBlock.DropDown();
      }

      FallingBlock.Update(timeIncrement, BoardState);
    }
  }
}