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
    public readonly int StartingX;

    public FallingBlockGenerator(int blockCount, int boardWidth)
    {
      _blockCount = blockCount;
      _boardWidth = boardWidth;
      StartingX = _boardWidth / 2 - (_boardWidth % 2 == 0 ? 1 : 0);
    }

    public FallingBlock Next()
    {
      return new FallingBlock(
        (Block) (_random.Next(_blockCount) + 1),
        (Block) (_random.Next(_blockCount) + 1),
        StartingX
      );
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