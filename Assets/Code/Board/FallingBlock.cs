using UnityEngine;

namespace Code.Board
{
  public class FallingBlock
  {
    private float _timeToFall = 0.5f;
    private float _timeFalling;

    private bool _fallFast;

    public Vector2Int StaticBlock;
    public Vector2Int RotatingBlock;
    public readonly Block StaticCode;
    public readonly Block RotatingCode;
    public BlockOrientation Orientation = BlockOrientation.Up;

    public FallingBlock(Block staticCode, Block rotatingCode, int x)
    {
      StaticBlock = new Vector2Int(x, 1);
      RotatingBlock = new Vector2Int(x, 0);
      StaticCode = staticCode;
      RotatingCode = rotatingCode;
    }
    
    public void Update(float deltaTime, BoardState boardState)
    {
      if (Input.GetKeyDown(KeyCode.UpArrow)) TryRotate(boardState);
      _timeFalling += deltaTime;

      if (_fallFast) return;

      if (Input.GetKeyDown(KeyCode.LeftArrow)
          && boardState.IsEmpty(StaticBlock.x - 1, StaticBlock.y)
          && boardState.IsEmpty(RotatingBlock.x - 1, RotatingBlock.y))
      {
        StaticBlock.x--;
        RotatingBlock.x--;
      }
      else if (Input.GetKeyDown(KeyCode.RightArrow)
               && boardState.IsEmpty(StaticBlock.x + 1, StaticBlock.y)
               && boardState.IsEmpty(RotatingBlock.x + 1, RotatingBlock.y))
      {
        StaticBlock.x++;
        RotatingBlock.x++;
      }
    }

    public void DropDown()
    {
      StaticBlock.y++;
      RotatingBlock.y++;
      _timeFalling -= _timeToFall;
    }

    public void FallFast()
    {
      _timeToFall /= 10;
      _fallFast = true;
      _timeFalling = _timeToFall;
    }

    public float GetBlockProgress()
    {
      return _timeFalling / _timeToFall;
    }

    public bool ShouldDrop()
    {
      return GetBlockProgress() >= 1f;
    }

    private void TryRotate(BoardState boardState)
    {
      switch (Orientation)
      {
        case BlockOrientation.Up:
        {
          if (boardState.IsEmpty(StaticBlock.x + 1, StaticBlock.y))
          {
            Orientation = BlockOrientation.Right;
            RotatingBlock.x = StaticBlock.x + 1;
            RotatingBlock.y = StaticBlock.y;
          }
          else if (boardState.IsEmpty(StaticBlock.x - 1, StaticBlock.y))
          {
            Orientation = BlockOrientation.Right;
            RotatingBlock.x = StaticBlock.x;
            RotatingBlock.y = StaticBlock.y;
            StaticBlock.x -= 1;
          }

          break;
        }
        case BlockOrientation.Right:
        {
          Orientation = BlockOrientation.Down;
          if (boardState.IsEmpty(StaticBlock.x, StaticBlock.y + 1))
          {
            RotatingBlock.x = StaticBlock.x;
            RotatingBlock.y = StaticBlock.y + 1;
          }
          else
          {
            RotatingBlock.x = StaticBlock.x;
            RotatingBlock.y = StaticBlock.y;
            StaticBlock.y -= 1;
          }

          break;
        }
        case BlockOrientation.Down:
        {
          if (boardState.IsEmpty(StaticBlock.x - 1, StaticBlock.y))
          {
            Orientation = BlockOrientation.Left;
            RotatingBlock.x = StaticBlock.x - 1;
            RotatingBlock.y = StaticBlock.y;
          }
          else if (boardState.IsEmpty(StaticBlock.x + 1, StaticBlock.y))
          {
            Orientation = BlockOrientation.Left;
            RotatingBlock.x = StaticBlock.x;
            RotatingBlock.y = StaticBlock.y;
            StaticBlock.x += 1;
          }

          break;
        }
        case BlockOrientation.Left:
        {
          Orientation = BlockOrientation.Up;
          RotatingBlock.x = StaticBlock.x;
          RotatingBlock.y = StaticBlock.y - 1;
          break;
        }
      }
    }

    public enum BlockOrientation
    {
      Up,
      Down,
      Left,
      Right
    }
  }
}