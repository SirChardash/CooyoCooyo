using Code.Events;
using UnityEngine;

namespace Code.Board
{
  public class GameLogic
  {
    private readonly BoardState _boardState;
    private readonly FallingBlockGenerator _fallingBlockGenerator;
    public FallingBlock FallingBlock;
    private readonly BoardCleaner _cleaner;

    public GameLogic(BoardState boardState)
    {
      _boardState = boardState;
      _fallingBlockGenerator = Game.FallingBlockGenerator;
      FallingBlock = _fallingBlockGenerator.Next();
      _cleaner = new BoardCleaner(boardState.Height, boardState.Width);
    }

    public void Update(float deltaTime)
    {
      if (!_boardState.IsEmpty(_fallingBlockGenerator.StartingX, 0)
          || !_boardState.IsEmpty(_fallingBlockGenerator.StartingX, 1))
      {
        Game.InvokeLevelEnd();
      }

      if ((!_boardState.IsEmpty(FallingBlock.StaticBlock.x, FallingBlock.StaticBlock.y + 1)
           || !_boardState.IsEmpty(FallingBlock.RotatingBlock.x, FallingBlock.RotatingBlock.y + 1))
          && FallingBlock.ShouldDrop()) HandleFallingBlockPlacement();
      else HandleFastFall();

      FallingBlock.Update(deltaTime, _boardState);
    }

    private void HandleFastFall()
    {
      if (Input.GetKeyDown(KeyCode.DownArrow)
          && _boardState.IsEmpty(FallingBlock.StaticBlock.x, FallingBlock.StaticBlock.y + 1)
          && _boardState.IsEmpty(FallingBlock.RotatingBlock.x, FallingBlock.RotatingBlock.y + 1))
      {
        FallingBlock.FallFast();
      }

      if (FallingBlock.ShouldDrop()) FallingBlock.DropDown();
    }

    private void HandleFallingBlockPlacement()
    {
      var staticBlockY = FallingBlock.StaticBlock.y;
      var rotatingBlockY = FallingBlock.RotatingBlock.y;
      while (_boardState.IsEmpty(FallingBlock.StaticBlock.x, staticBlockY + 1)) staticBlockY++;
      while (_boardState.IsEmpty(FallingBlock.RotatingBlock.x, rotatingBlockY + 1)) rotatingBlockY++;

      if (FallingBlock.StaticBlock.y < FallingBlock.RotatingBlock.y) staticBlockY--;
      else if (FallingBlock.StaticBlock.y > FallingBlock.RotatingBlock.y) rotatingBlockY--;

      _boardState.Set(FallingBlock.StaticBlock.x, staticBlockY, FallingBlock.StaticCode);
      _boardState.Set(FallingBlock.RotatingBlock.x, rotatingBlockY, FallingBlock.RotatingCode);
      FallingBlock = _fallingBlockGenerator.Next();

      var cleaningResult = _cleaner.TryClean(_boardState);
      if (cleaningResult.AnythingHappened())
      {
        throw new BoardCleaningEvent {CleaningResult = cleaningResult};
      }
    }
  }
}