using System;
using UnityEngine;

namespace Code.Board
{
  public class FallingBlock
  {
    private float _timeToFall = 0.5f;
    private float _timeFalling;
    public Vector2Int StaticBlock = new Vector2Int(3, 1);
    public Vector2Int RotatingBlock = new Vector2Int(3, 0);
    public int StaticCode;
    public int RotatingCode;
    private Orientation _orientation = Orientation.Up;

    public void Update(float timeIncrement, BoardState boardState)
    {
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

      if (Input.GetKeyDown(KeyCode.UpArrow)) TryRotate(boardState);

      _timeFalling += timeIncrement;
      if (_timeFalling >= _timeToFall)
      {
        StaticBlock.y++;
        RotatingBlock.y++;
        _timeFalling -= _timeToFall;
      }

      if (Input.GetKeyDown(KeyCode.DownArrow))
      {
        _timeToFall /= 10;
      }
    }

    public float GetBlockProgress()
    {
      return _timeToFall / _timeFalling;
    }

    private void TryRotate(BoardState boardState)
    {
      switch (_orientation)
      {
        case Orientation.Up:
        {
          if (boardState.IsEmpty(StaticBlock.x + 1, StaticBlock.y))
          {
            _orientation = Orientation.Right;
            RotatingBlock.x = StaticBlock.x + 1;
            RotatingBlock.y = StaticBlock.y;
          }
          else if (boardState.IsEmpty(StaticBlock.x - 1, StaticBlock.y))
          {
            _orientation = Orientation.Right;
            RotatingBlock.x = StaticBlock.x;
            RotatingBlock.y = StaticBlock.y;
            StaticBlock.x -= 1;
          }

          break;
        }
        case Orientation.Right:
        {
          _orientation = Orientation.Down;
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
        case Orientation.Down:
        {
          if (boardState.IsEmpty(StaticBlock.x - 1, StaticBlock.y))
          {
            _orientation = Orientation.Left;
            RotatingBlock.x = StaticBlock.x - 1;
            RotatingBlock.y = StaticBlock.y;
          }
          else if (boardState.IsEmpty(StaticBlock.x + 1, StaticBlock.y))
          {
            _orientation = Orientation.Left;
            RotatingBlock.x = StaticBlock.x;
            RotatingBlock.y = StaticBlock.y;
            StaticBlock.x += 1;
          }

          break;
        }
        case Orientation.Left:
        {
          _orientation = Orientation.Up;
          RotatingBlock.x = StaticBlock.x;
          RotatingBlock.y = StaticBlock.y - 1;
          break;
        }
      }
    }

    private enum Orientation
    {
      Up,
      Down,
      Left,
      Right
    }
  }
}