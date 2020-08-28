using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Board
{
  public class MessBlocks
  {
    public readonly List<MessBlock> Blocks;

    public MessBlocks(List<MessBlock> blocks)
    {
      Blocks = blocks;
    }

    public void Confirm(MessBlock messBlock)
    {
      Blocks.Remove(messBlock);
    }

    public void Update(float deltaTime)
    {
      Blocks.ForEach(block =>
      {
        block.Update(deltaTime);
      });
    }

    public bool IsEmpty()
    {
      return Blocks.Count == 0;
    }

    public List<MessBlock> GetShouldDropBlocks()
    {
      return Blocks.Where(block => block.ShouldDrop()).ToList();
    }

    public class MessBlock
    {
      public Vector2Int ExpectedPosition;
      public readonly Block Block = Block.Mess;
      private readonly float _timeToFall;
      private float _timeFalling;
      
      public float GetBlockProgress()
      {
        return _timeFalling / _timeToFall;
      }
      
      public bool ShouldDrop()
      {
        return GetBlockProgress() >= 1f;
      }

      public void Update(float deltaTime)
      {
        _timeFalling += deltaTime;
      }
      
      public MessBlock(int x, int y)
      {
        _timeToFall = 1f * (y / 10f);
        ExpectedPosition = new Vector2Int(x, y);
      }
    }
  }
}