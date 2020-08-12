using System.Collections.Generic;
using System.Linq;
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

    public GameLogic(int boardHeight, int boardWidth, int blockCount, Scoreboard scoreboard)
    {
      BoardState = new BoardState(boardHeight, boardWidth);
      _boardWidth = boardWidth;
      _fallingBlockGenerator = new FallingBlockGenerator(blockCount);
      FallingBlock = _fallingBlockGenerator.Next();
      _cleaner = new BoardCleaner(boardHeight, boardWidth);
      _scoreboard = scoreboard;
    }

    public void Update(float timeIncrement)
    {
      if (_scoreboard.IsComplete())
      {
        Debug.Log("level finished");
        throw new GameEndEvent();
      }

      for (var x = 0; x < _boardWidth; x++)
      {
        if (!BoardState.IsEmpty(x, 0))
        {
          Debug.Log("game ended");
          throw new GameEndEvent();
        }
      }

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
        if (cleaningResult.AnythingHappened())
        {
          Debug.Log($"penalty: {_scoreboard.TryContribute(cleaningResult.Poofs)}");

          if (!_scoreboard.IsComplete())
          {
            var objectiveString = "";
          
            foreach (var keyValuePair in _scoreboard.GetCurrentObjective().Objectives)
            {
              objectiveString += ($"{keyValuePair.Key}:{keyValuePair.Value}#");
            }

            Debug.Log(objectiveString);
          }
          
          throw new BoardCleaningEvent {CleaningResult = cleaningResult};
        }
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