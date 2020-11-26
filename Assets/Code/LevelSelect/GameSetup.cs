using System.Collections.Generic;
using Code.Board;
using UnityEngine;

namespace Code.LevelSelect
{
  public class GameSetup
  {
    public readonly int BlockCount;
    public readonly Dictionary<Block, Sprite> SpriteMapping;

    public GameSetup(int blockCount, Dictionary<Block, Sprite> spriteMapping)
    {
      BlockCount = blockCount;
      SpriteMapping = spriteMapping;
    }
  }
}