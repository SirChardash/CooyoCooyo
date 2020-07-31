using UnityEngine;
using Random = System.Random;

namespace Code.Board
{
  public class FallingBlockGenerator
  {
    private readonly Random _random = new Random();
    private readonly int _blockCount;
    
    public FallingBlockGenerator(int blockCount)
    {
      _blockCount = blockCount;
    }
    
    public FallingBlock Next()
    {
      return new FallingBlock
      {
        StaticCode = (Block) (_random.Next(_blockCount) + 1),
        RotatingCode = (Block) (_random.Next(_blockCount) + 1)
      };
    }
  }
}