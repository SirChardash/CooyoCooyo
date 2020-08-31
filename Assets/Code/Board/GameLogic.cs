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
    private readonly int _boardWidth;
    private readonly Scoreboard _scoreboard;

    public GameLogic(BoardState boardState, int blockCount, Scoreboard scoreboard)
    {
      BoardState = boardState;
      _boardWidth = boardState.Width;
      _fallingBlockGenerator = new FallingBlockGenerator(blockCount, boardState.Width);
      FallingBlock = _fallingBlockGenerator.Next();
      _cleaner = new BoardCleaner(boardState.Height, boardState.Width);
      _scoreboard = scoreboard;
    }

    public void Update(float deltaTime)
    {
      if (_scoreboard.IsComplete()) End(true);

      for (var x = 0; x < _boardWidth; x++)
      {
        if (!BoardState.IsEmpty(x, 0)) End(false);
      }

      if ((!BoardState.IsEmpty(FallingBlock.StaticBlock.x, FallingBlock.StaticBlock.y + 1)
           || !BoardState.IsEmpty(FallingBlock.RotatingBlock.x, FallingBlock.RotatingBlock.y + 1))
          && FallingBlock.ShouldDrop()) HandleFallingBlockPlacement();
      else HandleFastFall();

      FallingBlock.Update(deltaTime, BoardState);
    }

    private void HandleFastFall()
    {
      if (Input.GetKeyDown(KeyCode.DownArrow)
          && BoardState.IsEmpty(FallingBlock.StaticBlock.x, FallingBlock.StaticBlock.y + 1)
          && BoardState.IsEmpty(FallingBlock.RotatingBlock.x, FallingBlock.RotatingBlock.y + 1))
      {
        FallingBlock.FallFast();
      }

      if (FallingBlock.ShouldDrop()) FallingBlock.DropDown();
    }

    private void HandleFallingBlockPlacement()
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
      if (cleaningResult.AnythingHappened())
      {
        var penalty = _scoreboard.TryContribute(cleaningResult.Poofs);
        var messBlocks = _fallingBlockGenerator.Mess(penalty,
          new BoardState(cleaningResult.BoardStates[cleaningResult.BoardStates.Count - 1]));
        cleaningResult.Penalty = penalty;
        if (penalty > 0) Game.InvokeMessFallEvent(messBlocks);

        if (!_scoreboard.IsComplete())
        {
          var objectiveString = "";

          foreach (var keyValuePair in _scoreboard.GetCurrentObjective().Objectives)
          {
            objectiveString += $"{keyValuePair.Key}:{keyValuePair.Value}#";
          }

          Debug.Log(objectiveString);
        }

        throw new BoardCleaningEvent {CleaningResult = cleaningResult};
      }
    }

    private static void End(bool positive)
    {
      Debug.Log(positive ? "level finished" : "game ended");
      throw new GameEndEvent();
    }
  }
}