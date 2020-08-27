using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.Board
{
  public class FallingBlockGenerator
  {
    private readonly Random _random = new Random();
    private readonly int _blockCount;
    private readonly int _boardWidth;

    public FallingBlockGenerator(int blockCount, int boardWidth)
    {
      _blockCount = blockCount;
      _boardWidth = boardWidth;
    }

    public FallingBlock Next()
    {
      return new FallingBlock
      {
        StaticCode = (Block) (_random.Next(_blockCount) + 1),
        RotatingCode = (Block) (_random.Next(_blockCount) + 1)
      };
    }

    public MessBlocks Mess(int penalty, BoardState board)
    {
      if (penalty <= 0) return null;

      var randoms = new HashSet<int>();
      while (randoms.Count < penalty) randoms.Add(_random.Next(_boardWidth));

      var y = new int[_boardWidth];
      for (var x = 0; x < _boardWidth; x++)
      {
        while (board.IsEmpty(x, y[x] + 1)) y[x]++;
      }

      return new MessBlocks(randoms.Select(x => new MessBlocks.MessBlock(x, y[x])).ToList());
    }
  }
}