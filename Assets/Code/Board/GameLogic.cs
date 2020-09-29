using UnityEngine;

namespace Code.Board
{
  public class GameLogic
  {
    private readonly BoardState _boardState;
    private readonly FallingBlockGenerator _fallingBlockGenerator;
    private readonly BoardCleaner _cleaner;

    public GameLogic(BoardState boardState)
    {
      _boardState = boardState;
      _fallingBlockGenerator = Game.FallingBlockGenerator;
      _cleaner = new BoardCleaner(boardState.Height, boardState.Width);
      Game.BlockFall += HandleFallingBlockPlacement;
    }

    public void Update(float deltaTime)
    {
      if (!_boardState.IsEmpty(_fallingBlockGenerator.StartingX, 0)
          || !_boardState.IsEmpty(_fallingBlockGenerator.StartingX, 1))
      {
        Game.InvokeLevelEnd();
      }
    }

    private void HandleFallingBlockPlacement(Vector2Int staticBlock, Vector2Int rotatingBlock, Block staticCode, Block rotatingCode)
    {
      var staticBlockY = staticBlock.y;
      var rotatingBlockY = rotatingBlock.y;
      while (_boardState.IsEmpty(staticBlock.x, staticBlockY + 1)) staticBlockY++;
      while (_boardState.IsEmpty(rotatingBlock.x, rotatingBlockY + 1)) rotatingBlockY++;

      if (staticBlock.y < rotatingBlock.y) staticBlockY--;
      else if (staticBlock.y > rotatingBlock.y) rotatingBlockY--;

      _boardState.Set(staticBlock.x, staticBlockY, staticCode);
      _boardState.Set(rotatingBlock.x, rotatingBlockY, rotatingCode);

      var cleaningResult = _cleaner.TryClean(_boardState);
      if (cleaningResult.AnythingHappened())
      {
        Game.InvokePoof(cleaningResult);
      }
      else
      {
        Game.InvokeBlockFallResolved();
      }
    }
  }
}