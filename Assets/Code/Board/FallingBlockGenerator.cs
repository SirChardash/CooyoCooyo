using UnityEngine;
using Random = System.Random;

namespace Code.Board
{
  public class FallingBlockGenerator
  {

    private readonly Random _random = new Random();
    
    public FallingBlock Next()
    {
      return new FallingBlock {StaticCode = _random.Next(4) + 1, RotatingCode = _random.Next(4) + 1};
    }
  }
}