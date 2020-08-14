using System.Collections.Generic;
using UnityEngine;

namespace Code.Board
{
  public class MessBlocks
  {
    private float _timeToFall = 0.1f;
    private float _timeFalling;
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
      _timeFalling += deltaTime;
    }

    public float GetBlockProgress()
    {
      return _timeToFall / _timeFalling;
    }

    public bool ShouldDrop()
    {
      return GetBlockProgress() >= 1f;
    }

    public void ConfirmFalling()
    {
      _timeFalling -= _timeToFall;
    }

    public bool IsEmpty()
    {
      return Blocks.Count == 0;
    }

    public class MessBlock
    {
      public Vector2Int Position;
      public readonly Block Block = Block.Mess;
      
      public MessBlock(int x)
      {
        Position = new Vector2Int(x, 0);
      }

      public void DropDown()
      {
        Position.y++;
      }
    }
  }
}