using System.Linq;
using UnityEngine;

namespace Code.Board
{
  public class BoardCleaner
  {
    public CleaningResult TryClean(BoardState board, int startX, int startY)
    {
      var tempBoard = board.Clone();
      var fill = FloodFill(tempBoard, startX, startY);
      if (Count(fill) >= 4)
      {
        for (var x = 0; x < 8; x++)
        {
          for (var y = 0; y < 10; y++)
          {
            if (fill[y, x]) tempBoard[y, x] = 0;
          }
        }

        for (var x = 0; x < 8; x++)
        {
          for (var y = 8; y >= 0; y--)
          {
            if (tempBoard[y, x] != 0)
            {
              var newLocation = y;
              while (newLocation < 9 && tempBoard[newLocation + 1, x] == 0)
              {
                newLocation++;
              }

              var code = tempBoard[y, x];
              tempBoard[y, x] = 0;
              tempBoard[newLocation, x] = code;
            }
          }
        }
      }

      return new CleaningResult(tempBoard);
    }

    private static bool[,] FloodFill(int[,] map, int x, int y)
    {
      var fill = new bool[10, 8];
      FloodFill(fill, map, x, y);
      return fill;
    }

    private static void FloodFill(bool[,] fill, int[,] map, int x, int y)
    {
      fill[y, x] = true;
      if (x > 0 && map[y, x - 1] == map[y, x] && !fill[y, x - 1]) FloodFill(fill, map, x - 1, y);
      if (x < 7 && map[y, x + 1] == map[y, x] && !fill[y, x + 1]) FloodFill(fill, map, x + 1, y);
      if (y > 0 && map[y - 1, x] == map[y, x] && !fill[y - 1, x]) FloodFill(fill, map, x, y - 1);
      if (y < 9 && map[y + 1, x] == map[y, x] && !fill[y + 1, x]) FloodFill(fill, map, x, y + 1);
    }

    private static int Count(bool[,] fill)
    {
      return fill.Cast<bool>().Count(flag => flag);
    }
  }
}