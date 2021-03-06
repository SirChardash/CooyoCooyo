﻿using System.Collections.Generic;
using Code.Common;

namespace Code.Board
{
  public class BoardCleaner
  {
    private readonly int _boardHeight;
    private readonly int _boardWidth;

    public BoardCleaner(int boardHeight, int boardWidth)
    {
      _boardHeight = boardHeight;
      _boardWidth = boardWidth;
    }

    public CleaningResult TryClean(BoardState board)
    {
      var boardStates = new List<Block[,]>();
      var poofs = new List<Poof>();
      bool poofHappened;
      var observedBoard = board.Clone();
      do
      {
        poofHappened = false;
        var poofMap = new bool[_boardHeight, _boardWidth];
        for (var x = 0; x < _boardWidth; x++)
        {
          for (var y = 0; y < _boardHeight; y++)
          {
            if (!poofMap[y, x] && observedBoard[y, x] != Block.Empty && observedBoard[y, x] != Block.Mess)
            {
              var result = FloodFill(observedBoard, x, y);
              if (result.Count > 3)
              {
                Array2DUtils.Or(poofMap, result.FillMap);
                poofs.Add(new Poof {Block = observedBoard[y, x], Count = result.Count});
                poofHappened = true;
              }
            }
          }
        }

        var poofState = new Block[_boardHeight, _boardWidth];
        var fallDownState = (Block[,]) observedBoard.Clone();
        for (var x = 0; x < _boardWidth; x++)
        {
          for (var y = 0; y < _boardHeight; y++)
          {
            poofState[y, x] = poofMap[y, x] ? Block.Poof1 : observedBoard[y, x];
            fallDownState[y, x] = poofMap[y, x] ? 0 : fallDownState[y, x];
          }
        }

        FallDown(fallDownState);

        if (poofHappened)
        {
          boardStates.Add(poofState);
          boardStates.Add(fallDownState);
        }

        observedBoard = (Block[,]) fallDownState.Clone();
      } while (poofHappened);

      return new CleaningResult(boardStates, poofs);
    }

    private static void FallDown(Block[,] board)
    {
      for (var x = 0; x < board.GetLength(1); x++)
      {
        for (var y = board.GetLength(0) - 1; y >= 0; y--)
        {
          if (board[y, x] == 0) continue;
          var newLocation = y;
          while (newLocation < board.GetLength(0) - 1 && board[newLocation + 1, x] == 0)
          {
            newLocation++;
          }

          var code = board[y, x];
          board[y, x] = 0;
          board[newLocation, x] = code;
        }
      }
    }

    private FloodFillResult FloodFill(Block[,] map, int x, int y)
    {
      var result = new FloodFillResult(_boardHeight, _boardWidth);
      FloodFill(result, map, x, y);
      return result;
    }

    private void FloodFill(FloodFillResult result, Block[,] map, int x, int y)
    {
      result.FillMap[y, x] = true;
      if (map[y, x] != Block.Mess) result.Count++;

      if (x > 0
          && (map[y, x - 1] == map[y, x] || map[y, x - 1] == Block.Mess)
          && !result.FillMap[y, x - 1]) FloodFill(result, map, x - 1, y);

      if (x < _boardWidth - 1
          && (map[y, x + 1] == map[y, x] || map[y, x + 1] == Block.Mess)
          && !result.FillMap[y, x + 1]) FloodFill(result, map, x + 1, y);

      if (y > 0
          && (map[y - 1, x] == map[y, x] || map[y - 1, x] == Block.Mess)
          && !result.FillMap[y - 1, x]) FloodFill(result, map, x, y - 1);

      if (y < _boardHeight - 1
          && (map[y + 1, x] == map[y, x] || map[y + 1, x] == Block.Mess)
          && !result.FillMap[y + 1, x]) FloodFill(result, map, x, y + 1);
    }

    private class FloodFillResult
    {
      public int Count { get; set; }
      public bool[,] FillMap { get; }

      public FloodFillResult(int mapHeight, int mapWidth)
      {
        FillMap = new bool[mapHeight, mapWidth];
      }
    }
  }
}