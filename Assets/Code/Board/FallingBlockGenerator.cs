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

    public MessBlocks Mess(int penalty)
    {
      if (penalty <= 0) return null;

      var randoms = new HashSet<int>();
      while (randoms.Count < penalty) randoms.Add(_random.Next(_boardWidth));

      return new MessBlocks(randoms.Select(x => new MessBlocks.MessBlock(x)).ToList());
    }
  }
}